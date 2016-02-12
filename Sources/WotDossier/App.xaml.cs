using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using Common.Logging;
using WotDossier.Applications;
using WotDossier.Applications.Logic;
using WotDossier.Applications.View;
using WotDossier.Applications.ViewModel;
using WotDossier.Applications.ViewModel.Replay;
using WotDossier.Common;
using WotDossier.Dal;
using WotDossier.Dal.NHibernate;
using WotDossier.Framework;
using WotDossier.Framework.Applications.Services;
using WotDossier.Framework.Presentation.Services;
using WotDossier.Update.Update;
using WotDossier.Views;

namespace WotDossier
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static readonly ILog _log = LogManager.GetLogger<App>();

        private Mutex _mutex;
        private const string INSTANCE_ID = "1D4E6094-31A4-402F-AB5B-9E4000A4ED48";

        private ApplicationController _controller;
        
        [Import]
        public ApplicationController Controller
        {
            get { return _controller; }
            set { _controller = value; }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Application.Startup" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.StartupEventArgs" /> that contains the event data.</param>
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

            //set app lang
            CultureHelper.SetUiCulture(SettingsReader.Get().Language);

            //TODO: Remove, hard hack
            //RestoreAutomapperDll();

            bool isNewInstance;
            _mutex = new Mutex(true, INSTANCE_ID, out isNewInstance);
            if (!isNewInstance)
            {
                //WPFMessageBox.Show(Gui.Properties.Resources.Msg_ApplicationIs_Already_Started, Gui.Properties.Resources.MessageBox_Title_Error, WPFMessageBoxImage.Error);
                Shutdown();
                //activate already opened app window
                NativeMethods.ActivateWindow(ApplicationInfo.FullProductName);
                return;
            }

            //Apply application UI theme
            ThemesManager.ApplyTheme(SettingsReader.Get().Theme);

#if !DEBUG
            // Don't handle the exceptions in Debug mode because otherwise the Debugger wouldn't
            // jump into the code when an exception occurs.
            DispatcherUnhandledException += AppDispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += AppDomainUnhandledException;
#endif

            // start application
            try
            {
                _log.Trace("OnStartup start");

                DatabaseManager manager = new DatabaseManager();
                manager.InitDatabase();

                CompositionContainerFactory.Instance.RegisterSingle<ISessionStorage, DesktopAppSessionStorage>();
                CompositionContainerFactory.Instance.RegisterSingle<DataProvider, DataProvider>();
                CompositionContainerFactory.Instance.RegisterSingle<DossierRepository, DossierRepository>();

                CompositionContainerFactory.Instance.RegisterSingle<IShellView, MainWindow>();

                CompositionContainerFactory.Instance.RegisterSingle<ShellViewModel, ShellViewModel>();
                CompositionContainerFactory.Instance.RegisterSingle<ApplicationController, ApplicationController>();

                // Registrations here
                CompositionContainerFactory.Instance.Register<SettingsViewModel, SettingsViewModel>();
                CompositionContainerFactory.Instance.Register<ClanSearchViewModel, ClanSearchViewModel>();
                CompositionContainerFactory.Instance.Register<ClanViewModel, ClanViewModel>();
                CompositionContainerFactory.Instance.Register<AboutViewModel, AboutViewModel>();
                CompositionContainerFactory.Instance.Register<PlayersCompareViewModel, PlayersCompareViewModel>();
                CompositionContainerFactory.Instance.Register<PlayerSearchViewModel, PlayerSearchViewModel>();
                CompositionContainerFactory.Instance.Register<PlayerServerStatisticViewModel, PlayerServerStatisticViewModel>();
                CompositionContainerFactory.Instance.Register<TankStatisticViewModel, TankStatisticViewModel>();
                CompositionContainerFactory.Instance.Register<AddReplayFolderViewModel, AddReplayFolderViewModel>();
                CompositionContainerFactory.Instance.Register<UploadReplayViewModel, UploadReplayViewModel>();
                CompositionContainerFactory.Instance.Register<ReplayViewModel, ReplayViewModel>();
                CompositionContainerFactory.Instance.Register<ReplayViewerSettingsViewModel, ReplayViewerSettingsViewModel>();

                CompositionContainerFactory.Instance.Register<ReplaysManager, ReplaysManager>();

                CompositionContainerFactory.Instance.Register<IAddReplayFolderView, AddReplayFolderWindow>();
                CompositionContainerFactory.Instance.Register<IClanView, ClanWindow>();
                CompositionContainerFactory.Instance.Register<IAboutView, AboutWindow>();
                CompositionContainerFactory.Instance.Register<IPlayerServerStatisticView, PlayerServerStatisticWindow>();
                CompositionContainerFactory.Instance.Register<IReplayView, ReplayWindow>();
                CompositionContainerFactory.Instance.Register<ISearchView, SearchWindow>();
                CompositionContainerFactory.Instance.Register<ITankStatisticView, TankStatisticWindow>();
                CompositionContainerFactory.Instance.Register<IFileDialogService, FileDialogService>();
                CompositionContainerFactory.Instance.Register<IMessageService, MessageService>();
                CompositionContainerFactory.Instance.Register<ISettingsView, SettingsWindow>();
                CompositionContainerFactory.Instance.Register<IUploadReplayView, UploadReplayWindow>();
                CompositionContainerFactory.Instance.Register<IPlayersCompareView, PlayersCompareWindow>();
                CompositionContainerFactory.Instance.Register<IReplayViewerSettingsView, ReplayViewerSettingsWindow>();

                //SyncManager syncManager = new SyncManager(CompositionContainerFactory.Instance.GetExport<DossierRepository>());
                //syncManager.Sync();

                Controller = CompositionContainerFactory.Instance.GetExport<ApplicationController>();

                Controller.Run();

                _log.Trace("OnStartup end");
            }
            catch (Exception exception)
            {
                HandleException(exception, false);
                Shutdown();
            }

            base.OnStartup(e);
        }

        private void RestoreAutomapperDll()
        {
            const string library = @"AutoMapper.dll";
            string currentDirectory = Folder.AssemblyDirectory();
            string path = Path.Combine(currentDirectory, library);
            if (!File.Exists(path))
            {
                Assembly entryAssembly = Assembly.GetEntryAssembly();
                var resourceName = entryAssembly.GetName().Name + @"." + library;
                byte[] embeddedResource = entryAssembly.GetEmbeddedResource(resourceName);
                using (FileStream fileStream = File.OpenWrite(path))
                {
                    fileStream.Write(embeddedResource, 0, embeddedResource.Length);
                    fileStream.Flush();
                }
            }
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
                MessageBox.Show(e.ToString(), ApplicationInfo.ProductName, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Application.Exit" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.Windows.ExitEventArgs" /> that contains the event data.</param>
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
