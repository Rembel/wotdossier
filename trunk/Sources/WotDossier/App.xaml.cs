using System;
using System.ComponentModel.Composition;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using Common.Logging;
using WotDossier.Applications;
using WotDossier.Applications.ViewModel;
using WotDossier.Dal;
using WotDossier.Domain.Replay;
using WotDossier.Framework;

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
            SettingsReader reader = new SettingsReader(WotDossierSettings.SettingsPath);
            //set app lang
            var culture = new CultureInfo(reader.Get().Language);
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            CompositionContainerFactory.Instance.Container.SatisfyImportsOnce(this);

#if (DEBUG != true)
            // Don't handle the exceptions in Debug mode because otherwise the Debugger wouldn't
            // jump into the code when an exception occurs.
            DispatcherUnhandledException += AppDispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += AppDomainUnhandledException;
#endif

            // start application
            try
            {
                //Controller.Run();

                ReplayFile replayFile = new ReplayFile(new FileInfo(@"I:\20130421_2021_ussr-IS-3_18_cliff.wotreplay"));

                if (replayFile != null)
                {
                    ReplayViewModel viewModel = CompositionContainerFactory.Instance.Container.GetExport<ReplayViewModel>().Value;

                    //convert dossier cache file to json
                    CacheHelper.ReplayToJson(replayFile.FileInfo);
                    Thread.Sleep(1000);
                    Replay replay = WotApiClient.Instance.ReadReplay(replayFile.FileInfo.FullName.Replace(replayFile.FileInfo.Extension, ".json"));
                    viewModel.Init(replay);
                    viewModel.Show();
                }
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
            if (Controller != null)
            {
                CompositionContainerFactory.Instance.Container.Dispose();
                Controller.Dispose();
            }
            base.OnExit(e);
        }
    }
}
