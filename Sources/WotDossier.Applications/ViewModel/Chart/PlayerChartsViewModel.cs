using System;
using System.Collections.Generic;
using System.Linq;
using Common.Logging;
using WotDossier.Applications.Logic;
using WotDossier.Applications.ViewModel.Filter;
using WotDossier.Applications.ViewModel.Replay;
using WotDossier.Applications.ViewModel.Rows;
using WotDossier.Applications.ViewModel.Statistic;
using WotDossier.Common;
using WotDossier.Domain;

namespace WotDossier.Applications.ViewModel.Chart
{
    public class PlayerChartsViewModel : CommonChartsViewModel
    {
        #region [Properties and Fields]

        private static readonly ILog _log = LogManager.GetCurrentClassLogger();

        private List<IDataPoint> _lastUsedTanksDataSource;
        private IEnumerable<ReplayFile> _replaysDataSource;

        private List<IDataPoint> _efficiencyByTierDataSource;
        private List<IDataPoint> _efficiencyByTypeDataSource;
        private List<IDataPoint> _efficiencyByCountryDataSource;

        private List<IDataPoint> _battlesByTierDataSource;
        private List<IDataPoint> _battlesByTypeDataSource;
        private List<IDataPoint> _battlesByCountryDataSource;

        private List<IDataPoint> _winPercentByTierDataSource;
        private List<IDataPoint> _winPercentByTypeDataSource;
        private List<IDataPoint> _winPercentByCountryDataSource;

        /// <summary>
        /// Gets or sets the last used tanks data source.
        /// </summary>
        /// <value>
        /// The last used tanks data source.
        /// </value>
        public List<IDataPoint> LastUsedTanksDataSource
        {
            get { return _lastUsedTanksDataSource; }
            set
            {
                _lastUsedTanksDataSource = value;
                RaisePropertyChanged("LastUsedTanksDataSource");
            }
        }

