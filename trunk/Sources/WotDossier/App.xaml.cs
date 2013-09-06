using System;
using System.ComponentModel.Composition;
using System.Data.SqlClient;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using Common.Logging;
using WotDossier.Applications;
using WotDossier.Framework;
using WotDossier.Framework.Forms;
using WotDossier.Framework.Presentation.Services;

namespace WotDossier
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static readonly ILog _log = LogManager.GetLogger("App");

        private ApplicationController _controller;
        
        [Import]
        public ApplicationController Controller
        {
            get { return _controller; }
            set { _controller = value; }
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            //set app lang
            var culture = new CultureInfo(SettingsReader.Get().Language);
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

#if !DEBUG
            // Don't handle the exceptions in Debug mode because otherwise the Debugger wouldn't
            // jump into the code when an exception occurs.
            DispatcherUnhandledException += AppDispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += AppDomainUnhandledException;
#endif

            // start application
            try
            {
                CompositionContainerFactory.Instance.Container.SatisfyImportsOnce(this);
                Controller.Run();
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
            if (!isTerminating)
            {
                if (e is SqlException)
                {
                    WpfMessageBox.Show(WotDossier.Resources.Resources.Msg_SqlExceptionOccurred, ApplicationInfo.ProductName, WpfMessageBoxButton.OK, WPFMessageBoxImage.Error);
                }
                else
                {
                    WpfMessageBox.Show(WotDossier.Resources.Resources.Msg_ExceptionOccurred, ApplicationInfo.ProductName, WpfMessageBoxButton.OK, WPFMessageBoxImage.Error);
                }
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            if (Controller != null)
            {
                CompositionContainerFactory.Instance.Container.Dispose();
                Controller.Dispose();
            }
            base.OnExit(e);
        }
    }
}
