using System;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Threading;
using Common.Logging;
using WotDossier.Applications;
using WotDossier.Applications.Update;
using WotDossier.Applications.View;
using WotDossier.Applications.ViewModel;
using WotDossier.Applications.ViewModel.Replay;
using WotDossier.Dal;
using WotDossier.Dal.NHibernate;
using WotDossier.Domain;
using WotDossier.Framework;
using WotDossier.Framework.Presentation.Services;
using WotDossier.Views;

namespace WotDossier.ReplaysManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static readonly ILog _log = LogManager.GetCurrentClassLogger();

        private ReplaysManagerController _controller;

        [Import]
        public ReplaysManagerController Controller
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

                //set app lang
                CultureHelper.SetUiCulture();

                // Registrations here
                CompositionContainerFactory.Instance.RegisterSingle<ReplaysManagerController, ReplaysManagerController>();
                CompositionContainerFactory.Instance.RegisterSingle<ReplayManagerShellViewModel, ReplayManagerShellViewModel>();
                CompositionContainerFactory.Instance.Register<AddReplayFolderViewModel, AddReplayFolderViewModel>();
                CompositionContainerFactory.Instance.Register<UploadReplayViewModel, UploadReplayViewModel>();
                CompositionContainerFactory.Instance.Register<ReplayViewModel, ReplayViewModel>();
                CompositionContainerFactory.Instance.Register<AboutViewModel, AboutViewModel>();
                CompositionContainerFactory.Instance.Register<SettingsViewModel, SettingsViewModel>();
                CompositionContainerFactory.Instance.Register<PlayerSearchViewModel, PlayerSearchViewModel>();
                CompositionContainerFactory.Instance.Register<ReplayViewerSettingsViewModel, ReplayViewerSettingsViewModel>();
                CompositionContainerFactory.Instance.Register<PlayerServerStatisticViewModel, PlayerServerStatisticViewModel>();

                CompositionContainerFactory.Instance.RegisterSingle<IDataProvider, DataProvider>();
                CompositionContainerFactory.Instance.RegisterSingle<DossierRepository, DossierRepository>();
                CompositionContainerFactory.Instance.Register<Applications.Logic.ReplaysManager, Applications.Logic.ReplaysManager>();
                CompositionContainerFactory.Instance.Register<ISessionStorage, DesktopAppSessionStorage>();

                CompositionContainerFactory.Instance.Register<IAddReplayFolderView, AddReplayFolderWindow>();
                CompositionContainerFactory.Instance.RegisterSingle<IShellView, MainWindow>();
                CompositionContainerFactory.Instance.Register<IReplayView, ReplayWindow>();
                //CompositionContainerFactory.Instance.Register<IFileDialogService, FileDialogService>();
                //CompositionContainerFactory.Instance.Register<IMessageService, MessageService>();
                CompositionContainerFactory.Instance.Register<IUploadReplayView, UploadReplayWindow>();
                CompositionContainerFactory.Instance.Register<IAboutView, AboutWindow>();
                CompositionContainerFactory.Instance.Register<ISettingsView, SettingsWindow>();
                CompositionContainerFactory.Instance.Register<IReplayViewerSettingsView, ReplayViewerSettingsWindow>();

                Controller = CompositionContainerFactory.Instance.GetExport<ReplaysManagerController>();

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
