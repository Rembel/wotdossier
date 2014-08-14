using System;
using System.Runtime.InteropServices;

namespace WotDossier
{
    internal static class NativeMethods
    {
        public static readonly int WM_SHOWFIRSTINSTANCE = RegisterWindowMessage("WM_SHOWFIRSTINSTANCE");
        private const int SW_MAXIMIZE = 3;
        private const int SW_RESTORE = 9;

        [DllImport("User32.dll", CharSet = CharSet.Unicode)]
        public static extern IntPtr FindWindow(string className, string windowName);
        [DllImport("User32.dll")]
        public static extern int SetForegroundWindow(int hWnd);
        [DllImport("User32.dll")]
        public static extern bool ShowWindow(int hWnd, int nCmdShow);
        [DllImport("user32")]
        public static extern bool PostMessage(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam);
        [DllImport("user32", CharSet = CharSet.Unicode)]
        public static extern int RegisterWindowMessage(string message);

        [DllImport("User32.dll")]
        public static extern bool RegisterHotKey(
            [In] IntPtr hWnd,
            [In] int id,
            [In] uint fsModifiers,
            [In] uint vk);

        [DllImport("User32.dll")]
        public static extern bool UnregisterHotKey(
            [In] IntPtr hWnd,
            [In] int id);

        /// <summary>
        /// Activates the window.
        /// </summary>
        /// <param name="name">The name of the window.</param>
        public static void ActivateWindow(string name)
        {
            IntPtr hWnd = FindWindow(null, name);
            if (hWnd.ToInt32() > 0)
            {
                SetForegroundWindow(hWnd.ToInt32());
                ShowWindow(hWnd.ToInt32(), SW_RESTORE);
            }

            PostMessage(hWnd, WM_SHOWFIRSTINSTANCE, IntPtr.Zero, IntPtr.Zero);
        }
    }
}
