using System.Threading;
using System.Windows.Threading;
using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using WotDossier.Applications.Update;
using WotDossier.Applications.View;
using WotDossier.Applications.ViewModel.Rows;
using WotDossier.Dal;
using WotDossier.Domain;
using WotDossier.Framework;
using WotDossier.Framework.Applications;
using WotDossier.Framework.EventAggregator;
using WotDossier.Framework.Forms.Commands;
using WotDossier.Framework.Forms.ProgressDialog;

namespace WotDossier.Applications.ViewModel
{
    /// <summary>
    /// The ViewModel for the application's main window.
    /// </summary>
    [Export(typeof(ReplayManagerShellViewModel))]
    public class ReplayManagerShellViewModel : ViewModel<IShellView>
    {
        #region [ Properties and Fields ]

        public ProgressControlViewModel ProgressView { get; set; }

        public PlayerChartsViewModel ChartView { get; set; }

        public ReplaysViewModel ReplaysViewModel { get; set; }

        private bool _loadInProgress;
        public bool LoadInProgress
        {
            get { return _loadInProgress; }
            set
            {
                _loadInProgress = value;
                RaisePropertyChanged("LoadCommand");
            }
        }

        #endregion

        #region Commands

        public DelegateCommand LoadCommand { get; set; }
        public DelegateCommand SettingsCommand { get; set; }
        public DelegateCommand AboutCommand { get; set; }

        public DelegateCommand<object> RowDoubleClickCommand { get; set; }
        public DelegateCommand<object> AddToFavoriteCommand { get; set; }
        public DelegateCommand<object> RemoveFromFavoriteCommand { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ReplayManagerShellViewModel"/> class.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="dossierRepository"></param>
        [ImportingConstructor]
        public ReplayManagerShellViewModel([Import(typeof(IShellView))]IShellView view, [Import]DossierRepository dossierRepository)
            : base(view, false)
        {
            LoadCommand = new DelegateCommand(OnLoad, CanLoad);
            SettingsCommand = new DelegateCommand(OnSettings);
            AboutCommand = new DelegateCommand(OnAbout);
            
            RowDoubleClickCommand = new DelegateCommand<object>(OnRowDoubleClick);

            WeakEventHandler.SetAnyGenericHandler<ReplayManagerShellViewModel, CancelEventArgs>(
                h => view.Closing += new CancelEventHandler(h), h => view.Closing -= new CancelEventHandler(h), this, (s, e) => s.ViewClosing(e));

            ProgressView = new ProgressControlViewModel();

            ChartView = new PlayerChartsViewModel();

            ReplaysViewModel = new ReplaysViewModel(dossierRepository, ProgressView, ChartView);
        }

        private bool CanLoad()
        {
            return !LoadInProgress;
        }

        #endregion

        #region Handlers

        private void OnRowDoubleClick(object rowData)
        {
            ITankStatisticRow tankStatisticRowViewModel = rowData as ITankStatisticRow;

            //NRE if row of type TotalTankStatisticRowViewModel
            if (tankStatisticRowViewModel != null && !(tankStatisticRowViewModel is TotalTankStatisticRowViewModel))
            {
                TankStatisticViewModel viewModel = CompositionContainerFactory.Instance.GetExport<TankStatisticViewModel>();
                if (viewModel != null)
                {
                    viewModel.TankStatistic = tankStatisticRowViewModel;
                    AppSettings appSettings = SettingsReader.Get();

                    ITankStatisticRow temp = tankStatisticRowViewModel.GetPreviousStatistic();

                    // configure LastNBattles stat for tank
                    if (appSettings.PeriodSettings.Period == StatisticPeriod.LastNBattles)
                    {
                        int battles = tankStatisticRowViewModel.BattlesCount - appSettings.PeriodSettings.LastNBattles;
                        ITankStatisticRow model = tankStatisticRowViewModel.GetAll().OrderBy(x => x.BattlesCount).FirstOrDefault(x => x.BattlesCount >= battles);
                        tankStatisticRowViewModel.SetPreviousStatistic(model);
                    }

                    viewModel.Show();

                    //restore settings 
                    if (appSettings.PeriodSettings.Period == StatisticPeriod.LastNBattles)
                    {
                        tankStatisticRowViewModel.SetPreviousStatistic(temp);
                    }
                }
            }
        }

        private void OnSettings()
        {
            var viewModel = CompositionContainerFactory.Instance.GetExport<SettingsViewModel>();
            if (viewModel != null)
            {
                viewModel.Show();
            }
        }

        private void OnAbout()
        {
            AboutViewModel viewModel = CompositionContainerFactory.Instance.GetExport<AboutViewModel>();
            if (viewModel != null)
            {
                viewModel.Show();
            }
        }

        #endregion

        #region load

        private void OnLoad()
        {
            //AppSettings settings = SettingsReader.Get();

            //if (settings == null || string.IsNullOrEmpty(settings.PlayerName) || string.IsNullOrEmpty(settings.Server))
            //{
            //    MessageBox.Show(Resources.Resources.WarningMsg_SpecifyPlayerName, Resources.Resources.WindowCaption_Warning,
            //        MessageBoxButton.OK, MessageBoxImage.Warning);
            //    return;
            //}

            ReplaysViewModel.LoadReplaysList();
        }

        #endregion

        private void ViewClosing(CancelEventArgs e)
        {
            if (!e.Cancel)
            {
                e.Cancel = !IsCloseAllowed();
            }
        }

        /// <summary>
        /// Shows this instance.
        /// </summary>
        public virtual void Show()
        {
            ViewTyped.Loaded += OnShellViewActivated;
            ViewTyped.Show();

            Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Send, (SendOrPostCallback)delegate
            {
                UpdateChecker.CheckForUpdates();
            }, null);
        }

        private void OnShellViewActivated(object sender, EventArgs eventArgs)
        {
            ViewTyped.Loaded -= OnShellViewActivated;
            OnLoad();
        }

        /// <summary>
        /// Closes this instance.
        /// </summary>
        /// <returns></returns>
        public bool Close()
        {
            bool close = false;
            if (IsCloseAllowed())
            {
                ViewTyped.Close();
                close = true;
            }
            return close;
        }

        private bool IsCloseAllowed()
        {
            return true;
        }
    }
}