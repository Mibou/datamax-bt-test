using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace ZgLib
{
    public static class Log
    {
        // Write a message to the logs with an exception name
        public static void Write(string message, string exceptionName)
        {
            Write(exceptionName + " : " + message);
        }
        public static void Write(Exception expt)
        {
            Write(expt.GetType().ToString() + " " + expt.Message);
        }

        public static void Write(string message)
        {
            Write(message, false);
        }

        // Write a message to the logs
        public static void Write(string message, bool waitforit)
        {
#if DEBUG
            if (waitforit)
                Debug.Write(message);
            else
                Debug.WriteLine(message);

            StreamWriter sWriter = null;

            try
            {
                string dirPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase) + "\\logs";

                if (!Directory.Exists(dirPath))
                    Directory.CreateDirectory(dirPath);

                sWriter = new StreamWriter(dirPath + "\\log.txt");
                sWriter.WriteLine(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") + " - " + message);
                sWriter.Close();
            }
            catch (Exception)
            {
            }
            finally
            {
                if (sWriter != null)
                    sWriter.Close();
            }
#endif
        }

        // Write a message to the logs
        public static void Finished()
        {
#if DEBUG
            Debug.WriteLine(" -- Done.");
#endif
        }

        // Write a message to the logs
        public static void Failed()
        {
#if DEBUG
            Debug.WriteLine(" -- Failed.");
#endif
        }
    }

}
