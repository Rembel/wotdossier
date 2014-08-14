using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;

namespace WotDossier
{
    /// <summary>
    /// Class implementing support for "minimize to tray" functionality.
    /// </summary>
    public static class MinimizeToTray
    {
        /// <summary>
        /// Enables "minimize to tray" behavior for the specified Window.
        /// </summary>
        /// <param name="window">Window to enable the behavior for.</param>
        public static void Enable(Window window)
        {
            // No need to track this instance; its event handlers will keep it alive
            new MinimizeToTrayInstance(window);
        }

        /// <summary>
        /// Class implementing "minimize to tray" functionality for a Window instance.
        /// </summary>
        private class MinimizeToTrayInstance
        {
            private readonly Window _window;
            private NotifyIcon _notifyIcon;
            private bool _balloonShown;

            /// <summary>
            /// Initializes a new instance of the MinimizeToTrayInstance class.
            /// </summary>
            /// <param name="window">Window instance to attach to.</param>
            public MinimizeToTrayInstance(Window window)
            {
                Debug.Assert(window != null, "window parameter is null.");
                _window = window;
                _window.SourceInitialized += OnSourceInitialized;
                _window.Closing += WindowOnClosing;
                _window.StateChanged += HandleStateChanged;
            }

            private void WindowOnClosing(object sender, CancelEventArgs cancelEventArgs)
            {
                UnregisterHotKey(_window);
            }

            private void OnSourceInitialized(object sender, EventArgs eventArgs)
            {
                RegisterRestoreWindowHotKey(_window);
            }

            /// <summary>
            /// Handles the Window's StateChanged event.
            /// </summary>
            /// <param name="sender">Event source.</param>
            /// <param name="e">Event arguments.</param>
            private void HandleStateChanged(object sender, EventArgs e)
            {
                if (_notifyIcon == null)
                {
                    // Initialize NotifyIcon instance "on demand"
                    _notifyIcon = new NotifyIcon();
                    _notifyIcon.Icon = Icon.ExtractAssociatedIcon(Assembly.GetEntryAssembly().Location);
                    _notifyIcon.MouseClick += HandleNotifyIconOrBalloonClicked;
                    _notifyIcon.BalloonTipClicked += HandleNotifyIconOrBalloonClicked;
                }
                // Update copy of Window Title in case it has changed
                _notifyIcon.Text = _window.Title;

                // Show/hide Window and NotifyIcon
                var minimized = (_window.WindowState == WindowState.Minimized);
                _window.ShowInTaskbar = !minimized;
                _notifyIcon.Visible = minimized;
                if (minimized && !_balloonShown)
                {
                    // If this is the first time minimizing to the tray, show the user what happened
                    _notifyIcon.ShowBalloonTip(1000, null, _window.Title, ToolTipIcon.None);
                    _balloonShown = true;
                }
            }

            /// <summary>
            /// Handles a click on the notify icon or its balloon.
            /// </summary>
            /// <param name="sender">Event source.</param>
            /// <param name="e">Event arguments.</param>
            private void HandleNotifyIconOrBalloonClicked(object sender, EventArgs e)
            {
                // Restore the Window
                _window.WindowState = WindowState.Maximized;

                // Get the window to the front.
                _window.Topmost = true;
                _window.Topmost = false;

                // 'Steal' the focus.
                _window.Activate();
            }

            #region Restore hot key

            private static HwndSource _source;
            private const int HOTKEY_ID = 9000;

            private void RegisterRestoreWindowHotKey(Window window)
            {
                var helper = new WindowInteropHelper(window);
                _source = HwndSource.FromHwnd(helper.Handle);
                _source.AddHook(HwndHook);
                RegisterHotKey(window);
            }

            private void RegisterHotKey(Window window)
            {
                var helper = new WindowInteropHelper(window);
                const uint VK_F10 = 0x79;
                const uint MOD_CTRL = 0x0002;
                if (!NativeMethods.RegisterHotKey(helper.Handle, HOTKEY_ID, MOD_CTRL, VK_F10))
                {
                    // handle error
                }
            }

            private void UnregisterHotKey(Window window)
            {
                _source.RemoveHook(HwndHook);
                _source = null;
                var helper = new WindowInteropHelper(window);
                NativeMethods.UnregisterHotKey(helper.Handle, HOTKEY_ID);
            }

            private IntPtr HwndHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
            {
                const int WM_HOTKEY = 0x0312;
                
                if((msg == WM_HOTKEY && wParam.ToInt32() == HOTKEY_ID)
                    || msg == NativeMethods.WM_SHOWFIRSTINSTANCE)
                {
                    OnHotKeyPressed();
                    handled = true;
                }
                
                return IntPtr.Zero;
            }

            private void OnHotKeyPressed()
            {
                HandleNotifyIconOrBalloonClicked(null, null);
            }

            #endregion
        }
    }
}
