using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Datalogic.API;
using System.IO.Ports;
using ZgLib;
using System.Threading;
using System.IO;
using System.Windows.Forms;

namespace ZgPrinter
{
    public class Printer
    {
        // Parent-calling form
        private TextBox _logTextbox;

        // Connection information
        private string _hwAddr = "";
        private string _comAddr = "";
        private bool _paired;
        private bool _pairing=false;
        private bool _cancelConnection=false;
        private int _ticks;
        private string _ticksDiff;

        // Global flag
        private bool _keepAlive = true;
        private static SerialPort _sport = new SerialPort();
        private bool _distanceFault = false;

        // Command management
        private Queue<Tuple<DelegateMessageHandling, int>> _commandQueue;
        private delegate void DelegateMessageHandling(string message, int tickStart);
        private DelegateMessageHandling _delegateStatus;
        private int _queueId = 0;

        /// <summary>
        /// Printer constructor
        /// </summary>
        /// <param name="hwAddr"></param>
        public Printer(string hwAddr, TextBox logTextbox)
        {
            _logTextbox = logTextbox;

            // Set hardware address
            SetHwaddr(hwAddr);

            // Set timeouts
            try
            {
                _sport.ReadTimeout = 1000;
                _sport.WriteTimeout = 1000;
            }
            catch (IOException) { }

            // Set data received event
            _sport.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
            _commandQueue = new Queue<Tuple<DelegateMessageHandling, int>>();
        }

        /// <summary>
        /// Log event in the log textbox
        /// </summary>
        /// <param name="log">Message to log</param>
        private void LogInTextbox(string log)
        {
            _logTextbox.Invoke((Action)delegate
            {
                _logTextbox.Text += log + Environment.NewLine;
                _logTextbox.Select(_logTextbox.TextLength, 0);
                _logTextbox.ScrollToCaret();
            });
        }

        /// <summary>
        /// Log event in the log textbox
        /// </summary>
        /// <param name="log">Message to log</param>
        private void LogInTextbox(string log, bool nonewline)
        {
            if (nonewline)
                LogInTextbox(log);
            else
                LogInTextbox(log + Environment.NewLine);
        }

        /// <summary>
        /// Data received event handler
        /// </summary>
        /// <param name="sender">Serial port</param>
        /// <param name="e">Event data</param>
        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            string inData = _sport.ReadExisting();
            foreach (string readStr in inData.Split('\r'))
            {
                if (readStr != "")
                {
                    if (_commandQueue.Count > 0)
                    {
                        try
                        {
                            Tuple<DelegateMessageHandling, int> tComTick = _commandQueue.Dequeue();
                            tComTick.Item1(readStr, tComTick.Item2);
                        }
                        catch (Exception) { }
                    }
                }
            }
        }

        /// <summary>
        /// Status meter to measure the difference between sent and received keep alive messages
        /// </summary>
        /// <param name="alive_message">A keep alive message from the printer, whatever</param>
        private void AliveUpdate(string aliveMessage, int startTicks)
        {
            LogInTextbox("Receiving status [QS" + _commandQueue.Count.ToString() + "] after " + (Environment.TickCount - startTicks).ToString() + "ms");
        }

        private void SetTicks()
        {
            _ticks = Environment.TickCount;
        }

        private string UpdateTicksdiff()
        {
            if (_ticks > 0)
            {
                _ticksDiff = (Environment.TickCount - _ticks).ToString() + "ms";
            }
            else
                _ticksDiff = "Fail";

            return _ticksDiff;
        }

        public string GetTicksDiff()
        {
            string ticksDiffTemp = _ticksDiff;
            _ticksDiff = "Waiting...";
            return ticksDiffTemp;
        }


        public void SetHwaddr(string hwAddr)
        {
            _paired = false;
            _hwAddr = hwAddr;
        }
        public void CancelConnection()
        {
            _cancelConnection = true;
        }

        /// <summary>
        /// Get connection state flag
        /// </summary>
        /// <returns>Connection is cancelled</returns>
        public bool CancelledConnection()
        {
            return _cancelConnection;
        }

        /// <summary>
        /// Pair the printer to the device
        /// </summary>
        public void Pair() { Pair(false); }

