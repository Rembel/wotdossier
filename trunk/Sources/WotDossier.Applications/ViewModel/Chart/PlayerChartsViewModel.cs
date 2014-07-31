using System;
using System.Collections.Generic;
using System.Linq;
using Common.Logging;
using WotDossier.Applications.Logic;
using WotDossier.Applications.ViewModel.Replay;
using WotDossier.Applications.ViewModel.Rows;
using WotDossier.Applications.ViewModel.Statistic;
using WotDossier.Domain;
using WotDossier.Domain.Replay;

namespace WotDossier.Applications.ViewModel.Chart
{
    public class PlayerChartsViewModel : CommonChartsViewModel
    {
        #region [Properties and Fields]

        private static readonly ILog _log = LogManager.GetCurrentClassLogger();

        private List<SellInfo> _lastUsedTanksDataSource;

        private List<DataPoint> _efficiencyByTierDataSource;
        private List<DataPoint> _efficiencyByTypeDataSource;
        private List<DataPoint> _efficiencyByCountryDataSource;

        private List<DataPoint> _battlesByTierDataSource;
        private List<DataPoint> _battlesByTypeDataSource;
        private List<DataPoint> _battlesByCountryDataSource;

        private List<DataPoint> _replaysByMapDataSource;
        private double _maxMapBattles = 10;
        private double _maxWinReplayPercent = 100;
        private List<DataPoint> _winReplaysPercentByMapDataSource;
        private double _maxBattlesByType;
        private double _maxBattlesByTier;
        private double _maxBattlesByCountry;
        private IEnumerable<ReplayFile> _replaysDataSource;
        private bool _resp1;
        private bool _resp2;
        private bool _allResps = true;
        private DateTime? _startDate;
        private DateTime? _endDate = DateTime.Now;

        private List<ListItem<BattleType>> _battleTypes = new List<ListItem<BattleType>>
            {
                new ListItem<BattleType>(BattleType.Unknown, Resources.Resources.TankFilterPanel_All), 
                new ListItem<BattleType>(BattleType.Regular, Resources.Resources.BattleType_Regular), 
                new ListItem<BattleType>(BattleType.Historical,Resources.Resources.BattleType_Historical), 
                new ListItem<BattleType>(BattleType.CyberSport,Resources.Resources.BattleType_CyberSport), 
                new ListItem<BattleType>(BattleType.ClanWar, Resources.Resources.BattleType_ClanWar), 
                new ListItem<BattleType>(BattleType.CompanyWar,Resources.Resources.BattleType_CompanyWar), 
            };

        /// <summary>
        /// Gets the battle types.
        /// </summary>
        /// <value>
        /// The battle types.
        /// </value>
        public List<ListItem<BattleType>> BattleTypes
        {
            get { return _battleTypes; }
        }

        private BattleType _battleType;
        private List<DataPoint> _winPercentByTierDataSource;
        private List<DataPoint> _winPercentByTypeDataSource;
        private List<DataPoint> _winPercentByCountryDataSource;

