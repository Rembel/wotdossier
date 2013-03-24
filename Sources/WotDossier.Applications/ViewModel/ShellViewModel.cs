using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Media;
using Microsoft.Research.DynamicDataDisplay;
using Microsoft.Research.DynamicDataDisplay.DataSources;
using Microsoft.Research.DynamicDataDisplay.PointMarkers;
using WotDossier.Applications.View;
using WotDossier.Dal;
using WotDossier.Domain;
using WotDossier.Domain.Entities;
using WotDossier.Domain.Player;
using WotDossier.Domain.Rows;
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
        private IEnumerable<TankRow> _battles;
        private IEnumerable<TankRow> _xp;
        private IEnumerable<TankRow> _frags;
        private IEnumerable<TankRow> _damage;
        private IEnumerable<TankRow> _battleAwards;
        private IEnumerable<TankRow> _specialAwards;
        private IEnumerable<TankRow> _series;
        private IEnumerable<TankRow> _medals;
        private IEnumerable<TankRow> _ratings;
        private IEnumerable<TankRow> _performance;
        private IEnumerable<TankRowMasterTanker> _masterTanker;
        private IEnumerable<TankRow> _epics;
        private IEnumerable<TankRow> _time;

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

        public IEnumerable<TankRow> Time
        {
            get { return _time; }
            set
            {
                _time = value;
                RaisePropertyChanged("Time");
            }
        }

        public IEnumerable<TankRow> Epics
        {
            get { return _epics; }
            set
            {
                _epics = value;
                RaisePropertyChanged("Epics");
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

        public IEnumerable<TankRow> Performance
        {
            get { return _performance; }
            set
            {
                _performance = value;
                RaisePropertyChanged("Performance");
            }
        }

        public IEnumerable<TankRow> Ratings
        {
            get { return _ratings; }
            set
            {
                _ratings = value;
                RaisePropertyChanged("Ratings");
            }
        }

        public IEnumerable<TankRow> Medals
        {
            get { return _medals; }
            set
            {
                _medals = value;
                RaisePropertyChanged("Medals");
            }
        }

        public IEnumerable<TankRow> Series
        {
            get { return _series; }
            set
            {
                _series = value;
                RaisePropertyChanged("Series");
            }
        }

        public IEnumerable<TankRow> SpecialAwards
        {
            get { return _specialAwards; }
            set
            {
                _specialAwards = value;
                RaisePropertyChanged("SpecialAwards");
            }
        }

        public IEnumerable<TankRow> BattleAwards
        {
            get { return _battleAwards; }
            set
            {
                _battleAwards = value;
                RaisePropertyChanged("BattleAwards");
            }
        }

        public IEnumerable<TankRow> Damage
        {
            get { return _damage; }
            set
            {
                _damage = value;
                RaisePropertyChanged("Damage");
            }
        }

        public IEnumerable<TankRow> Frags
        {
            get { return _frags; }
            set
            {
                _frags = value;
                RaisePropertyChanged("Frags");
            }
        }

        public IEnumerable<TankRow> Xp
        {
            get { return _xp; }
            set
            {
                _xp = value;
                RaisePropertyChanged("Xp");
            }
        }

        public IEnumerable<TankRow> Battles
        {
            get { return _battles; }
            set
            {
                _battles = value;
                RaisePropertyChanged("Battles");
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

            PlayerStat playerStat = Read.LoadPlayerStat(settings);

            PlayerEntity player;

            if (playerStat != null)
            {
                player = _dossierRepository.GetOrCreatePlayer(settings.PlayerId, playerStat);

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
                List<TankJson> tanks = Read.ReadTanks(cacheFile.FullName.Replace(".dat", ".json"));

                IEnumerable<TankRow> tankRows = tanks.Select(x => new TankRow(x)).OrderByDescending(x => x.Tier).ThenBy(x => x.Tank);

                //IEnumerable<TankRowBattles> battles = tanks.Select(x => new TankRowBattles(x)).OrderByDescending(x => x.Tier).ThenBy(x => x.Tank);
                //IEnumerable<TankRowXP> xp = tanks.Select(x => new TankRowXP(x)).OrderByDescending(x => x.Tier).ThenBy(x => x.Tank);
                //IEnumerable<TankRowFrags> frags = tanks.Select(x => new TankRowFrags(x)).OrderByDescending(x => x.Tier).ThenBy(x => x.Tank);
                //IEnumerable<TankRowDamage> damage = tanks.Select(x => new TankRowDamage(x)).OrderByDescending(x => x.Tier).ThenBy(x => x.Tank);
                //IEnumerable<TankRowBattleAwards> battleAwards = tanks.Select(x => new TankRowBattleAwards(x)).OrderByDescending(x => x.Tier).ThenBy(x => x.Tank);
                //IEnumerable<TankRowSpecialAwards> specialAwards = tanks.Select(x => new TankRowSpecialAwards(x)).OrderByDescending(x => x.Tier).ThenBy(x => x.Tank);
                //IEnumerable<TankRowSeries> series = tanks.Select(x => new TankRowSeries(x)).OrderByDescending(x => x.Tier).ThenBy(x => x.Tank);
                //IEnumerable<TankRowMedals> medals = tanks.Select(x => new TankRowMedals(x)).OrderByDescending(x => x.Tier).ThenBy(x => x.Tank);
                //IEnumerable<TankRowRatings> ratings = tanks.Select(x => new TankRowRatings(x)).OrderByDescending(x => x.Tier).ThenBy(x => x.Tank);
                //IEnumerable<TankRowPerformance> performance = tanks.Select(x => new TankRowPerformance(x)).OrderByDescending(x => x.Tier).ThenBy(x => x.Tank);
                //IEnumerable<TankRowEpic> epic = tanks.Select(x => new TankRowEpic(x)).OrderByDescending(x => x.Tier).ThenBy(x => x.Tank);
                //IEnumerable<TankRowTime> time = tanks.Select(x => new TankRowTime(x)).OrderByDescending(x => x.Tier).ThenBy(x => x.Tank);

                IEnumerable<KeyValuePair<int, int>> killed = tanks.SelectMany(x => x.Frags).Select(x => new KeyValuePair<int, int>(x.TankId, x.CountryId)).Distinct();
                IEnumerable<TankRowMasterTanker> masterTanker = Read.TankDictionary.Where(x => !killed.Contains(x.Key) && IsExistedtank(x.Value)).Select(x => new TankRowMasterTanker(x.Value, Read.GetTankContour(x.Value))).OrderBy(x => x.IsPremium).ThenBy(x => x.Tier);

                Battles = tankRows;
                Xp = tankRows;
                Frags = tankRows;
                Damage = tankRows;
                BattleAwards = tankRows;
                SpecialAwards = tankRows;
                Series = tankRows;
                Medals = tankRows;
                Ratings = tankRows;
                Performance = tankRows;
                MasterTanker= masterTanker;
                Epics = tankRows;
                Time = tankRows;
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