        /// <summary>
        /// Pair the printer to the device a
        /// </summary>
        /// <param name="clear_pairings"></param>
        public void Pair(bool clear_pairings)
        {
            _pairing = true;
            _cancelConnection = false;

            if (clear_pairings)
            {
                LogInTextbox("Clearing all pairings", true);
                Device.BtClearAllPairings();
                LogInTextbox("");
            }

            if (_hwAddr == "" || _hwAddr.Length != 12)
            {
                LogInTextbox("Wrong HW address, cannot pair.");
                return;
            }
            if (SerialPort.GetPortNames().Length == 0)
            {
                LogInTextbox("No free COM port, cannot pair.");
                return;
            }

            LogInTextbox("Pairing printer...");
            for (int com = 0; com < 10; com++)
            {
                if (_cancelConnection)
                {
                    _pairing = false;
                    return;
                }

                try
                {
                    LogInTextbox("Trying pairing printer " + _hwAddr + " on COM" + com + "...");
                    if (Device.BtCreateSerialPairing((_hwAddr + "\0\0").ToCharArray(), com))
                    {
                        _comAddr = "COM" + com;
                        LogInTextbox("Printer " + _hwAddr + " paired on port " + _comAddr + ".");
                        Connect();
                        _paired = true;
                        _pairing = false;
                        return;
                    }
                }
                catch (Exception) { continue; }
            }

            LogInTextbox("Printer pairing failure.");
            _pairing = false;
            return;
        }

        public bool IsPairing()
        {
            return _pairing;
        }
        public bool IsPaired()
        {
            return _paired;
        }

        public void Dispose()
        {
            Close();

            try
            {
                LogInTextbox("Disposing printer pairing.");
                _sport.Dispose();
            }
            catch (IOException) { }
        }

        public void Close()
        {
            LogInTextbox("Closing printer pairing.");
            _sport.Close();
        }

        /// <summary>
        /// Connect to the printer : 3 tries
        /// </summary>
        public void Connect() { Connect(3); }

        /// <summary>
        /// Connect to the printer : (3 - tries) tries
        /// </summary>
        /// <param name="tries_left">Tries left</param>
        public void Connect(int triesLeft)
        {
            triesLeft--;

            if (_cancelConnection)
                return;

            LogInTextbox("Connecting to printer " + _hwAddr + " on port " + _comAddr, true);
            try
            {
                // Lets clear the command queue
                _commandQueue.Clear();

                // Close port if it's open
                if (_sport.IsOpen)
                    _sport.Close();
                
                // Re-open it
                _sport.PortName = _comAddr;
                _sport.Open();
                LogInTextbox("");
            }
            catch (IOException expt)
            {
                // Try again
                if (triesLeft > 0)
                {
                    Log.Failed();
                    LogInTextbox("Trying again... (" + (triesLeft) + " tries left)");
                    Connect(triesLeft);
                }

                // But not too long !
                else
                {
                    Log.Failed();
                    LogInTextbox("Connection failed.");
                    throw expt;
                }
            }
        }

        // Harass remote device to keep connection alive
        public void KeepAlive()
        {
            _keepAlive = true;

            try
            {
                if (!_sport.IsOpen)
                    Connect();

                if (_sport.IsOpen)
                {
                    try
                    {
                        // Request status
                        StatusCommand statuscommand = new StatusCommand();
                        _sport.Write(statuscommand.get_buffer(), 0, statuscommand.get_buffer().Length);
                        _commandQueue.Enqueue(new Tuple<DelegateMessageHandling, int>(AliveUpdate, Environment.TickCount));
                        LogInTextbox("Sending status request [QS" + _commandQueue.Count + "]");
                    }
                    catch (TimeoutException)
                    {
                        LogInTextbox("[timeout] couldn't send keepAlive, need to reconnect.");
                        _distanceFault = true;
                    }
                    catch (Exception)
                    {
                        LogInTextbox("[except] couldn't send keepAlive.");
                    }
                }
            }
            catch (Exception expt)
            {
                if (!(expt.InnerException is InvalidOperationException))
                {
                    // Ask operator to get closer to the printer
                    try { Connect(); }
                    catch (Exception) { }
                }
            }

            // Continue
            _keepAlive = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool KeepAlivePending()
        {
            return _keepAlive;
        }
    }
}
