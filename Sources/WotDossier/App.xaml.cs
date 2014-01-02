using System;
using System.ComponentModel.Composition;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using Common.Logging;
using WotDossier.Applications;
using WotDossier.Applications.Update;
using WotDossier.Framework;
using WotDossier.Framework.Presentation.Services;
using Configuration = NHibernate.Cfg.Configuration;

namespace WotDossier
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static readonly ILog _log = LogManager.GetLogger("App");

        private Mutex _mutex;
        private const string INSTANCE_ID = "1D4E6094-31A4-402F-AB5B-9E4000A4ED48";

        private ApplicationController _controller;
        
        [Import]
        public ApplicationController Controller
        {
            get { return _controller; }
            set { _controller = value; }
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            /*App config appSettings section encrypt\decrypt*/
            //string appName = "WotDossier.exe";
            //System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(appName);
            //AppSettingsSection section = config.GetSection("appSettings") as AppSettingsSection;
            //if (section.SectionInformation.IsProtected)
            //{
            //    section.SectionInformation.UnprotectSection();
            //}
            //else
            //{
            //    section.SectionInformation.ProtectSection("DataProtectionConfigurationProvider");
            //}
            //config.Save();

            bool isNewInstance;
            _mutex = new Mutex(true, INSTANCE_ID, out isNewInstance);
            if (!isNewInstance)
            {
                //WPFMessageBox.Show(Gui.Properties.Resources.Msg_ApplicationIs_Already_Started, Gui.Properties.Resources.MessageBox_Title_Error, WPFMessageBoxImage.Error);
                Shutdown();
                //activate already opened app window
                NativeMethods.ActivateWindow(ApplicationInfo.ProductName);
                return;
            }
            
#if !DEBUG
            // Don't handle the exceptions in Debug mode because otherwise the Debugger wouldn't
            // jump into the code when an exception occurs.
            DispatcherUnhandledException += AppDispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += AppDomainUnhandledException;
#endif

            // start application
            try
            {
                DatabaseManager manager = new DatabaseManager();
                manager.InitDatabase();
                manager.Update();

                //set app lang
                var culture = new CultureInfo(SettingsReader.Get().Language);
                Thread.CurrentThread.CurrentCulture = culture;
                Thread.CurrentThread.CurrentUICulture = culture;

                Controller = CompositionContainerFactory.Instance.Container.GetExport<ApplicationController>().Value;

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
                    MessageBox.Show(e.ToString(), ApplicationInfo.ProductName, MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show(e.ToString(), ApplicationInfo.ProductName, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            if (Controller != null)
            {
                CompositionContainerFactory.Instance.Container.Dispose();
            }
            base.OnExit(e);
        }
    }
}
