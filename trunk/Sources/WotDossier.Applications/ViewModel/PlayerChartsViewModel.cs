using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using WotDossier.Applications.Logic;
using WotDossier.Applications.ViewModel.Replay;
using WotDossier.Applications.ViewModel.Rows;

namespace WotDossier.Applications.ViewModel
{
    public class PlayerChartsViewModel : CommonChartsViewModel
    {
        #region [Properties and Fields]

        private List<SellInfo> _lastUsedTanksDataSource;

        private List<DataPoint> _efficiencyByTierDataSource;
        private List<GenericPoint<string, double>> _efficiencyByTypeDataSource;
        private List<GenericPoint<string, double>> _efficiencyByCountryDataSource;

        private List<DataPoint> _battlesByTierDataSource;
        private List<GenericPoint<string, double>> _battlesByTypeDataSource;
        private List<GenericPoint<string, double>> _battlesByCountryDataSource;

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
        private DateTime? _endDate;

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
                {
                    RefreshReplaysCharts();
                }
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
                {
                    RefreshReplaysCharts();
                }
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
                {
                    RefreshReplaysCharts();
                }
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
        public List<GenericPoint<string, double>> EfficiencyByTypeDataSource
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
        public List<GenericPoint<string, double>> EfficiencyByCountryDataSource
        {
            get { return _efficiencyByCountryDataSource; }
            set
            {
                _efficiencyByCountryDataSource = value;
                RaisePropertyChanged("EfficiencyByCountryDataSource");
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
        public List<GenericPoint<string, double>> BattlesByTypeDataSource
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
        public List<GenericPoint<string, double>> BattlesByCountryDataSource
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

            if (dataSource.Any())
            {
                ReplaysByMapDataSource = dataSource;
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

            if (dataSource.Any())
            {
                WinReplaysPercentByMapDataSource = dataSource;
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
            InitCharts(playerStatistic);

            InitEfficiencyByTierChart(tanks);
            InitEfficiencyByTypeChart(tanks);
            InitEfficiencyByCountryChart(tanks);
            InitBattlesByTierChart(tanks);
            InitBattlesByTypeChart(tanks);
            InitBattlesByCountryChart(tanks);
            InitLastUsedTanksChart(playerStatistic, tanks);
        }

        private void RefreshReplaysCharts()
        {
            InitWinReplaysPercentByMapChart();
            InitBattlesByMapChart();
        }

        private IEnumerable<ReplayFile> Filter(IEnumerable<ReplayFile> replaysDataSource)
        {
            return replaysDataSource.Where(x =>
                                   (Resp1 && x.Team == 1
                                    || Resp2 && x.Team == 2
                                    || AllResps)
                                    &&
                                    (StartDate == null || x.PlayTime.Date >= StartDate)
                                    &&
                                    (EndDate == null || x.PlayTime.Date <= EndDate)
                ).ToList();

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
            IEnumerable<GenericPoint<string, double>> dataSource = statisticViewModels.GroupBy(x => x.Type).Select(x => new GenericPoint<string, double>(x.Key.ToString(CultureInfo.InvariantCulture), RatingHelper.EffectivityRating(
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
            IEnumerable<GenericPoint<string, double>> dataSource = statisticViewModels.GroupBy(x => x.CountryId).Select(x => new GenericPoint<string, double>(x.Key.ToString(CultureInfo.InvariantCulture), RatingHelper.EffectivityRating(
                x.Average(y => y.AvgDamageDealt),
                x.Key,
                x.Average(y => y.AvgFrags),
                x.Average(y => y.AvgSpotted),
                x.Average(y => y.AvgCapturePoints),
                x.Average(y => y.AvgDroppedCapturePoints))));
            EfficiencyByCountryDataSource = dataSource.ToList();
        }

        private void InitBattlesByTierChart(List<ITankStatisticRow> statisticViewModels)
        {
            List<DataPoint> dataSource = statisticViewModels.GroupBy(x => x.Tier).Select(x => new DataPoint(x.Key, x.Sum(y => y.BattlesCount))).ToList();
            if (dataSource.Any())
            {
                BattlesByTierDataSource = dataSource;
                double max = dataSource.Max(x => x.Y);
                MaxBattlesByTier = max*1.2;
            }
        }

        private void InitBattlesByTypeChart(List<ITankStatisticRow> statisticViewModels)
        {
            List<GenericPoint<string, double>> dataSource = statisticViewModels.GroupBy(x => x.Type).Select(x => new GenericPoint<string, double>(x.Key.ToString(CultureInfo.InvariantCulture), x.Sum(y => y.BattlesCount))).ToList();
            if (dataSource.Any())
            {
                BattlesByTypeDataSource = dataSource;
                double max = dataSource.Max(x => x.Y);
                MaxBattlesByType = max*1.2;
            }
        }

        private void InitBattlesByCountryChart(List<ITankStatisticRow> statisticViewModels)
        {
            List<GenericPoint<string, double>> dataSource = statisticViewModels.GroupBy(x => x.CountryId).Select(x => new GenericPoint<string, double>(x.Key.ToString(CultureInfo.InvariantCulture), x.Sum(y => y.BattlesCount))).ToList();
            if (dataSource.Any())
            {
                BattlesByCountryDataSource = dataSource;
                double max = dataSource.Max(x => x.Y);
                MaxBattlesByCountry = max * 1.2;
            }
        }
    }
}
