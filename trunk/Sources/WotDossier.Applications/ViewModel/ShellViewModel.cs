using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using WotDossier.Framework.Applications;
using WotDossier.Framework.EventAggregator;
using WotDossier.Framework.Forms.Commands;
using Common.Logging;

namespace WotDossier.Applications.ViewModel
{
    /// <summary>
    /// The ViewModel for the application's main window.
    /// </summary>
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
        private IEnumerable<TankRow> _tanks;

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

        public IEnumerable<TankRow> Tanks
        {
            get { return _tanks; }
            set
            {
                _tanks = value;
                RaisePropertyChanged("Tanks");
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ShellViewModel"/> class.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="dossierRepository"></param>
        public ShellViewModel(IShellView view, DossierRepository dossierRepository)
            : this(view, false)
        {
            _dossierRepository = dossierRepository;
            LoadCommand = new DelegateCommand(OnLoad);

            WeakEventHandler.SetAnyGenericHandler<ShellViewModel, CancelEventArgs>(
                h => view.Closing += new CancelEventHandler(h), h => view.Closing -= new CancelEventHandler(h), this, (s, e) => s.ViewClosing(s, e));
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
                IEnumerable<PlayerStatisticViewModel> statisticViewModels = playerEntities.Where(x => x.Id != currentStatistic.Id)
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

                    _dossierRepository.UpdateTankStatistic(playerName, tanks);

                    Tanks = tanks.Select(x => new TankRow(x)).OrderByDescending(x => x.Tier).ThenBy(x => x.Tank);

                    IEnumerable<KeyValuePair<int, int>> killed = tanks.SelectMany(x => x.Frags).Select(x => new KeyValuePair<int, int>(x.TankId, x.CountryId)).Distinct();
                    IEnumerable<TankRowMasterTanker> masterTanker = WotApiClient.TankDictionary.Where(x => !killed.Contains(x.Key) && IsExistedtank(x.Value))
                        .Select(x => new TankRowMasterTanker(x.Value, WotApiClient.Instance.GetTankContour(x.Value))).OrderBy(x => x.IsPremium).ThenBy(x => x.Tier);
                    MasterTanker = masterTanker;
            };

            System.Windows.Threading.Dispatcher.CurrentDispatcher.BeginInvoke(act);
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
}