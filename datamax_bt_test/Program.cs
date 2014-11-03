using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace datamax_bt_test
{
    static class Program
    {
        // Consts
        const long ERROR_ALREADY_EXISTS = 183;
        private const int BS_MULTILINE = 0x00002000;
        private const int GWL_STYLE = -16;

        // Dll imports
        [DllImport("coredll.dll", SetLastError = true)]
        public static extern int GetLastError();

        [DllImport("coredll.dll", SetLastError = true)]
        public static extern int CreateMutex(int lpMutexAttributes, int bInitiaOwner, string lpName);

        [DllImport("coredll.dll", SetLastError = true)]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("coredll.dll", SetLastError = true)]
        private static extern IntPtr FindWindow(string className, string wndName);

        [DllImport("coredll.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("coredll.dll", SetLastError = true)]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [MTAThread]
        static void Main()
        {
            // Run
            HideStatusBar();
            Application.Run(new Form_BtTest());
            Application.Exit();
            ShowStatusBar();
        }

        /// <summary>
        /// Hide task bar
        /// </summary>
        public static void HideStatusBar()
        {
            ShowWindow(FindWindow("HHTaskBar", null), 0);
        }

        /// <summary>
        /// Show task bar
        /// </summary>
        public static void ShowStatusBar()
        {
            ShowWindow(FindWindow("HHTaskBar", null), 1);
        }
    }
}