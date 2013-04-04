using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Media;
using Microsoft.Research.DynamicDataDisplay;
using Microsoft.Research.DynamicDataDisplay.DataSources;
using Microsoft.Research.DynamicDataDisplay.PointMarkers;
using WotDossier.Applications.View;
using WotDossier.Applications.ViewModel.Rows;
using WotDossier.Common;
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

        private const string BATTLES_RATING_MARKER_TOOLTIP_FORMAT = "Battles: {0}\nRating: {1:0.00}";
        private const string BATTLES_WIN_PERCENT_MARKER_TOOLTIP_FORMAT = "Battles: {0}\nWin percent: {1:0.00}";
        private const string BATTLES_AVG_DAMAGE_MARKER_TOOLTIP_FORMAT = "Battles: {0}\nAvg Damage: {1:0.00}";
        private readonly DossierRepository _dossierRepository;
        private readonly SettingsReader _reader = new SettingsReader(WotDossierSettings.SettingsPath);

        private PlayerStatisticViewModel _playerStatistic;
        private IEnumerable<TankRowMasterTanker> _masterTanker;
        private IEnumerable<TankStatisticRowViewModel> _tanks;
        
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
            AppSettings settings = _reader.Get();

            PlayerStatistic = GetPlayerStatistic();

            FileInfo cacheFile = CacheHelper.GetCacheFile();

            if (cacheFile != null)
            {
                CacheHelper.BinaryCacheToJson(cacheFile);

                Thread.Sleep(1000);
             
                InitTanksStatistic(cacheFile);
            }
        }

        private PlayerStatisticViewModel GetPlayerStatistic()
        {
            AppSettings settings = _reader.Get();
            if (settings == null || string.IsNullOrEmpty(settings.PlayerId) || string.IsNullOrEmpty(settings.Server))
            {
                return null;
            }

            PlayerStat playerStat = WotApiClient.Instance.LoadPlayerStat(settings);

            if (playerStat != null)
            {
                PlayerEntity player = _dossierRepository.UpdatePlayerStatistic(settings.PlayerId, playerStat);

                var playerEntities = _dossierRepository.GetPlayerStatistic(settings.PlayerId).ToList();

                PlayerStatisticEntity currentStatistic = playerEntities.OrderByDescending(x => x.Updated).First();
                List<PlayerStatisticViewModel> statisticViewModels = playerEntities.Where(x => x.Id != currentStatistic.Id)
                    .Select(x => new PlayerStatisticViewModel(x)).ToList();

                IEnumerable<PlayerStatisticViewModel> viewModels = playerEntities.Select(x => new PlayerStatisticViewModel(x));

                InitChart(viewModels);

                PlayerStatisticViewModel playerStatisticViewModel = new PlayerStatisticViewModel(currentStatistic, statisticViewModels);
                playerStatisticViewModel.Name = player.Name;
                playerStatisticViewModel.Created = player.Creaded;
                playerStatisticViewModel.BattlesPerDay = playerStatisticViewModel.BattlesCount / (DateTime.Now - player.Creaded).Days;

                if (playerStat.data.clan.clan != null)
                {
                    playerStatisticViewModel.Clan = new PlayerStatisticClanViewModel(playerStat.data.clan);
                }

                return playerStatisticViewModel;
            }
            return null;
        }

        private void InitChart(IEnumerable<PlayerStatisticViewModel> statisticViewModels)
        {
            InitRatingChart(statisticViewModels);
            InitWinPercentChart(statisticViewModels);
            InitAvgDamageChart(statisticViewModels);
        }

        private void InitRatingChart(IEnumerable<PlayerStatisticViewModel> statisticViewModels)
        {
            DataRect dataRect = DataRect.Create(0, 0, 100000, 2500);
            ChartRating.Viewport.Domain = dataRect;
            ChartRating.Viewport.Visible = dataRect;

            ChartRating.RemoveUserElements();

            IEnumerable<DataPoint> erPoints = statisticViewModels.Select(x => new DataPoint(x.BattlesCount, x.EffRating));
            var dataSource = new EnumerableDataSource<DataPoint>(erPoints) {XMapping = x => x.X, YMapping = y => y.Y};
            dataSource.AddMapping(ShapeElementPointMarker.ToolTipTextProperty, point => String.Format(BATTLES_RATING_MARKER_TOOLTIP_FORMAT, point.X, point.Y));
            SolidColorBrush brush = new SolidColorBrush {Color = Colors.Blue};
            Pen lineThickness = new Pen(brush, 2);
            ElementPointMarker circlePointMarker = new CircleElementPointMarker {Size = 7, Fill = brush, Brush = brush};
            ChartRating.AddLineGraph(dataSource, lineThickness, circlePointMarker, new PenDescription("РЭ"));

            IEnumerable<DataPoint> wn6Points = statisticViewModels.Select(x => new DataPoint(x.BattlesCount, x.WN6Rating));
            dataSource = new EnumerableDataSource<DataPoint>(wn6Points) {XMapping = x => x.X, YMapping = y => y.Y};
            dataSource.AddMapping(ShapeElementPointMarker.ToolTipTextProperty,
                                  point => String.Format(BATTLES_RATING_MARKER_TOOLTIP_FORMAT, point.X, point.Y));
            brush = new SolidColorBrush {Color = Colors.Green};
            lineThickness = new Pen(brush, 2);
            circlePointMarker = new CircleElementPointMarker {Size = 7, Fill = brush, Brush = brush};
            ChartRating.AddLineGraph(dataSource, lineThickness, circlePointMarker, new PenDescription("WN6"));

            ChartRating.FitToView();
        }

        private void InitWinPercentChart(IEnumerable<PlayerStatisticViewModel> statisticViewModels)
        {
            ChartWinPercent.RemoveUserElements();

            IEnumerable<DataPoint> erPoints = statisticViewModels.Select(x => new DataPoint(x.BattlesCount, x.WinsPercent));
            var dataSource = new EnumerableDataSource<DataPoint>(erPoints) { XMapping = x => x.X, YMapping = y => y.Y };
            dataSource.AddMapping(ShapeElementPointMarker.ToolTipTextProperty, point => String.Format(BATTLES_WIN_PERCENT_MARKER_TOOLTIP_FORMAT, point.X, point.Y));
            SolidColorBrush brush = new SolidColorBrush { Color = Colors.Blue };
            Pen lineThickness = new Pen(brush, 2);
            ElementPointMarker circlePointMarker = new CircleElementPointMarker { Size = 7, Fill = brush, Brush = brush };
            ChartWinPercent.AddLineGraph(dataSource, lineThickness, circlePointMarker, new PenDescription("Win %"));

            ChartWinPercent.FitToView();
        }

        private void InitAvgDamageChart(IEnumerable<PlayerStatisticViewModel> statisticViewModels)
        {
            ChartAvgDamage.RemoveUserElements();

            IEnumerable<DataPoint> erPoints = statisticViewModels.Select(x => new DataPoint(x.BattlesCount, x.DamageDealt / (double)x.BattlesCount));
            var dataSource = new EnumerableDataSource<DataPoint>(erPoints) { XMapping = x => x.X, YMapping = y => y.Y };
            dataSource.AddMapping(ShapeElementPointMarker.ToolTipTextProperty, point => String.Format(BATTLES_AVG_DAMAGE_MARKER_TOOLTIP_FORMAT, point.X, point.Y));
            SolidColorBrush brush = new SolidColorBrush { Color = Colors.Blue };
            Pen lineThickness = new Pen(brush, 2);
            ElementPointMarker circlePointMarker = new CircleElementPointMarker { Size = 7, Fill = brush, Brush = brush };
            ChartAvgDamage.AddLineGraph(dataSource, lineThickness, circlePointMarker, new PenDescription("Avg damage"));

            ChartAvgDamage.FitToView();
        }

        private void InitTanksStatistic(FileInfo cacheFile)
        {
            Action act = () =>
                {
                    Base32Encoder encoder = new Base32Encoder();
                    
                    string str = cacheFile.Name.Replace(cacheFile.Extension, string.Empty);
                    byte[] decodedFileNameBytes = encoder.Decode(str.ToLowerInvariant());
                    string decodedFileName = Encoding.UTF8.GetString(decodedFileNameBytes);

                    string playerName = decodedFileName.Split(';')[1];

                    List<TankJson> tanks = WotApiClient.Instance.ReadTanks(cacheFile.FullName.Replace(".dat", ".json"));

                    PlayerEntity playerEntity = _dossierRepository.UpdateTankStatistic(playerName, tanks);

                    List<TankStatisticEntity> entities = _dossierRepository.GetTanksStatistic(playerEntity).ToList();

                    Tanks = GetViewModels(entities); 

                    IEnumerable<KeyValuePair<int, int>> killed = tanks.SelectMany(x => x.Frags).Select(x => new KeyValuePair<int, int>(x.TankId, x.CountryId)).Distinct();
                    IEnumerable<TankRowMasterTanker> masterTanker = WotApiClient.TankDictionary.Where(x => !killed.Contains(x.Key) && IsExistedtank(x.Value))
                        .Select(x => new TankRowMasterTanker(x.Value, WotApiClient.Instance.GetTankContour(x.Value))).OrderBy(x => x.IsPremium).ThenBy(x => x.Tier);
                    MasterTanker = masterTanker;

                    if (PlayerStatistic != null)
                    {
                        IEnumerable<TankStatisticRowViewModel> viewModels = Tanks.Where(x => x.LastBattle >= PlayerStatistic.PreviousDate);
                        IEnumerable<SellInfo> items = viewModels.Select(x => new SellInfo {TankName = x.Tank, WinPercent = x.WinsPercentForPeriod, Battles = x.BattlesCountDelta});
                        LastUsedTanks.AddMany(items);
                    }
                };

            System.Windows.Threading.Dispatcher.CurrentDispatcher.BeginInvoke(act);
        }

        private IEnumerable<TankStatisticRowViewModel> GetViewModels(List<TankStatisticEntity> entities)
        {
            return entities.GroupBy(x => x.TankId).Select(ToStatisticViewModel).OrderByDescending(x => x.Tier).ThenBy(x => x.Tank);
        }

        private TankStatisticRowViewModel ToStatisticViewModel(IGrouping<int, TankStatisticEntity> tankStatisticEntities)
        {
            TankStatisticEntity currentStatistic = tankStatisticEntities.OrderByDescending(x => x.Updated).First();
            IEnumerable<TankJson> statisticViewModels = tankStatisticEntities.Where(x => x.Id != currentStatistic.Id)
                .Select(x => UnZipObject(x.Raw)).ToList();
            return new TankStatisticRowViewModel(UnZipObject(currentStatistic.Raw), statisticViewModels);
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

    public class OpenTankStatisticEvent : BaseEvent<TankStatisticRowViewModel>
    {
    }
}