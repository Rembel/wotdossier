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
using WotDossier.Applications.View;
using WotDossier.Applications.ViewModel.Rows;
using WotDossier.Dal;
using WotDossier.Domain;
using WotDossier.Domain.Entities;
using WotDossier.Domain.Player;
using WotDossier.Domain.Tank;
using WotDossier.Framework;
using WotDossier.Framework.Applications;
using WotDossier.Framework.EventAggregator;
using WotDossier.Framework.Forms.Commands;
using Common.Logging;

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
        private readonly SettingsReader _reader = new SettingsReader(WotDossierSettings.SettingsPath);

        private PlayerStatisticViewModel _playerStatistic;
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

        public DelegateCommand<object> OnRowDoubleClickCommand { get; set; }

        public PlayerStatisticViewModel PlayerStatistic
        {
            get { return _playerStatistic; }
            set
            {
                _playerStatistic = value;
                RaisePropertyChanged("PlayerStatistic");
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
            get { return _tanks; }
            set
            {
                _tanks = value;
                RaisePropertyChanged("Tanks");
            }
        }

        private ObservableCollection<SellInfo> _lastUsedTanks = new ObservableCollection<SellInfo>();
        public ObservableCollection<SellInfo> LastUsedTanks
        {
            get { return _lastUsedTanks; }
            set { _lastUsedTanks = value; }
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
            OnRowDoubleClickCommand = new DelegateCommand<object>(OnRowDoubleClick); 

            WeakEventHandler.SetAnyGenericHandler<ShellViewModel, CancelEventArgs>(
                h => view.Closing += new CancelEventHandler(h), h => view.Closing -= new CancelEventHandler(h), this, (s, e) => s.ViewClosing(s, e));
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
            PlayerStatistic = GetPlayerStatistic();

            if (PlayerStatistic != null)
            {
                InitChart();

                FileInfo cacheFile = CacheHelper.GetCacheFile(PlayerStatistic.Name);

                if (cacheFile != null)
                {
                    //convert dossier cache file to json
                    CacheHelper.BinaryCacheToJson(cacheFile);
                    Thread.Sleep(1000);
                    InitTanksStatistic(cacheFile);
                    InitLastUsedTanksChart();
                }
                else
                {
                    MessageBox.Show("Для указанного игрока не найдено данных по танкам в локальном кэше игры", "Warning",
                                    MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private void InitLastUsedTanksChart()
        {
            LastUsedTanks.Clear();
            IEnumerable<TankStatisticRowViewModel> viewModels = Tanks.Where(x => x.LastBattle.Date > PlayerStatistic.PreviousDate.Date);
            IEnumerable<SellInfo> items = viewModels.Select(x => new SellInfo {TankName = x.Tank, WinPercent = x.WinsPercentForPeriod, Battles = x.BattlesCountDelta});
            LastUsedTanks.AddMany(items);
        }

        private PlayerStatisticViewModel GetPlayerStatistic()
        {
            AppSettings settings = _reader.Get();
            if (settings == null || string.IsNullOrEmpty(settings.PlayerId) || string.IsNullOrEmpty(settings.Server))
            {
                MessageBox.Show("Please specify player name before", "Configuration", MessageBoxButton.OK,
                                MessageBoxImage.Warning);
                return null;
            }

            PlayerStat playerStat = null;

            try
            {
                playerStat = WotApiClient.Instance.LoadPlayerStat(settings);
            }
            catch (Exception e)
            {
                _log.Error(e);
                MessageBox.Show(Resources.Resources.ErrorMsg_GetPlayerData, Resources.Resources.WindowCaption_Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if (playerStat != null && playerStat.data.name.Equals(settings.PlayerId, StringComparison.InvariantCultureIgnoreCase))
            {
                PlayerEntity player = _dossierRepository.UpdatePlayerStatistic(settings.PlayerId, playerStat);

                var statisticEntities = _dossierRepository.GetPlayerStatistic(settings.PlayerId).ToList();

                PlayerStatisticEntity currentStatistic = statisticEntities.OrderByDescending(x => x.BattlesCount).First();
                List<PlayerStatisticViewModel> oldStatisticEntities = statisticEntities.Where(x => x.Id != currentStatistic.Id)
                    .Select(x => new PlayerStatisticViewModel(x)).ToList();

                PlayerStatisticViewModel currentStatisticViewModel = new PlayerStatisticViewModel(currentStatistic, oldStatisticEntities);
                currentStatisticViewModel.Name = player.Name;
                currentStatisticViewModel.Created = player.Creaded;
                currentStatisticViewModel.BattlesPerDay = currentStatisticViewModel.BattlesCount / (DateTime.Now - player.Creaded).Days;

                if (playerStat.data.clan.clan != null)
                {
                    currentStatisticViewModel.Clan = new PlayerStatisticClanViewModel(playerStat.data.clan);
                }

                return currentStatisticViewModel;
            }
            return null;
        }

        private void InitTanksStatistic(FileInfo cacheFile)
        {
           // Action act = () =>
                {
                    var playerName = CacheHelper.GetPlayerName(cacheFile);

                    List<TankJson> tanks = WotApiClient.Instance.ReadTanks(cacheFile.FullName.Replace(".dat", ".json"));

                    PlayerEntity playerEntity = _dossierRepository.UpdateTankStatistic(playerName, tanks);

                    if (playerEntity == null)
                    {
                        MessageBox.Show(Resources.Resources.ErrorMsg_GetPlayerInfo, Resources.Resources.WindowCaption_Error, MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    IEnumerable<TankStatisticEntity> entities = _dossierRepository.GetTanksStatistic(playerEntity);

                    Tanks = entities.GroupBy(x => x.TankId).Select(ToStatisticViewModel).OrderByDescending(x => x.Tier).ThenBy(x => x.Tank);

                    InitMasterTankerList(tanks);
                };

            //System.Windows.Threading.Dispatcher.CurrentDispatcher.BeginInvoke(act);
        }

        private void InitMasterTankerList(List<TankJson> tanks)
        {
            IEnumerable<KeyValuePair<int, int>> killed =
                tanks.SelectMany(x => x.Frags).Select(x => new KeyValuePair<int, int>(x.TankId, x.CountryId)).Distinct();
            IEnumerable<TankRowMasterTanker> masterTanker = WotApiClient.TankDictionary.Where(
                x => !killed.Contains(x.Key) && IsExistedtank(x.Value))
                                                                        .Select(
                                                                            x =>
                                                                            new TankRowMasterTanker(x.Value,
                                                                                                    WotApiClient.Instance
                                                                                                                .GetTankContour(
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
            ElementPointMarker circlePointMarker = new CircleElementPointMarker { Size = 7, Fill = brush, Brush = brush };
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
            circlePointMarker = new CircleElementPointMarker { Size = 7, Fill = brush, Brush = brush };
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
            ElementPointMarker circlePointMarker = new CircleElementPointMarker { Size = 7, Fill = brush, Brush = brush };
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
            ElementPointMarker circlePointMarker = new CircleElementPointMarker { Size = 7, Fill = brush, Brush = brush };
            ChartAvgDamage.AddLineGraph(dataSource, lineThickness, circlePointMarker,
                                        new PenDescription(Resources.Resources.ChartLegend_AvgDamage));

            ChartAvgDamage.FitToView();
        }

        #endregion

        public virtual void Show()
        {
            ViewTyped.Show();
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