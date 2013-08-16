using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using Microsoft.Research.DynamicDataDisplay;
using Microsoft.Research.DynamicDataDisplay.DataSources;
using Microsoft.Research.DynamicDataDisplay.PointMarkers;
using WotDossier.Applications.Update;
using WotDossier.Applications.View;
using WotDossier.Applications.ViewModel.Rows;
using WotDossier.Dal;
using WotDossier.Domain;
using WotDossier.Domain.Entities;
using WotDossier.Domain.Player;
using WotDossier.Domain.Replay;
using WotDossier.Domain.Tank;
using WotDossier.Framework;
using WotDossier.Framework.Applications;
using WotDossier.Framework.EventAggregator;
using WotDossier.Framework.Forms.Commands;
using Common.Logging;
using WotDossier.Framework.Forms.ProgressDialog;

namespace WotDossier.Applications.ViewModel
{
    /// <summary>
    /// The ViewModel for the application's main window.
    /// </summary>
    [Export(typeof(ShellViewModel))]
    public class ShellViewModel : ViewModel<IShellView>
    {
        private static readonly ILog _log = LogManager.GetLogger("ShellViewModel");

        private readonly DossierRepository _dossierRepository;
        private PlayerStatisticViewModel _playerStatistic;
        private PlayerStatisticViewModel _sessionStatistic;
        private IEnumerable<TankRowMasterTanker> _masterTanker;
        private IEnumerable<TankStatisticRowViewModel> _tanks = new List<TankStatisticRowViewModel>();

        #region [ Properties ]

        public ChartPlotter ChartRating
        {
            get { return ViewTyped.ChartRating; }
        }

        public ChartPlotter ChartWinPercent
        {
            get { return ViewTyped.ChartWinPercent; }
        }

        public ChartPlotter ChartAvgDamage
        {
            get { return ViewTyped.ChartAvgDamage; }
        }

        public DelegateCommand LoadCommand { get; set; }
        public DelegateCommand SettingsCommand { get; set; }

        public DelegateCommand<object> OnRowDoubleClickCommand { get; set; }
        public DelegateCommand<object> OnReplayRowDoubleClickCommand { get; set; }
        public DelegateCommand<object> OnReplayRowUploadCommand { get; set; }
        public DelegateCommand<object> OnReplayRowDeleteCommand { get; set; }

        public PlayerStatisticViewModel PlayerStatistic
        {
            get { return _playerStatistic; }
            set
            {
                _playerStatistic = value;
                RaisePropertyChanged("PlayerStatistic");
            }
        }

        public PlayerStatisticViewModel SessionStatistic
        {
            get { return _sessionStatistic; }
            set
            {
                _sessionStatistic = value;
                RaisePropertyChanged("SessionStatistic");
            }
        }

        public IEnumerable<TankRowMasterTanker> MasterTanker
        {
            get { return _masterTanker; }
            set
            {
                _masterTanker = value;
                RaisePropertyChanged("MasterTanker");
            }
        }

        public IEnumerable<TankStatisticRowViewModel> Tanks
        {
            get { return TankFilter.Filter(_tanks); }
            set
            {
                _tanks = value.ToList();
                RaisePropertyChanged("Tanks");
            }
        }

        public ObservableCollection<ReplayFile> Replays
        {
            get { return _replays; }
            set
            {
                _replays = value;
                RaisePropertyChanged("Replays");
            }
        }

        private FraggsCountViewModel _fraggsCount = new FraggsCountViewModel();

        public FraggsCountViewModel FraggsCount
        {
            get { return _fraggsCount; }
            set { _fraggsCount = value; }
        }

        private ObservableCollection<SellInfo> _lastUsedTanks = new ObservableCollection<SellInfo>();
        private ObservableCollection<ReplayFile> _replays;
        private TankFilterViewModel _tankFilter;
        private PlayerStatisticViewModel _sessionStartStatistic;
        private ProgressControlViewModel _progressView;

        public ObservableCollection<SellInfo> LastUsedTanks
        {
            get { return _lastUsedTanks; }
            set { _lastUsedTanks = value; }
        }

        public ProgressControlViewModel ProgressView
        {
            get { return _progressView; }
            set { _progressView = value; }
        }

