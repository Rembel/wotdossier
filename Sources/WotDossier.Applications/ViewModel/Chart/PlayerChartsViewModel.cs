using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Common.Logging;
using WotDossier.Applications.Logic;
using WotDossier.Applications.ViewModel.Filter;
using WotDossier.Applications.ViewModel.Replay;
using WotDossier.Applications.ViewModel.Rows;
using WotDossier.Applications.ViewModel.Statistic;
using WotDossier.Domain;
using WotDossier.Domain.Interfaces;

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
        
        private List<DataPoint> _winPercentByTierDataSource;
        private List<DataPoint> _winPercentByTypeDataSource;
        private List<DataPoint> _winPercentByCountryDataSource;

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
            get { return ReplaysFilter.Filter(_replaysDataSource, true); }
            set { _replaysDataSource = value; }
        }

        private ReplaysFilterViewModel _replaysFilter;
        private string _totalReplaysCount;
        private string _totalWinPercent;

        public ReplaysFilterViewModel ReplaysFilter
        {
            get { return _replaysFilter; }
        }

        #endregion

        public string TotalReplaysCount
        {
            get { return _totalReplaysCount; }
            set
            {
                _totalReplaysCount = value;
                RaisePropertyChanged("TotalReplaysCount");
            }
        }

        public string TotalWinPercent
        {
            get { return _totalWinPercent; }
            set
            {
                _totalWinPercent = value;
                RaisePropertyChanged("TotalWinPercent");
            }
        }

        public PlayerChartsViewModel()
        {
            _replaysFilter = new ReplaysFilterViewModel();
            _replaysFilter.FilterChanged += RefreshReplaysCharts; //refresh charts on filter changes
        }

        /// <summary>
        /// Inits the battles by map chart.
        /// </summary>
        public void InitBattlesByMapChart()
        {
            var replaysDataSource = ReplaysDataSource.Distinct(new ReplaysComparer());
            List<DataPoint> dataSource = replaysDataSource.GroupBy(x => x.MapId).Select(x => new DataPoint(x.Count(), x.Key)).ToList();

            var totalReplaysCount = replaysDataSource.Count();

            TotalReplaysCount = string.Format(Resources.Resources.Chart_Replays_Total_Count, totalReplaysCount);

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
            var replaysDataSource = ReplaysDataSource.Distinct(new ReplaysComparer());
            List<DataPoint> dataSource = replaysDataSource.GroupBy(x => x.MapId).Select(
                x => new DataPoint(
                    100 * x.Sum(y => (y.IsWinner == BattleStatus.Victory ? 1.0 : 0.0)) / x.Count(), x.Key)).ToList();

            var count = replaysDataSource.Count();
            double totalWinPercent = 0;
            if (count > 0)
            {
                totalWinPercent = replaysDataSource.Count(y => y.IsWinner == BattleStatus.Victory)*100.0/count;
            }
            TotalWinPercent = string.Format(Resources.Resources.Chart_Replays_Total_Win_Percent, totalWinPercent);

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
            IEnumerable<ITankStatisticRow> viewModels = tanks.Where(x => x.Updated > playerStatistic.PrevStatisticSliceDate);
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

            //InitEfficiencyByTierChart(tanks);
            //InitEfficiencyByTypeChart(tanks);
            //InitEfficiencyByCountryChart(tanks);

            InitWn8ByTierChart(tanks);
            InitWn8ByTypeChart(tanks);
            InitWn8ByCountryChart(tanks);

            InitWinPercentByTierChart(tanks);
            InitWinPercentByTypeChart(tanks);
            InitWinPercentByCountryChart(tanks);

            InitBattlesByTierChart(tanks);
            InitBattlesByTypeChart(tanks);
            InitBattlesByCountryChart(tanks);
            InitLastUsedTanksChart(playerStatistic, tanks);

            _log.Trace("InitCharts end");
        }

        public void RefreshReplaysCharts()
        {
            InitWinReplaysPercentByMapChart();
            InitBattlesByMapChart();
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
            IEnumerable<DataPoint> dataSource = statisticViewModels.Where(x => x.Type != (int)TankType.Unknown).GroupBy(x => x.Type).Select(x => new DataPoint(x.Key, RatingHelper.EffectivityRating(
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
            var values = Enumerable.Range(1, 10);

            IEnumerable<DataPoint> dataSource = statisticViewModels.GroupBy(x => x.Tier)
                .Select(x => new DataPoint(x.Key, 
                    RatingHelper.Wn8(x.ToList())));

            var list = dataSource.ToList();

            var query = from value in values
                        join point in list on value equals point.X into gj
                        from subpet in gj.DefaultIfEmpty()
                        select subpet ?? new DataPoint(value, 0);

            EfficiencyByTierDataSource = query.ToList();
        }

        private void InitWn8ByTypeChart(List<ITankStatisticRow> statisticViewModels)
        {
            var values = Enum.GetValues(typeof(TankType)).Cast<int>().Where(x => x >= 0);

            IEnumerable<DataPoint> dataSource = statisticViewModels.Where(x => x.Type != (int)TankType.Unknown).GroupBy(x => x.Type)
                .Select(x => new DataPoint(x.Key,
                    RatingHelper.Wn8(x.ToList())));

            var list = dataSource.ToList();

            var query = from value in values
                        join point in list on value equals point.X into gj
                        from subpet in gj.DefaultIfEmpty()
                        select subpet ?? new DataPoint(value, 0);

            EfficiencyByTypeDataSource = query.ToList();
        }

        private void InitWn8ByCountryChart(List<ITankStatisticRow> statisticViewModels)
        {
            var values = Enum.GetValues(typeof(Country)).Cast<int>().Where(x => x >= 0);

            IEnumerable<DataPoint> dataSource = statisticViewModels.GroupBy(x => x.CountryId)
                .Select(x => new DataPoint(x.Key,
                    RatingHelper.Wn8(x.ToList())));
            var list = dataSource.ToList();

            var query = from value in values
                        join point in list on value equals point.X into gj
                        from subpet in gj.DefaultIfEmpty()
                        select  subpet ?? new DataPoint(value, 0);

            EfficiencyByCountryDataSource = query.ToList();
        }

        private void InitWinPercentByTierChart(List<ITankStatisticRow> statisticViewModels)
        {
            var values = Enumerable.Range(1, 10);

            IEnumerable<DataPoint> dataSource = statisticViewModels.GroupBy(x => x.Tier).Select(x => new DataPoint(x.Key,
                x.Sum(y => ((IStatisticBattles) y).Wins) * 100.0 / x.Sum(y => y.BattlesCount)));

            var list = dataSource.ToList();

            var query = from value in values
                        join point in list on value equals point.X into gj
                        from subpet in gj.DefaultIfEmpty()
                        select subpet ?? new DataPoint(value, 0);

            WinPercentByTierDataSource = query.ToList();
        }

        private void InitWinPercentByTypeChart(List<ITankStatisticRow> statisticViewModels)
        {
            var values = Enum.GetValues(typeof(TankType)).Cast<int>().Where(x => x >= 0);

            IEnumerable<DataPoint> dataSource = statisticViewModels.Where(x => x.Type != (int)TankType.Unknown).GroupBy(x => x.Type).Select(x => new DataPoint(x.Key, 
                x.Sum(y => ((IStatisticBattles) y).Wins) * 100.0/x.Sum(y => y.BattlesCount)));

            var list = dataSource.ToList();

            var query = from value in values
                        join point in list on value equals point.X into gj
                        from subpet in gj.DefaultIfEmpty()
                        select subpet ?? new DataPoint(value, 0);

            WinPercentByTypeDataSource = query.ToList();
        }

        private void InitWinPercentByCountryChart(List<ITankStatisticRow> statisticViewModels)
        {
            var values = Enum.GetValues(typeof(Country)).Cast<int>().Where(x => x >= 0);

            IEnumerable<DataPoint> dataSource = statisticViewModels.GroupBy(x => x.CountryId).Select(x => new DataPoint(x.Key,
                x.Sum(y => ((IStatisticBattles) y).Wins) * 100.0 / x.Sum(y => y.BattlesCount)));

            var list = dataSource.ToList();

            var query = from value in values
                        join point in list on value equals point.X into gj
                        from subpet in gj.DefaultIfEmpty()
                        select subpet ?? new DataPoint(value, 0);

            WinPercentByCountryDataSource = query.ToList();
        }

        private void InitBattlesByTierChart(List<ITankStatisticRow> statisticViewModels)
        {
            var values = Enumerable.Range(1, 10);

            List<DataPoint> dataSource = statisticViewModels.GroupBy(x => x.Tier).Select(x => new DataPoint(x.Key, x.Sum(y => y.BattlesCount))).ToList();
            if (dataSource.Any())
            {
                double max = dataSource.Max(x => x.Y);
                MaxBattlesByTier = max * 1.2;

                var list = dataSource.ToList();

                var query = from value in values
                            join point in list on value equals point.X into gj
                            from subpet in gj.DefaultIfEmpty()
                            select subpet ?? new DataPoint(value, 0);

                BattlesByTierDataSource = query.ToList();
            }
        }

        private void InitBattlesByTypeChart(List<ITankStatisticRow> statisticViewModels)
        {
            var values = Enum.GetValues(typeof(TankType)).Cast<int>().Where(x => x >= 0);

            List<DataPoint> dataSource = statisticViewModels.Where(x => x.Type != (int)TankType.Unknown).GroupBy(x => x.Type).Select(x => new DataPoint(x.Key, x.Sum(y => y.BattlesCount))).ToList();
            if (dataSource.Any())
            {
                double max = dataSource.Max(x => x.Y);
                MaxBattlesByType = max * 1.2;

                var list = dataSource.ToList();

                var query = from value in values
                            join point in list on value equals point.X into gj
                            from subpet in gj.DefaultIfEmpty()
                            select subpet ?? new DataPoint(value, 0);

                BattlesByTypeDataSource = query.ToList();
            }
        }

        private void InitBattlesByCountryChart(List<ITankStatisticRow> statisticViewModels)
        {
            var values = Enum.GetValues(typeof(Country)).Cast<int>().Where(x => x >= 0);

            List<DataPoint> dataSource = statisticViewModels.GroupBy(x => x.CountryId).Select(x => new DataPoint(x.Key, x.Sum(y => y.BattlesCount))).ToList();
            if (dataSource.Any())
            {
                double max = dataSource.Max(x => x.Y);
                MaxBattlesByCountry = max * 1.2;

                var list = dataSource.ToList();

                var query = from value in values
                            join point in list on value equals point.X into gj
                            from subpet in gj.DefaultIfEmpty()
                            select subpet ?? new DataPoint(value, 0);

                BattlesByCountryDataSource = query.ToList();
            }
        }
    }

    public class ReplaysComparer : IEqualityComparer<ReplayFile>
    {
        public bool Equals(ReplayFile x, ReplayFile y)
        {
            return x.ReplayId == y.ReplayId;
        }

        public int GetHashCode(ReplayFile replayFile)
        {
            return replayFile.ReplayId.GetHashCode();
        }
    }
}
