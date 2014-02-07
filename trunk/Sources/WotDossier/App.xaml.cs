using System;
using System.ComponentModel.Composition;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using Common.Logging;
using SimpleInjector;
using WotDossier.Applications;
using WotDossier.Applications.Logic;
using WotDossier.Applications.Update;
using WotDossier.Applications.View;
using WotDossier.Applications.ViewModel;
using WotDossier.Applications.ViewModel.Replay;
using WotDossier.Dal;
using WotDossier.Dal.NHibernate;
using WotDossier.Framework;
using WotDossier.Framework.Applications.Services;
using WotDossier.Framework.Presentation.Services;
using WotDossier.Views;
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

                // Registrations here
                CompositionContainerFactory.Instance.Register<ApplicationController, ApplicationController>(Lifestyle.Singleton);
                CompositionContainerFactory.Instance.Register<ShellViewModel, ShellViewModel>(Lifestyle.Singleton);
                CompositionContainerFactory.Instance.Register<SettingsViewModel, SettingsViewModel>(Lifestyle.Transient);
                CompositionContainerFactory.Instance.Register<ClanSearchViewModel, ClanSearchViewModel>(Lifestyle.Transient);
                CompositionContainerFactory.Instance.Register<ClanViewModel, ClanViewModel>(Lifestyle.Transient);
                CompositionContainerFactory.Instance.Register<PlayersCompareViewModel, PlayersCompareViewModel>(Lifestyle.Transient);
                CompositionContainerFactory.Instance.Register<PlayerSearchViewModel, PlayerSearchViewModel>(Lifestyle.Transient);
                CompositionContainerFactory.Instance.Register<PlayerServerStatisticViewModel, PlayerServerStatisticViewModel>(Lifestyle.Transient);
                CompositionContainerFactory.Instance.Register<TankStatisticViewModel, TankStatisticViewModel>(Lifestyle.Transient);
                CompositionContainerFactory.Instance.Register<AddReplayFolderViewModel, AddReplayFolderViewModel>(Lifestyle.Transient);
                CompositionContainerFactory.Instance.Register<UploadReplayViewModel, UploadReplayViewModel>(Lifestyle.Transient);
                CompositionContainerFactory.Instance.Register<ReplayViewModel, ReplayViewModel>(Lifestyle.Transient);

                CompositionContainerFactory.Instance.Register<IDataProvider, DataProvider>(Lifestyle.Singleton);
                CompositionContainerFactory.Instance.Register<DossierRepository, DossierRepository>(Lifestyle.Singleton);
                CompositionContainerFactory.Instance.Register<ReplaysManager, ReplaysManager>(Lifestyle.Singleton);
                CompositionContainerFactory.Instance.Register<ISessionStorage, NHibernateSessionStorage>(Lifestyle.Singleton);

                CompositionContainerFactory.Instance.Register<IAddReplayFolderView, AddReplayFolderWindow>(Lifestyle.Transient);
                CompositionContainerFactory.Instance.Register<IClanView, ClanWindow>(Lifestyle.Transient);
                CompositionContainerFactory.Instance.Register<IShellView, MainWindow>(Lifestyle.Singleton);
                CompositionContainerFactory.Instance.Register<IPlayerServerStatisticView, PlayerServerStatisticWindow>(Lifestyle.Transient);
                CompositionContainerFactory.Instance.Register<IReplayView, ReplayWindow>(Lifestyle.Transient);
                CompositionContainerFactory.Instance.Register<ISearchView, SearchWindow>(Lifestyle.Transient);
                CompositionContainerFactory.Instance.Register<ITankStatisticView, TankStatisticWindow>(Lifestyle.Transient);
                CompositionContainerFactory.Instance.Register<IFileDialogService, FileDialogService>(Lifestyle.Transient);
                CompositionContainerFactory.Instance.Register<IMessageService, MessageService>(Lifestyle.Transient);
                CompositionContainerFactory.Instance.Register<ISettingsView, SettingsWindow>(Lifestyle.Transient);
                CompositionContainerFactory.Instance.Register<IUploadReplayView, UploadReplayWindow>(Lifestyle.Transient);
                CompositionContainerFactory.Instance.Register<IPlayersCompareView, PlayersCompareWindow>(Lifestyle.Transient);

                Controller = CompositionContainerFactory.Instance.GetExport<ApplicationController>();

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
                CompositionContainerFactory.Instance.Dispose();
            }
            base.OnExit(e);
        }
    }
}