        public sealed class SellInfo : INotifyPropertyChanged
        {
            private double _winPercent;
            public double WinPercent
            {
                get { return _winPercent; }
                set { _winPercent = value; PropertyChanged.Raise(this, "WinPercent"); }
            }

            private string _tankName;
            public string TankName
            {
                get { return _tankName; }
                set { _tankName = value; PropertyChanged.Raise(this, "TankName"); }
            }

            private int _battles;
            public int Battles
            {
                get { return _battles; }
                set { _battles = value; PropertyChanged.Raise(this, "Battles"); }
            }

            #region INotifyPropertyChanged Members

            public event PropertyChangedEventHandler PropertyChanged;

            #endregion
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ShellViewModel"/> class.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="dossierRepository"></param>
        [ImportingConstructor]
        public ShellViewModel([Import(typeof(IShellView))]IShellView view, [Import]DossierRepository dossierRepository)
            : this(view, false)
        {
            _dossierRepository = dossierRepository;
            LoadCommand = new DelegateCommand(OnLoad);
            SettingsCommand = new DelegateCommand(OnSettings);
            OnRowDoubleClickCommand = new DelegateCommand<object>(OnRowDoubleClick);
            OnReplayRowDoubleClickCommand = new DelegateCommand<object>(OnReplayRowDoubleClick);
            OnReplayRowUploadCommand = new DelegateCommand<object>(OnReplayRowUpload);
            OnReplayRowDeleteCommand = new DelegateCommand<object>(OnReplayRowDelete);

            WeakEventHandler.SetAnyGenericHandler<ShellViewModel, CancelEventArgs>(
                h => view.Closing += new CancelEventHandler(h), h => view.Closing -= new CancelEventHandler(h), this, (s, e) => s.ViewClosing(s, e));

            TankFilter = new TankFilterViewModel();
            TankFilter.PropertyChanged += TankFilterOnPropertyChanged;

            EventAggregatorFactory.EventAggregator.GetEvent<StatisticPeriodChangedEvent>().Subscribe(OnStatisticPeriodChanged);

            ProgressView = new ProgressControlViewModel();
        }

        private void OnStatisticPeriodChanged(StatisticPeriodChangedEvent obj)
        {
            InitLastUsedTanksChart();
        }

        private void OnSettings()
        {
            SettingsViewModel viewModel = CompositionContainerFactory.Instance.Container.GetExport<SettingsViewModel>().Value;
            List<DateTime> list = null;
            if (PlayerStatistic != null)
            {
                list = PlayerStatistic.GetAll().Select(x => x.Updated).OrderByDescending(x => x).Skip(1).ToList();
            }
            viewModel.PrevDates = list ?? new List<DateTime>();
            viewModel.Show();
        }

        private void TankFilterOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            RaisePropertyChanged("Tanks");
        }

        public TankFilterViewModel TankFilter
        {
            get { return _tankFilter; }
            set { _tankFilter = value; }
        }

        private void OnReplayRowDelete(object rowData)
        {
            ReplayFile replayFile = rowData as ReplayFile;

            if (replayFile != null)
            {
                if (replayFile.FileInfo.Exists)
                {
                    replayFile.FileInfo.Delete();
                    Replays.Remove(replayFile);
                }
            }
        }

        private void OnReplayRowUpload(object rowData)
        {
            ReplayFile replayFile = rowData as ReplayFile;

            if (replayFile != null)
            {
                UploadReplayViewModel viewModel = CompositionContainerFactory.Instance.Container.GetExport<UploadReplayViewModel>().Value;
                viewModel.ReplayFile = replayFile;
                viewModel.Show();
            }
        }

        private void OnRowDoubleClick(object rowData)
        {
            TankStatisticRowViewModel tankStatisticRowViewModel = rowData as TankStatisticRowViewModel;

            if (tankStatisticRowViewModel != null)
            {
                TankStatisticViewModel viewModel = CompositionContainerFactory.Instance.Container.GetExport<TankStatisticViewModel>().Value;
                viewModel.TankStatistic = tankStatisticRowViewModel;
                viewModel.Show();
            }
        }

