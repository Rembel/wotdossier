using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace WotDossier
{
    internal static class NativeMethods
    {
        private const int SW_MAXIMIZE = 3;

        [DllImport("User32.dll")]
        public static extern int FindWindow(String ClassName, String WindowName);
        [DllImport("User32.dll")]
        public static extern int SetForegroundWindow(int hWnd);
        [DllImport("User32.dll")]
        public static extern bool ShowWindow(int hWnd, int nCmdShow);

        /// <summary>
        /// Activates the window.
        /// </summary>
        /// <param name="name">The name of the window.</param>
        public static void ActivateWindow(string name)
        {
            int hWnd;
            string tx = null;
            foreach (Process proc in Process.GetProcesses())
            {
                tx = proc.MainWindowTitle;
                if (tx == name)
                {
                    tx = proc.MainWindowTitle;
                    hWnd = proc.Handle.ToInt32(); break;
                }
            }
            hWnd = FindWindow(null, tx);
            if (hWnd > 0)
            {
                SetForegroundWindow(hWnd);
                ShowWindow(hWnd, SW_MAXIMIZE);
            }
        }
    }
}
