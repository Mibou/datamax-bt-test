using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZgPrinter;
using System.Threading;
using ZgRemoteApp;
using Datalogic.API;
using ZgLib;

namespace datamax_bt_test
{
    public partial class Form_BtTest : Form
    {
        Printer printer;
        LaserManager lm;
        System.Threading.Timer check_ticksdiff;
        System.Threading.Timer keepAliveTimer;

        public Form_BtTest()
        {
            // Initializing laser manager
            lm = new LaserManager();

            // Initializing components (form controls)
            InitializeComponent();

            // Focusing on hardware address textbox
            txt_HwAddr.Focus();

            // Enabling laser
            lm.Laser_Enable();

            // Initializing bluetooth
            Device.BtInitialize();

            // Enabling bluetooth
            Device.BtEnable(true);

            // Adding listing on title label to close app
            this.Click += CloseEvent;

            // Checking all controls
            foreach (Control ctrl in this.Controls)
            {
                // Moving all panels to 0,0 location
                if (ctrl.GetType() == typeof(Panel) && ctrl.Parent is Form_BtTest)
                {
                    ctrl.Visible = false;
                    ctrl.Location = new Point(0, 0);
                }
            }

            // Display main panel
            pnl_Main.Show();
        }

        private void CloseEvent(object sender, EventArgs e) {
            this.Close();
        }

        public void keepalive_state(bool state)
        {
            if (state)
                keepAliveTimer.Change(6000, Timeout.Infinite);
            else
                keepAliveTimer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        // Harass remote device to keep connection alive
        private void keepAliveTimerTimer_Tick(object sender)
        {

            // Threaded connection
            Thread t = new Thread(new ThreadStart(printer.KeepAlive));
            t.Priority = ThreadPriority.AboveNormal;
            t.Start();
            while (printer.KeepAlivePending())
            {
                Application.DoEvents();
            }
            t.Join();

            // Restart timer
            Log.Write("Timer keepAliveTimerTimer_Tick");
            keepAliveTimer.Change(1000, Timeout.Infinite);
        }

        private void btn_Connect_Click(object sender, EventArgs e)
        {
            if (printer != null && printer.IsPaired())
            {
                DisconnectPrinter();

                // Changing button label
                btn_Connect.Text = "Connect";
            }
            else
            {
                // Changing button label
                btn_Connect.Text = "Connecting...";

                if (printer == null)
                    printer = new Printer(txt_HwAddr.Text, txt_Log);
                else
                    printer.SetHwaddr(txt_HwAddr.Text);

                // Threaded connection
                Cursor.Current = Cursors.WaitCursor;
                Thread t = new Thread(new ThreadStart(printer.Pair));
                t.Priority = ThreadPriority.AboveNormal;
                t.Start();
                Thread.Sleep(1000);
                while (printer.IsPairing())
                {
                    Application.DoEvents();
                    Thread.Sleep(1000);
                }
                t.Join();
                Cursor.Current = Cursors.Default;

                // Connection status
                if (printer != null && printer.IsPaired())
                {
                    // Changing button label
                    btn_Connect.Text = "Connected";
                    Thread.Sleep(1000);
                    btn_Connect.Text = "Disconnect";

                    // Staying aliiii-iiiii-iiiive (tribute to the bee gees)
                    if (keepAliveTimer == null)
                        keepAliveTimer = new System.Threading.Timer(keepAliveTimerTimer_Tick, null, 0, Timeout.Infinite);
                }
                else
                {
                    // Changing button label
                    btn_Connect.Text = "Connect";
                }
            }
        }

        private void txt_HwAddr_GotFocus(object sender, EventArgs e)
        {
            DisconnectPrinter();

            // Changing button label
            btn_Connect.Text = "Connect";

            // Timer select all
            var timer = new System.Windows.Forms.Timer { Interval = 100, Enabled = true };
            timer.Tick += (EventHandler)delegate
            {
                txt_HwAddr.Focus();
                txt_HwAddr.SelectAll();
                timer.Dispose();
            };
        }

        private void DisconnectPrinter()
        {
            // Disconnecting printer
            if (printer != null)
            {
                printer.Close();
                printer.Dispose();
            }
        }

        private void Form_BtTest_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    this.Close();
                    break;
            }
        }
    }
}