        private void OnReplayRowDoubleClick(object rowData)
        {
            ReplayFile replayFile = rowData as ReplayFile;

            if (replayFile != null)
            {
                string jsonFile = replayFile.FileInfo.FullName.Replace(replayFile.FileInfo.Extension, ".json");
                //convert dossier cache file to json
                if (!File.Exists(jsonFile))
                {
                    CacheHelper.ReplayToJson(replayFile.FileInfo);
                    Thread.Sleep(1000);
                }

                if (!File.Exists(jsonFile))
                {
                    MessageBox.Show(Resources.Resources.Msg_Error_on_replay_file_read, Resources.Resources.WindowCaption_Error, MessageBoxButton.OK, MessageBoxImage.Error);
                }

                Replay replay = WotApiClient.Instance.ReadReplay(jsonFile);
                if (ValidateReplayData(replay))
                {
                    ReplayViewModel viewModel = CompositionContainerFactory.Instance.Container.GetExport<ReplayViewModel>().Value;
                    viewModel.Init(replay, replayFile);
                    viewModel.Show();
                }
                else
                {
                    MessageBox.Show(Resources.Resources.Msg_File_incomplete_or_not_supported, Resources.Resources.WindowCaption_Error, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private bool ValidateReplayData(Replay replayFile)
        {
            if (replayFile.datablock_battle_result != null)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShellViewModel"/> class.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="isChild">if set to <c>true</c> [is child].</param>
        public ShellViewModel(IShellView view, bool isChild)
            : base(view, isChild)
        {
        }

        #endregion

        private void OnLoad()
        {
            AppSettings settings = SettingsReader.Get();

            PlayerStat playerStat = LoadPlayerStatistic(settings);

            if (playerStat != null && playerStat.data.name.Equals(settings.PlayerId, StringComparison.InvariantCultureIgnoreCase))
            {
                FileInfo cacheFile = CacheHelper.GetCacheFile(playerStat.data.name);

                List<TankJson> tanks;

                if (cacheFile != null)
                {
                    //convert dossier cache file to json
                    CacheHelper.BinaryCacheToJson(cacheFile);
                    Thread.Sleep(1000);

                    tanks = LoadTanks(cacheFile);

                    PlayerStatistic = InitPlayerStatisticViewModel(playerStat, tanks);

                    PlayerStatisticViewModel clone = PlayerStatistic.Clone();

                    if (_sessionStartStatistic == null)
                    {
                        //save start session statistic
                        _sessionStartStatistic = clone;
                    }
                    else
                    {
                        clone.SetPreviousStatistic(_sessionStartStatistic);
                    }

                    SessionStatistic = clone;

                    InitTanksStatistic(playerStat, tanks);

                    InitChart();
                    InitLastUsedTanksChart();
                }
                else
                {
                    MessageBox.Show(Resources.Resources.WarningMsg_CanntFindPlayerDataInDossierCache, Resources.Resources.WindowCaption_Warning,
                                    MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }

            LoadReplaysList();
        }

        private void LoadReplaysList()
        {
            string replaysFolder = Folder.GetReplaysFolder();

            if (string.IsNullOrEmpty(replaysFolder))
            {
                return;
            }

            if (Directory.Exists(replaysFolder))
            {
                ObservableCollection<ReplayFile> replayFilesTemp = new ObservableCollection<ReplayFile>();

                ProgressDialogResult result = ProgressView.Execute((Window)ViewTyped, Resources.Resources.ProgressTitle_Loading_replays, (bw, we) =>
                {
                    string[] files = Directory.GetFiles(replaysFolder, "*.wotreplay");
                    List<FileInfo> replays = files.Select(x => new FileInfo(Path.Combine(replaysFolder, x))).Where(x => x.Length > 0).ToList();

                    int count = replays.Count();

                    List<ReplayFile> replayFiles = new List<ReplayFile>(count);

                    int index = 0;
                    foreach (FileInfo replay in replays)
                    {
                        ReplayFile replayFile = new ReplayFile(replay, WotApiClient.Instance.ReadReplay2Blocks(replay));
                        replayFiles.Add(replayFile);
                        index++;
                        int percent = (index + 1) * 100 / count;
                        if (ProgressView.ReportWithCancellationCheck(bw, we, percent, Resources.Resources.ProgressLabel_Processing_file_format, index + 1, count, replay.Name))
                        {
                            return;
                        }
                    }

                    // So this check in order to avoid default processing after the Cancel button has been pressed.
                    // This call will set the Cancelled flag on the result structure.
                    ProgressView.CheckForPendingCancellation(bw, we);

                    replayFiles.OrderByDescending(x => x.PlayTime).ToList().ForEach(replayFilesTemp.Add);
                    Replays = replayFilesTemp;
                }, new ProgressDialogSettings(true, true, false));
            }
            else
            {
                MessageBox.Show(string.Format(Resources.Resources.Msg_CantFindReplaysDirectory, replaysFolder), Resources.Resources.WindowCaption_Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void InitLastUsedTanksChart()
        {
            LastUsedTanks.Clear();
            IEnumerable<TankStatisticRowViewModel> viewModels = _tanks.Where(x => x.Updated > PlayerStatistic.PreviousDate);
            IEnumerable<SellInfo> items = viewModels.Select(x => new SellInfo { TankName = x.Tank, WinPercent = x.WinsPercentForPeriod, Battles = x.BattlesCountDelta });
            LastUsedTanks.AddMany(items);
            RaisePropertyChanged("LastUsedTanksList");
        }

        public List<TankStatisticRowViewModel> LastUsedTanksList
        {
            get
            {
                List<TankStatisticRowViewModel> list = _tanks.Where(x => x.LastBattle > PlayerStatistic.PreviousDate).ToList();
                return list;
            }
        }

        private PlayerStatisticViewModel InitPlayerStatisticViewModel(PlayerStat playerStat, List<TankJson> tanks)
        {
            PlayerEntity player = _dossierRepository.UpdatePlayerStatistic(playerStat, tanks);

            var statisticEntities = _dossierRepository.GetPlayerStatistic(player.PlayerId).ToList();

            PlayerStatisticEntity currentStatistic = statisticEntities.OrderByDescending(x => x.BattlesCount).First();
            List<PlayerStatisticViewModel> oldStatisticEntities =
                statisticEntities.Where(x => x.Id != currentStatistic.Id)
                                 .Select(x => new PlayerStatisticViewModel(x)).ToList();

            PlayerStatisticViewModel currentStatisticViewModel = new PlayerStatisticViewModel(currentStatistic,
                                                                                              oldStatisticEntities);
            currentStatisticViewModel.Name = player.Name;
            currentStatisticViewModel.Created = player.Creaded;
            currentStatisticViewModel.BattlesPerDay = currentStatisticViewModel.BattlesCount/
                                                      (DateTime.Now - player.Creaded).Days;

            if (playerStat.data.clan.clan != null)
            {
                currentStatisticViewModel.Clan = new PlayerStatisticClanViewModel(playerStat.data.clan);
            }

            return currentStatisticViewModel;
        }

        private static PlayerStat LoadPlayerStatistic(AppSettings settings)
        {
            PlayerStat playerStat = null;
            if (settings == null || string.IsNullOrEmpty(settings.PlayerId) || string.IsNullOrEmpty(settings.Server))
            {
                MessageBox.Show(Resources.Resources.WarningMsg_SpecifyPlayerName, Resources.Resources.WindowCaption_Warning,
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
                return null;
            }

            try
            {
                playerStat = WotApiClient.Instance.LoadPlayerStat(settings);
            }
            catch (Exception e)
            {
                _log.Error(e);
                MessageBox.Show(Resources.Resources.ErrorMsg_GetPlayerData, Resources.Resources.WindowCaption_Error,
                                MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return playerStat;
        }

        private void InitTanksStatistic(PlayerStat playerStat, List<TankJson> tanks)
        {
            PlayerEntity playerEntity = _dossierRepository.UpdateTankStatistic(playerStat, tanks);

            if (playerEntity == null)
            {
                MessageBox.Show(Resources.Resources.ErrorMsg_GetPlayerInfo, Resources.Resources.WindowCaption_Error, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            IEnumerable<TankStatisticEntity> entities = _dossierRepository.GetTanksStatistic(playerEntity);

            Tanks = entities.GroupBy(x => x.TankId).Select(ToStatisticViewModel).OrderByDescending(x => x.Tier).ThenBy(x => x.Tank).ToList();

            InitMasterTankerList(tanks);

            FraggsCount.Init(_tanks.ToList());
        }

        private static List<TankJson> LoadTanks(FileInfo cacheFile)
        {
            List<TankJson> tanks = WotApiClient.Instance.ReadTanks(cacheFile.FullName.Replace(".dat", ".json"));
            return tanks;
        }

        private void InitMasterTankerList(List<TankJson> tanks)
        {
            IEnumerable<int> killed =
                tanks.SelectMany(x => x.Frags).Select(x => x.TankUniqueId).Distinct();
            IEnumerable<TankRowMasterTanker> masterTanker = WotApiClient.Instance.TanksDictionary.Where(
                x => !killed.Contains(x.Key) && IsExistedtank(x.Value))
                                                                        .Select(
                                                                            x =>
                                                                            new TankRowMasterTanker(x.Value,
                                                                                                    WotApiClient.Instance
                                                                                                                .GetTankIcon(
                                                                                                                    x.Value)))
                                                                        .OrderBy(x => x.IsPremium)
                                                                        .ThenBy(x => x.Tier);
            MasterTanker = masterTanker;
        }

        private TankStatisticRowViewModel ToStatisticViewModel(IGrouping<int, TankStatisticEntity> tankStatisticEntities)
        {
            IEnumerable<TankJson> statisticViewModels = tankStatisticEntities.Select(x => UnZipObject(x.Raw)).ToList();
            TankJson currentStatistic = statisticViewModels.OrderByDescending(x => x.Tankdata.battlesCount).First();
            IEnumerable<TankJson> prevStatisticViewModels =
                statisticViewModels.Where(x => x.Tankdata.battlesCount != currentStatistic.Tankdata.battlesCount);
            return new TankStatisticRowViewModel(currentStatistic, prevStatisticViewModels);
        }

        private static TankJson UnZipObject(byte[] x)
        {
            TankJson tankJson = WotApiHelper.UnZipObject<TankJson>(x);
            WotApiClient.Instance.ExtendPropertiesData(tankJson);
            return tankJson;
        }

        private bool IsExistedtank(TankInfo tankInfo)
        {
            return tankInfo.tankid <= 250 && !tankInfo.icon.Contains("training") && tankInfo.title != "KV" && tankInfo.title != "T23";
        }

        #region [ Charts initialization ]

        private void InitChart()
        {
            if (PlayerStatistic != null)
            {
                List<PlayerStatisticViewModel> statisticViewModels = PlayerStatistic.GetAll();
                InitRatingChart(statisticViewModels);
                InitWinPercentChart(statisticViewModels);
                InitAvgDamageChart(statisticViewModels);
            }
        }

        private void InitRatingChart(List<PlayerStatisticViewModel> statisticViewModels)
        {
            DataRect dataRect = DataRect.Create(0, 0, 100000, 2500);
            ChartRating.Viewport.Domain = dataRect;
            ChartRating.Viewport.Visible = dataRect;

            ChartRating.RemoveUserElements();

            IEnumerable<DataPoint> erPoints = statisticViewModels.Select(x => new DataPoint(x.BattlesCount, x.EffRating));
            var dataSource = new EnumerableDataSource<DataPoint>(erPoints) { XMapping = x => x.X, YMapping = y => y.Y };
            dataSource.AddMapping(ShapeElementPointMarker.ToolTipTextProperty,
                                  point =>
                                  String.Format(Resources.Resources.ChartTooltipFormat_Rating, point.X, point.Y));
            SolidColorBrush brush = new SolidColorBrush { Color = Colors.Blue };
            Pen lineThickness = new Pen(brush, 2);
            ElementPointMarker circlePointMarker = new CircleElementPointMarker { Size = 4, Fill = brush, Brush = brush };
            ChartRating.AddLineGraph(dataSource, lineThickness, circlePointMarker,
                                     new PenDescription(Resources.Resources.ChartLegend_ER));

            IEnumerable<DataPoint> wn6Points =
                statisticViewModels.Select(x => new DataPoint(x.BattlesCount, x.WN6Rating));
            dataSource = new EnumerableDataSource<DataPoint>(wn6Points) { XMapping = x => x.X, YMapping = y => y.Y };
            dataSource.AddMapping(ShapeElementPointMarker.ToolTipTextProperty,
                                  point =>
                                  String.Format(Resources.Resources.ChartTooltipFormat_Rating, point.X, point.Y));
            brush = new SolidColorBrush { Color = Colors.Green };
            lineThickness = new Pen(brush, 2);
            circlePointMarker = new CircleElementPointMarker { Size = 4, Fill = brush, Brush = brush };
            ChartRating.AddLineGraph(dataSource, lineThickness, circlePointMarker,
                                     new PenDescription(Resources.Resources.ChartLegend_WN6Rating));

            ChartRating.FitToView();
        }

        private void InitWinPercentChart(List<PlayerStatisticViewModel> statisticViewModels)
        {
            ChartWinPercent.RemoveUserElements();

            IEnumerable<DataPoint> erPoints =
                statisticViewModels.Select(x => new DataPoint(x.BattlesCount, x.WinsPercent));
            var dataSource = new EnumerableDataSource<DataPoint>(erPoints) { XMapping = x => x.X, YMapping = y => y.Y };
            dataSource.AddMapping(ShapeElementPointMarker.ToolTipTextProperty,
                                  point =>
                                  String.Format(Resources.Resources.ChartTooltipFormat_WinPercent, point.X, point.Y));
            SolidColorBrush brush = new SolidColorBrush { Color = Colors.Blue };
            Pen lineThickness = new Pen(brush, 2);
            ElementPointMarker circlePointMarker = new CircleElementPointMarker { Size = 4, Fill = brush, Brush = brush };
            ChartWinPercent.AddLineGraph(dataSource, lineThickness, circlePointMarker,
                                         new PenDescription(Resources.Resources.ChartLegend_WinPercent));

            ChartWinPercent.FitToView();
        }

        private void InitAvgDamageChart(List<PlayerStatisticViewModel> statisticViewModels)
        {
            ChartAvgDamage.RemoveUserElements();

            IEnumerable<DataPoint> erPoints =
                statisticViewModels.Select(x => new DataPoint(x.BattlesCount, x.DamageDealt / (double)x.BattlesCount));
            var dataSource = new EnumerableDataSource<DataPoint>(erPoints) { XMapping = x => x.X, YMapping = y => y.Y };
            dataSource.AddMapping(ShapeElementPointMarker.ToolTipTextProperty,
                                  point =>
                                  String.Format(Resources.Resources.ChartTooltipFormat_AvgDamage, point.X, point.Y));
            SolidColorBrush brush = new SolidColorBrush { Color = Colors.Blue };
            Pen lineThickness = new Pen(brush, 2);
            ElementPointMarker circlePointMarker = new CircleElementPointMarker { Size = 4, Fill = brush, Brush = brush };
            ChartAvgDamage.AddLineGraph(dataSource, lineThickness, circlePointMarker,
                                        new PenDescription(Resources.Resources.ChartLegend_AvgDamage));

            ChartAvgDamage.FitToView();
        }

        #endregion

        public virtual void Show()
        {
            ViewTyped.Loaded += OnShellViewActivated;
            ViewTyped.Show();
            UpdateChecker.CheckForUpdates();
        }

        private void OnShellViewActivated(object sender, EventArgs eventArgs)
        {
            ViewTyped.Loaded -= OnShellViewActivated;
            Action act = OnLoad;
            System.Windows.Threading.Dispatcher.CurrentDispatcher.BeginInvoke(act);
        }

        private void ViewClosing(object sender, CancelEventArgs e)
        {
            if (!e.Cancel)
            {
                e.Cancel = !IsCloseAllowed();
            }
        }

        public bool Close()
        {
            bool close = false;
            if (IsCloseAllowed())
            {
                CloseView();
                close = true;
            }
            return close;
        }

        private bool IsCloseAllowed()
        {
            return true;
        }

        public virtual void CloseView()
        {
            ViewTyped.Close();
        }

    }
}