        /// <summary>
        /// Gets or sets the efficiency by tier data source.
        /// </summary>
        /// <value>
        /// The efficiency by tier data source.
        /// </value>
        public List<IDataPoint> EfficiencyByTierDataSource
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
        public List<IDataPoint> EfficiencyByTypeDataSource
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
        public List<IDataPoint> EfficiencyByCountryDataSource
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
        public List<IDataPoint> WinPercentByTierDataSource
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
        public List<IDataPoint> WinPercentByTypeDataSource
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
        public List<IDataPoint> WinPercentByCountryDataSource
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
        public List<IDataPoint> BattlesByTierDataSource
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
        public List<IDataPoint> BattlesByTypeDataSource
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
        public List<IDataPoint> BattlesByCountryDataSource
        {
            get { return _battlesByCountryDataSource; }
            set
            {
                _battlesByCountryDataSource = value;
                RaisePropertyChanged("BattlesByCountryDataSource");
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
        public ReplaysFilterViewModel ReplaysFilter
        {
            get { return _replaysFilter; }
        }

        private List<MapStat> _mapsStat;
        public List<MapStat> MapsStat
        {
            get { return _mapsStat; }
            set
            {
                _mapsStat = value;
                RaisePropertyChanged("MapsStat");
            }
        }

        #endregion

        public PlayerChartsViewModel()
        {
            _replaysFilter = new ReplaysFilterViewModel();
            _replaysFilter.FilterChanged += RefreshReplaysCharts; //refresh charts on filter changes
        }

        /// <summary>
        /// Inits the battles by map chart.
        /// </summary>
        public void InitReplaysStat()
        {
            var replaysDataSource = ReplaysDataSource.Distinct(new ReplaysComparer());

            MapsStat = replaysDataSource.GroupBy(x => x.MapNameId).Select(x => new MapStat(x.ToList(), x.Key)).ToList();
        }

        /// <summary>
        /// Inits the last used tanks chart.
        /// </summary>
        /// <param name="playerStatistic">The player statistic.</param>
        /// <param name="tanks">The tanks.</param>
        public void InitLastUsedTanksChart(PlayerStatisticViewModel playerStatistic, List<ITankStatisticRow> tanks)
        {
            IEnumerable<ITankStatisticRow> viewModels = tanks.Where(x => x.Updated > playerStatistic.PrevStatisticSliceDate);
            IEnumerable<IDataPoint> items = viewModels.Select(x => new SellInfo (x.BattlesCountDelta, x.WinsPercentForPeriod, x.Tank));
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
            InitReplaysStat();
        }

        private void InitWn8ByTierChart(List<ITankStatisticRow> statisticViewModels)
        {
            var values = Enumerable.Range(1, 10);

            EfficiencyByTierDataSource = InitDataPointSource(
                values,
                statisticViewModels,
                row => (int)row.Tier,
                group => Math.Round(RatingHelper.Wn8(group.ToList()), 1),
                value => value.ToString());
        }

        private void InitWn8ByTypeChart(List<ITankStatisticRow> statisticViewModels)
        {
            var values = Enum.GetValues(typeof(TankType)).Cast<int>().Where(x => x > 0);

            EfficiencyByTypeDataSource = InitDataPointSource(
                values,
                statisticViewModels,
                row => row.Type,
                group => Math.Round(RatingHelper.Wn8(group.ToList()), 1),
                value => Resources.Resources.ResourceManager.GetEnumResource((TankType)value));
        }

        private void InitWn8ByCountryChart(List<ITankStatisticRow> statisticViewModels)
        {
            var values = Enum.GetValues(typeof(Country)).Cast<int>().Where(x => x >= 0);

            EfficiencyByCountryDataSource = InitDataPointSource(
                values,
                statisticViewModels,
                row => row.CountryId,
                group => Math.Round(RatingHelper.Wn8(group.ToList()), 1),
                value => Resources.Resources.ResourceManager.GetEnumResource((Country)value));
        }

        private void InitWinPercentByTierChart(List<ITankStatisticRow> statisticViewModels)
        {
            var values = Enumerable.Range(1, 10);

            WinPercentByTierDataSource = InitDataPointSource(
                values,
                statisticViewModels,
                row => (int) row.Tier,
                group => group.Any(x => x.BattlesCount > 0) ? Math.Round(group.Sum(y => y.Wins) * 100.0 / group.Sum(y => y.BattlesCount), 1) : 0,
                value => value.ToString());
        }

        private void InitWinPercentByTypeChart(List<ITankStatisticRow> statisticViewModels)
        {
            var values = Enum.GetValues(typeof(TankType)).Cast<int>().Where(x => x > 0);

            WinPercentByTypeDataSource = InitDataPointSource(
                values,
                statisticViewModels,
                row => row.Type,
                group => group.Any(x => x.BattlesCount > 0) ? Math.Round(group.Sum(y => y.Wins) * 100.0 / group.Sum(y => y.BattlesCount), 1) : 0,
                value => Resources.Resources.ResourceManager.GetEnumResource((TankType)value));
        }

        private void InitWinPercentByCountryChart(List<ITankStatisticRow> statisticViewModels)
        {
            var values = Enum.GetValues(typeof(Country)).Cast<int>().Where(x => x >= 0);
            WinPercentByCountryDataSource = InitDataPointSource(
                values, 
                statisticViewModels, 
                row => row.CountryId,
                group => group.Any(x => x.BattlesCount > 0) ? Math.Round(group.Sum(y => y.Wins) * 100.0 / group.Sum(y => y.BattlesCount), 1) : 0, 
                value => Resources.Resources.ResourceManager.GetEnumResource((Country)value));
        }

        private void InitBattlesByTierChart(List<ITankStatisticRow> statisticViewModels)
        {
            var values = Enumerable.Range(1, 10);
           
            BattlesByTierDataSource = InitDataPointSource(
                values,
                statisticViewModels,
                row => (int) row.Tier,
                group => group.Sum(y => y.BattlesCount),
                value => value.ToString());
        }

        private void InitBattlesByTypeChart(List<ITankStatisticRow> statisticViewModels)
        {
            var values = Enum.GetValues(typeof (TankType)).Cast<int>().Where(x => x > 0);
            
            BattlesByTypeDataSource = InitDataPointSource(
                values,
                statisticViewModels,
                row => row.Type,
                group => group.Sum(y => y.BattlesCount),
                value => Resources.Resources.ResourceManager.GetEnumResource((TankType) value));
        }

        private void InitBattlesByCountryChart(List<ITankStatisticRow> statisticViewModels)
        {
            var values = Enum.GetValues(typeof (Country)).Cast<int>().Where(x => x >= 0);
            
            BattlesByCountryDataSource = InitDataPointSource(
                values,
                statisticViewModels,
                row => row.CountryId,
                group => group.Sum(y => y.BattlesCount),
                value => Resources.Resources.ResourceManager.GetEnumResource((Country) value));
        }

        private List<IDataPoint> InitDataPointSource(IEnumerable<int> baseXValues, List<ITankStatisticRow> statisticViewModels,
            Func<ITankStatisticRow, int> groupKeySelector, Func<IGrouping<int, ITankStatisticRow>, double> calc,
            Func<int, string> localizeSelector)
        {
            List<LocalizedGenericPoint<double, double>> list = new List<LocalizedGenericPoint<double, double>>();

            if (statisticViewModels != null)
            {
                IEnumerable<LocalizedGenericPoint<double, double>> dataSource = statisticViewModels.GroupBy(
                    groupKeySelector)
                    .Select(
                        x =>
                            new LocalizedGenericPoint<double, double>(x.Key, x.Any() ? calc(x) : 0,
                                localizeSelector(x.Key)));

                list = dataSource.ToList();
            }

            var query = from value in baseXValues
                        join point in list on value equals point.X into gj
                        from subpet in gj.DefaultIfEmpty()
                        select subpet ?? new LocalizedGenericPoint<double, double>(value, 0, localizeSelector(value));

            return query.Cast<IDataPoint>().ToList();
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