        /// <summary>
        /// Gets or sets the type of the battle.
        /// </summary>
        /// <value>
        /// The type of the battle.
        /// </value>
        public BattleType BattleType
        {
            get { return _battleType; }
            set
            {
                _battleType = value;
                RefreshReplaysCharts();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="PlayerChartsViewModel" /> is resp1.
        /// </summary>
        /// <value>
        ///   <c>true</c> if resp1; otherwise, <c>false</c>.
        /// </value>
        public bool Resp1
        {
            get { return _resp1; }
            set
            {
                _resp1 = value;
                RefreshReplaysCharts();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="PlayerChartsViewModel" /> is resp2.
        /// </summary>
        /// <value>
        ///   <c>true</c> if resp2; otherwise, <c>false</c>.
        /// </value>
        public bool Resp2
        {
            get { return _resp2; }
            set
            {
                _resp2 = value;
                RefreshReplaysCharts();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [all resps].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [all resps]; otherwise, <c>false</c>.
        /// </value>
        public bool AllResps
        {
            get { return _allResps; }
            set
            {
                _allResps = value;
                RefreshReplaysCharts();
            }
        }

        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        /// <value>
        /// The start date.
        /// </value>
        public DateTime? StartDate
        {
            get { return _startDate; }
            set
            {
                _startDate = value;
                RefreshReplaysCharts();
            }
        }

        /// <summary>
        /// Gets or sets the end date.
        /// </summary>
        /// <value>
        /// The end date.
        /// </value>
        public DateTime? EndDate
        {
            get { return _endDate; }
            set
            {
                _endDate = value;
                RefreshReplaysCharts();
            }
        }

        /// <summary>
        /// Gets or sets the last used tanks data source.
        /// </summary>
        /// <value>
        /// The last used tanks data source.
        /// </value>
        public List<SellInfo> LastUsedTanksDataSource
        {
            get { return _lastUsedTanksDataSource; }
            set
            {
                _lastUsedTanksDataSource = value;
                RaisePropertyChanged("LastUsedTanksDataSource");
            }
        }

        /// <summary>
        /// Gets or sets the replays by map data source.
        /// </summary>
        /// <value>
        /// The replays by map data source.
        /// </value>
        public List<DataPoint> ReplaysByMapDataSource
        {
            get { return _replaysByMapDataSource; }
            set
            {
                _replaysByMapDataSource = value;
                RaisePropertyChanged("ReplaysByMapDataSource");
            }
        }

        /// <summary>
        /// Gets or sets the win replays percent by map data source.
        /// </summary>
        /// <value>
        /// The win replays percent by map data source.
        /// </value>
        public List<DataPoint> WinReplaysPercentByMapDataSource
        {
            get { return _winReplaysPercentByMapDataSource; }
            set
            {
                _winReplaysPercentByMapDataSource = value;
                RaisePropertyChanged("WinReplaysPercentByMapDataSource");
            }
        }

        /// <summary>
        /// Gets or sets the efficiency by tier data source.
        /// </summary>
        /// <value>
        /// The efficiency by tier data source.
        /// </value>
        public List<DataPoint> EfficiencyByTierDataSource
        {
            get { return _efficiencyByTierDataSource; }
            set
            {
                _efficiencyByTierDataSource = value;
                RaisePropertyChanged("EfficiencyByTierDataSource");
            }
        }

        /// <summary>
        /// Gets or sets the efficiency by type data source.
        /// </summary>
        /// <value>
        /// The efficiency by type data source.
        /// </value>
        public List<DataPoint> EfficiencyByTypeDataSource
        {
            get { return _efficiencyByTypeDataSource; }
            set
            {
                _efficiencyByTypeDataSource = value;
                RaisePropertyChanged("EfficiencyByTypeDataSource");
            }
        }

        /// <summary>
        /// Gets or sets the efficiency by country data source.
        /// </summary>
        /// <value>
        /// The efficiency by country data source.
        /// </value>
        public List<DataPoint> EfficiencyByCountryDataSource
        {
            get { return _efficiencyByCountryDataSource; }
            set
            {
                _efficiencyByCountryDataSource = value;
                RaisePropertyChanged("EfficiencyByCountryDataSource");
            }
        }

        /// <summary>
        /// Gets or sets the efficiency by tier data source.
        /// </summary>
        /// <value>
        /// The efficiency by tier data source.
        /// </value>
        public List<DataPoint> WinPercentByTierDataSource
        {
            get { return _winPercentByTierDataSource; }
            set
            {
                _winPercentByTierDataSource = value;
                RaisePropertyChanged("WinPercentByTierDataSource");
            }
        }

        /// <summary>
        /// Gets or sets the efficiency by type data source.
        /// </summary>
        /// <value>
        /// The efficiency by type data source.
        /// </value>
        public List<DataPoint> WinPercentByTypeDataSource
        {
            get { return _winPercentByTypeDataSource; }
            set
            {
                _winPercentByTypeDataSource = value;
                RaisePropertyChanged("WinPercentByTypeDataSource");
            }
        }

        /// <summary>
        /// Gets or sets the efficiency by country data source.
        /// </summary>
        /// <value>
        /// The efficiency by country data source.
        /// </value>
        public List<DataPoint> WinPercentByCountryDataSource
        {
            get { return _winPercentByCountryDataSource; }
            set
            {
                _winPercentByCountryDataSource = value;
                RaisePropertyChanged("WinPercentByCountryDataSource");
            }
        }

        /// <summary>
        /// Gets or sets the battles by tier data source.
        /// </summary>
        /// <value>
        /// The battles by tier data source.
        /// </value>
        public List<DataPoint> BattlesByTierDataSource
        {
            get { return _battlesByTierDataSource; }
            set
            {
                _battlesByTierDataSource = value;
                RaisePropertyChanged("BattlesByTierDataSource");
            }
        }

        /// <summary>
        /// Gets or sets the battles by type data source.
        /// </summary>
        /// <value>
        /// The battles by type data source.
        /// </value>
        public List<DataPoint> BattlesByTypeDataSource
        {
            get { return _battlesByTypeDataSource; }
            set
            {
                _battlesByTypeDataSource = value;
                RaisePropertyChanged("BattlesByTypeDataSource");
            }
        }

        /// <summary>
        /// Gets or sets the battles by country data source.
        /// </summary>
        /// <value>
        /// The battles by country data source.
        /// </value>
        public List<DataPoint> BattlesByCountryDataSource
        {
            get { return _battlesByCountryDataSource; }
            set
            {
                _battlesByCountryDataSource = value;
                RaisePropertyChanged("BattlesByCountryDataSource");
            }
        }

        /// <summary>
        /// Gets or sets the max map battles.
        /// </summary>
        /// <value>
        /// The max map battles.
        /// </value>
        public double MaxMapBattles
        {
            get { return _maxMapBattles; }
            set
            {
                _maxMapBattles = value;
                RaisePropertyChanged("MaxMapBattles");
            }
        }

        /// <summary>
        /// Gets or sets the max win replay percent.
        /// </summary>
        /// <value>
        /// The max win replay percent.
        /// </value>
        public double MaxWinReplayPercent
        {
            get { return _maxWinReplayPercent; }
            set
            {
                _maxWinReplayPercent = value;
                RaisePropertyChanged("MaxWinReplayPercent");
            }
        }

        /// <summary>
        /// Gets or sets the type of the max battles by.
        /// </summary>
        /// <value>
        /// The type of the max battles by.
        /// </value>
        public double MaxBattlesByType
        {
            get { return _maxBattlesByType; }
            set
            {
                _maxBattlesByType = value;
                RaisePropertyChanged("MaxBattlesByType");
            }
        }

        /// <summary>
        /// Gets or sets the max battles by tier.
        /// </summary>
        /// <value>
        /// The max battles by tier.
        /// </value>
        public double MaxBattlesByTier
        {
            get { return _maxBattlesByTier; }
            set
            {
                _maxBattlesByTier = value;
                RaisePropertyChanged("MaxBattlesByTier");
            }
        }

        /// <summary>
        /// Gets or sets the max battles by country.
        /// </summary>
        /// <value>
        /// The max battles by country.
        /// </value>
        public double MaxBattlesByCountry
        {
            get { return _maxBattlesByCountry; }
            set
            {
                _maxBattlesByCountry = value;
                RaisePropertyChanged("MaxBattlesByCountry");
            }
        }

        /// <summary>
        /// Gets or sets the replays data source.
        /// </summary>
        /// <value>
        /// The replays data source.
        /// </value>
        public IEnumerable<ReplayFile> ReplaysDataSource
        {
            get { return Filter(_replaysDataSource); }
            set { _replaysDataSource = value; }
        }

        #endregion

        /// <summary>
        /// Inits the battles by map chart.
        /// </summary>
        public void InitBattlesByMapChart()
        {
            List<DataPoint> dataSource = ReplaysDataSource.GroupBy(x => x.MapId).Select(x => new DataPoint(x.Count(), x.Key)).ToList();

            IEnumerable<ReplayFile> replayFiles = ReplaysDataSource.Where(x => x.MapId == 0);
            int count = replayFiles.Count();

            ReplaysByMapDataSource = dataSource;

            if (dataSource.Any())
            {
                double max = ReplaysByMapDataSource.Max(x => x.X);
                MaxMapBattles = max + 0.1 * max;
            }
        }

        /// <summary>
        /// Inits the win replays percent by map chart.
        /// </summary>
        public void InitWinReplaysPercentByMapChart()
        {
            List<DataPoint> dataSource = ReplaysDataSource.GroupBy(x => x.MapId).Select(
                x => new DataPoint(
                    100 * x.Sum(y => (y.IsWinner == BattleStatus.Victory ? 1.0 : 0.0)) / x.Count(), x.Key)).ToList();

            WinReplaysPercentByMapDataSource = dataSource;

            if (dataSource.Any())
            {
                double max = WinReplaysPercentByMapDataSource.Max(x => x.X);
                MaxWinReplayPercent = max;
            }
        }

        /// <summary>
        /// Inits the last used tanks chart.
        /// </summary>
        /// <param name="playerStatistic">The player statistic.</param>
        /// <param name="tanks">The tanks.</param>
        public void InitLastUsedTanksChart(PlayerStatisticViewModel playerStatistic, List<ITankStatisticRow> tanks)
        {
            IEnumerable<ITankStatisticRow> viewModels = tanks.Where(x => x.Updated > playerStatistic.PreviousDate);
            IEnumerable<SellInfo> items = viewModels.Select(x => new SellInfo { TankName = x.Tank, WinPercent = x.WinsPercentForPeriod, Battles = x.BattlesCountDelta });
            LastUsedTanksDataSource = items.ToList();
        }

        /// <summary>
        /// Inits the charts.
        /// </summary>
        /// <param name="playerStatistic">The player statistic.</param>
        /// <param name="tanks">The tanks.</param>
        public void InitCharts(PlayerStatisticViewModel playerStatistic, List<ITankStatisticRow> tanks)
        {
            _log.Trace("InitCharts start");

            InitCharts(playerStatistic);

            InitEfficiencyByTierChart(tanks);
            InitEfficiencyByTypeChart(tanks);
            InitEfficiencyByCountryChart(tanks);

            //InitWn8ByTierChart(tanks);
            //InitWn8ByTypeChart(tanks);
            //InitWn8ByCountryChart(tanks);

            InitWinPercentByTierChart(tanks);
            InitWinPercentByTypeChart(tanks);
            InitWinPercentByCountryChart(tanks);

            InitBattlesByTierChart(tanks);
            InitBattlesByTypeChart(tanks);
            InitBattlesByCountryChart(tanks);
            InitLastUsedTanksChart(playerStatistic, tanks);

            _log.Trace("InitCharts end");
        }

        private void RefreshReplaysCharts()
        {
            InitWinReplaysPercentByMapChart();
            InitBattlesByMapChart();
        }

        private IEnumerable<ReplayFile> Filter(IEnumerable<ReplayFile> replaysDataSource)
        {
            AppSettings settings = SettingsReader.Get();

            List<ReplayFile> replayFiles = replaysDataSource.Where(x =>
                (Resp1 && x.Team == 1
                 || Resp2 && x.Team == 2
                 || AllResps)
                &&
                (StartDate == null || x.PlayTime.Date >= StartDate)
                &&
                (EndDate == null || x.PlayTime.Date <= EndDate)
                && (settings.PlayerId == 0 || x.PlayerId == settings.PlayerId || x.PlayerName == settings.PlayerName)
                && (BattleType == BattleType.Unknown || x.BattleType == BattleType)
                && (settings.UseIncompleteReplaysResultsForCharts || x.IsWinner != BattleStatus.Incomplete)
                ).ToList();
            return replayFiles;

        }

        private void InitEfficiencyByTierChart(List<ITankStatisticRow> statisticViewModels)
        {
            IEnumerable<DataPoint> dataSource = statisticViewModels.GroupBy(x => x.Tier).Select(x => new DataPoint(x.Key, RatingHelper.EffectivityRating(
                x.Average(y => y.AvgDamageDealt),
                x.Key,
                x.Average(y => y.AvgFrags),
                x.Average(y => y.AvgSpotted),
                x.Average(y => y.AvgCapturePoints),
                x.Average(y => y.AvgDroppedCapturePoints))));
            EfficiencyByTierDataSource = dataSource.ToList();
        }

        private void InitEfficiencyByTypeChart(List<ITankStatisticRow> statisticViewModels)
        {
            IEnumerable<DataPoint> dataSource = statisticViewModels.GroupBy(x => x.Type).Select(x => new DataPoint(x.Key, RatingHelper.EffectivityRating(
                x.Average(y => y.AvgDamageDealt),
                x.Key,
                x.Average(y => y.AvgFrags),
                x.Average(y => y.AvgSpotted),
                x.Average(y => y.AvgCapturePoints),
                x.Average(y => y.AvgDroppedCapturePoints))));
            EfficiencyByTypeDataSource = dataSource.ToList();
        }

        private void InitEfficiencyByCountryChart(List<ITankStatisticRow> statisticViewModels)
        {
            IEnumerable<DataPoint> dataSource = statisticViewModels.GroupBy(x => x.CountryId).Select(x => new DataPoint(x.Key, RatingHelper.EffectivityRating(
                x.Average(y => y.AvgDamageDealt),
                x.Key,
                x.Average(y => y.AvgFrags),
                x.Average(y => y.AvgSpotted),
                x.Average(y => y.AvgCapturePoints),
                x.Average(y => y.AvgDroppedCapturePoints))));
            EfficiencyByCountryDataSource = dataSource.ToList();
        }

        private void InitWn8ByTierChart(List<ITankStatisticRow> statisticViewModels)
        {
            IEnumerable<DataPoint> dataSource = statisticViewModels.GroupBy(x => x.Tier)
                .Select(x => new DataPoint(x.Key, 
                    RatingHelper.Wn8(x.ToList())));
            EfficiencyByTierDataSource = dataSource.ToList();
        }

        private void InitWn8ByTypeChart(List<ITankStatisticRow> statisticViewModels)
        {
            IEnumerable<DataPoint> dataSource = statisticViewModels.GroupBy(x => x.Type)
                .Select(x => new DataPoint(x.Key,
                    RatingHelper.Wn8(x.ToList())));
            EfficiencyByTypeDataSource = dataSource.ToList();
        }

        private void InitWn8ByCountryChart(List<ITankStatisticRow> statisticViewModels)
        {
            IEnumerable<DataPoint> dataSource = statisticViewModels.GroupBy(x => x.CountryId)
                .Select(x => new DataPoint(x.Key,
                    RatingHelper.Wn8(x.ToList())));
            EfficiencyByCountryDataSource = dataSource.ToList();
        }

        private void InitWinPercentByTierChart(List<ITankStatisticRow> statisticViewModels)
        {
            IEnumerable<DataPoint> dataSource = statisticViewModels.GroupBy(x => x.Tier).Select(x => new DataPoint(x.Key,
                x.Sum(y => y.Wins) * 100.0 / x.Sum(y => y.BattlesCount)));
            WinPercentByTierDataSource = dataSource.ToList();
        }

        private void InitWinPercentByTypeChart(List<ITankStatisticRow> statisticViewModels)
        {
            IEnumerable<DataPoint> dataSource = statisticViewModels.GroupBy(x => x.Type).Select(x => new DataPoint(x.Key, 
                x.Sum(y => y.Wins) * 100.0/x.Sum(y => y.BattlesCount)));
            WinPercentByTypeDataSource = dataSource.ToList();
        }

        private void InitWinPercentByCountryChart(List<ITankStatisticRow> statisticViewModels)
        {
            IEnumerable<DataPoint> dataSource = statisticViewModels.GroupBy(x => x.CountryId).Select(x => new DataPoint(x.Key,
                x.Sum(y => y.Wins) * 100.0 / x.Sum(y => y.BattlesCount)));
            WinPercentByCountryDataSource = dataSource.ToList();
        }

        private void InitBattlesByTierChart(List<ITankStatisticRow> statisticViewModels)
        {
            List<DataPoint> dataSource = statisticViewModels.GroupBy(x => x.Tier).Select(x => new DataPoint(x.Key, x.Sum(y => y.BattlesCount))).ToList();
            if (dataSource.Any())
            {
                double max = dataSource.Max(x => x.Y);
                MaxBattlesByTier = max * 1.2;
                BattlesByTierDataSource = dataSource;
            }
        }

        private void InitBattlesByTypeChart(List<ITankStatisticRow> statisticViewModels)
        {
            List<DataPoint> dataSource = statisticViewModels.GroupBy(x => x.Type).Select(x => new DataPoint(x.Key, x.Sum(y => y.BattlesCount))).ToList();
            if (dataSource.Any())
            {
                double max = dataSource.Max(x => x.Y);
                MaxBattlesByType = max * 1.2;
                BattlesByTypeDataSource = dataSource;
            }
        }

        private void InitBattlesByCountryChart(List<ITankStatisticRow> statisticViewModels)
        {
            List<DataPoint> dataSource = statisticViewModels.GroupBy(x => x.CountryId).Select(x => new DataPoint(x.Key, x.Sum(y => y.BattlesCount))).ToList();
            if (dataSource.Any())
            {
                double max = dataSource.Max(x => x.Y);
                MaxBattlesByCountry = max * 1.2;
                BattlesByCountryDataSource = dataSource;
            }
        }
    }
}
