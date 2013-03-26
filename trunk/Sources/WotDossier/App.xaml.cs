using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using Common.Logging;
using WotDossier.Applications;
using System.Linq;
using WotDossier.Dal;

namespace WotDossier
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static readonly ILog _log = LogManager.GetLogger("App");

        private ApplicationController _controller;

        protected override void OnStartup(StartupEventArgs e)
        {
            SettingsReader reader = new SettingsReader(WotDossierSettings.SettingsPath);
            //set app lang
            var culture = new CultureInfo(reader.Get().Language);
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

#if (DEBUG != true)
            // Don't handle the exceptions in Debug mode because otherwise the Debugger wouldn't
            // jump into the code when an exception occurs.
            DispatcherUnhandledException += AppDispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += AppDomainUnhandledException;
#endif

            // start application
            _controller = new ApplicationController();
            try
            {
                _controller.Run(new MainWindow());
            }
            catch (Exception exception)
            {
                HandleException(exception, false);
                Shutdown();
            }

            base.OnStartup(e);
        }

        private void AppDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            HandleException(e.Exception, false);
            e.Handled = true;
        }

        private static void AppDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            HandleException(e.ExceptionObject as Exception, e.IsTerminating);
        }

        private static void HandleException(Exception e, bool isTerminating)
        {
            if (e == null)
            {
                return;
            }

            //Trace.TraceError(e.ToString());
            _log.Error(e);
            //if (!isTerminating)
            //{
            //    if (e is SqlException || e is EntityException)
            //    {
            //        WpfMessageBox.Show(Gui.Properties.Resources.Msg_SqlExceptionOccurred, ApplicationInfo.ProductName, WpfMessageBoxButton.OK, WPFMessageBoxImage.Error);
            //    }
            //    else
            //    {
            //        WpfMessageBox.Show(string.Format(CultureInfo.CurrentCulture, "{0}", e), ApplicationInfo.ProductName, WpfMessageBoxButton.OK, WPFMessageBoxImage.Error);
            //    }
            //}
        }

        protected override void OnExit(ExitEventArgs e)
        {
            if (_controller != null)
            {
                _controller.Dispose();
            }
            base.OnExit(e);
        }
    }
}
