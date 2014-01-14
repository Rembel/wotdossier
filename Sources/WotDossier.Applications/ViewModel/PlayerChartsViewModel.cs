using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Microsoft.Research.DynamicDataDisplay;
using Microsoft.Research.DynamicDataDisplay.DataSources;
using Microsoft.Research.DynamicDataDisplay.PointMarkers;
using WotDossier.Applications.ViewModel.Replay;
using WotDossier.Applications.ViewModel.Rows;
using WotDossier.Common;

namespace WotDossier.Applications.ViewModel
{
    public class PlayerChartsViewModel : INotifyPropertyChanged
    {
        private List<SellInfo> _lastUsedTanksDataSource;

        private EnumerableDataSource<DataPoint> _ratingDataSource;
        private EnumerableDataSource<DataPoint> _wn6RatingDataSource;
        private EnumerableDataSource<DataPoint> _winPercentDataSource;
        private EnumerableDataSource<DataPoint> _avgDamageDataSource;
        private EnumerableDataSource<DataPoint> _avgXpDataSource;
        private EnumerableDataSource<DataPoint> _killDeathRatioDataSource;
        private EnumerableDataSource<DataPoint> _survivePercentDataSource;
        private List<DataPoint> _efficiencyByTierDataSource;
        private List<GenericPoint<string, double>> _efficiencyByTypeDataSource;
        private List<GenericPoint<string, double>> _efficiencyByCountryDataSource;

        private List<DataPoint> _battlesByTierDataSource;
        private List<GenericPoint<string, double>> _battlesByTypeDataSource;
        private List<GenericPoint<string, double>> _battlesByCountryDataSource;

        private EnumerableDataSource<DataPoint> _avgSpottedDataSource;
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

        public bool Resp1
        {
            get { return _resp1; }
            set
            {
                _resp1 = value;
                InitWinReplaysPercentByMapChart();
                InitBattlesByMapChart();
            }
        }

        public bool Resp2
        {
            get { return _resp2; }
            set
            {
                _resp2 = value;
                InitWinReplaysPercentByMapChart();
                InitBattlesByMapChart();
            }
        }

        public bool AllResps
        {
            get { return _allResps; }
            set
            {
                _allResps = value;
                InitWinReplaysPercentByMapChart();
                InitBattlesByMapChart();
            }
        }

        public List<SellInfo> LastUsedTanksDataSource
        {
            get { return _lastUsedTanksDataSource; }
            set
            {
                _lastUsedTanksDataSource = value;
                RaisePropertyChanged("LastUsedTanksDataSource");
            }
        }

        public EnumerableDataSource<DataPoint> RatingDataSource
        {
            get { return _ratingDataSource; }
            set
            {
                _ratingDataSource = value;
                RaisePropertyChanged("RatingDataSource");
            }
        }

        public EnumerableDataSource<DataPoint> WN6RatingDataSource
        {
            get { return _wn6RatingDataSource; }
            set
            {
                _wn6RatingDataSource = value;
                RaisePropertyChanged("WN6RatingDataSource");
            }
        }

        public EnumerableDataSource<DataPoint> WinPercentDataSource
        {
            get { return _winPercentDataSource; }
            set
            {
                _winPercentDataSource = value;
                RaisePropertyChanged("WinPercentDataSource");
            }
        }

        public EnumerableDataSource<DataPoint> AvgDamageDataSource
        {
            get { return _avgDamageDataSource; }
            set
            {
                _avgDamageDataSource = value;
                RaisePropertyChanged("AvgDamageDataSource");
            }
        }

        public EnumerableDataSource<DataPoint> AvgXPDataSource
        {
            get { return _avgXpDataSource; }
            set
            {
                _avgXpDataSource = value;
                RaisePropertyChanged("AvgXPDataSource");
            }
        }

        public EnumerableDataSource<DataPoint> AvgSpottedDataSource
        {
            get { return _avgSpottedDataSource; }
            set
            {
                _avgSpottedDataSource = value;
                RaisePropertyChanged("AvgSpottedDataSource");
            }
        }

        public EnumerableDataSource<DataPoint> KillDeathRatioDataSource
        {
            get { return _killDeathRatioDataSource; }
            set
            {
                _killDeathRatioDataSource = value;
                RaisePropertyChanged("KillDeathRatioDataSource");
            }
        }

        public EnumerableDataSource<DataPoint> SurvivePercentDataSource
        {
            get { return _survivePercentDataSource; }
            set
            {
                _survivePercentDataSource = value;
                RaisePropertyChanged("SurvivePercentDataSource");
            }
        }

        public List<DataPoint> ReplaysByMapDataSource
        {
            get { return _replaysByMapDataSource; }
            set
            {
                _replaysByMapDataSource = value;
                RaisePropertyChanged("ReplaysByMapDataSource");
            }
        }

        public List<DataPoint> WinReplaysPercentByMapDataSource
        {
            get { return _winReplaysPercentByMapDataSource; }
            set
            {
                _winReplaysPercentByMapDataSource = value;
                RaisePropertyChanged("WinReplaysPercentByMapDataSource");
            }
        }

        public List<DataPoint> EfficiencyByTierDataSource
        {
            get { return _efficiencyByTierDataSource; }
            set
            {
                _efficiencyByTierDataSource = value;
                RaisePropertyChanged("EfficiencyByTierDataSource");
            }
        }

        public List<GenericPoint<string, double>> EfficiencyByTypeDataSource
        {
            get { return _efficiencyByTypeDataSource; }
            set
            {
                _efficiencyByTypeDataSource = value;
                RaisePropertyChanged("EfficiencyByTypeDataSource");
            }
        }

        public List<GenericPoint<string, double>> EfficiencyByCountryDataSource
        {
            get { return _efficiencyByCountryDataSource; }
            set
            {
                _efficiencyByCountryDataSource = value;
                RaisePropertyChanged("EfficiencyByCountryDataSource");
            }
        }

        public List<DataPoint> BattlesByTierDataSource
        {
            get { return _battlesByTierDataSource; }
            set
            {
                _battlesByTierDataSource = value;
                RaisePropertyChanged("BattlesByTierDataSource");
            }
        }

        public List<GenericPoint<string, double>> BattlesByTypeDataSource
        {
            get { return _battlesByTypeDataSource; }
            set
            {
                _battlesByTypeDataSource = value;
                RaisePropertyChanged("BattlesByTypeDataSource");
            }
        }

        public List<GenericPoint<string, double>> BattlesByCountryDataSource
        {
            get { return _battlesByCountryDataSource; }
            set
            {
                _battlesByCountryDataSource = value;
                RaisePropertyChanged("BattlesByCountryDataSource");
            }
        }

        public double MaxMapBattles
        {
            get { return _maxMapBattles; }
            set
            {
                _maxMapBattles = value;
                RaisePropertyChanged("MaxMapBattles");
            }
        }

        public double MaxWinReplayPercent
        {
            get { return _maxWinReplayPercent; }
            set
            {
                _maxWinReplayPercent = value;
                RaisePropertyChanged("MaxWinReplayPercent");
            }
        }

        public double MaxBattlesByType
        {
            get { return _maxBattlesByType; }
            set
            {
                _maxBattlesByType = value;
                RaisePropertyChanged("MaxBattlesByType");
            }
        }

        public double MaxBattlesByTier
        {
            get { return _maxBattlesByTier; }
            set
            {
                _maxBattlesByTier = value;
                RaisePropertyChanged("MaxBattlesByTier");
            }
        }

        public double MaxBattlesByCountry
        {
            get { return _maxBattlesByCountry; }
            set
            {
                _maxBattlesByCountry = value;
                RaisePropertyChanged("MaxBattlesByCountry");
            }
        }

        public IEnumerable<ReplayFile> ReplaysDataSource
        {
            get { return Filter(_replaysDataSource); }
            set { _replaysDataSource = value; }
        }

        private IEnumerable<ReplayFile> Filter(IEnumerable<ReplayFile> replaysDataSource)
        {
            return replaysDataSource.Where(x =>
                                   (Resp1 && x.Team == 1
                                    || Resp2 && x.Team == 2
                                    || AllResps)
                ).ToList();

        }

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

        private void InitEfficiencyByTierChart(List<TankStatisticRowViewModel> statisticViewModels)
        {
            IEnumerable<DataPoint> dataSource = statisticViewModels.GroupBy(x => x.Tier).Select(x => new DataPoint(x.Key, RatingHelper.CalcER(
                x.Average(y => y.AvgDamageDealt),
                x.Key,
                x.Average(y => y.AvgFrags),
                x.Average(y => y.AvgSpotted),
                x.Average(y => y.AvgCapturePoints),
                x.Average(y => y.AvgDroppedCapturePoints))));
            EfficiencyByTierDataSource = dataSource.ToList();
        }

        private void InitEfficiencyByTypeChart(List<TankStatisticRowViewModel> statisticViewModels)
        {
            IEnumerable<GenericPoint<string, double>> dataSource = statisticViewModels.GroupBy(x => x.Type).Select(x => new GenericPoint<string, double>(x.Key.ToString(), RatingHelper.CalcER(
                x.Average(y => y.AvgDamageDealt),
                x.Key,
                x.Average(y => y.AvgFrags),
                x.Average(y => y.AvgSpotted),
                x.Average(y => y.AvgCapturePoints),
                x.Average(y => y.AvgDroppedCapturePoints))));
            EfficiencyByTypeDataSource = dataSource.ToList();
        }

        private void InitEfficiencyByCountryChart(List<TankStatisticRowViewModel> statisticViewModels)
        {
            IEnumerable<GenericPoint<string, double>> dataSource = statisticViewModels.GroupBy(x => x.CountryId).Select(x => new GenericPoint<string, double>(x.Key.ToString(), RatingHelper.CalcER(
                x.Average(y => y.AvgDamageDealt),
                x.Key,
                x.Average(y => y.AvgFrags),
                x.Average(y => y.AvgSpotted),
                x.Average(y => y.AvgCapturePoints),
                x.Average(y => y.AvgDroppedCapturePoints))));
            EfficiencyByCountryDataSource = dataSource.ToList();
        }

        private void InitBattlesByTierChart(List<TankStatisticRowViewModel> statisticViewModels)
        {
            IEnumerable<DataPoint> dataSource = statisticViewModels.GroupBy(x => x.Tier).Select(x => new DataPoint(x.Key, x.Sum(y => y.BattlesCount)));
            BattlesByTierDataSource = dataSource.ToList();
            double max = dataSource.Max(x => x.Y);
            MaxBattlesByTier = max * 1.2;
        }

        private void InitBattlesByTypeChart(List<TankStatisticRowViewModel> statisticViewModels)
        {
            IEnumerable<GenericPoint<string, double>> dataSource = statisticViewModels.GroupBy(x => x.Type).Select(x => new GenericPoint<string, double>(x.Key.ToString(), x.Sum(y => y.BattlesCount)));
            BattlesByTypeDataSource = dataSource.ToList();
            double max = dataSource.Max(x => x.Y);
            MaxBattlesByType = max*1.2;
        }

        private void InitBattlesByCountryChart(List<TankStatisticRowViewModel> statisticViewModels)
        {
            IEnumerable<GenericPoint<string, double>> dataSource = statisticViewModels.GroupBy(x => x.CountryId).Select(x => new GenericPoint<string, double>(x.Key.ToString(), x.Sum(y => y.BattlesCount)));
            BattlesByCountryDataSource = dataSource.ToList();
            double max = dataSource.Max(x => x.Y);
            MaxBattlesByCountry = max * 1.2;
        }

        private void InitSurvivePercentChart(List<PlayerStatisticViewModel> statisticViewModels)
        {
            IEnumerable<DataPoint> erPoints = statisticViewModels.Select(x => new DataPoint(x.BattlesCount, x.SurvivedBattlesPercent));
            var dataSource = new EnumerableDataSource<DataPoint>(erPoints) { XMapping = x => x.X, YMapping = y => y.Y };
            dataSource.AddMapping(ShapeElementPointMarker.ToolTipTextProperty,
                                  point => String.Format(Resources.Resources.Chart_Tooltip_Survive, point.X, point.Y));
            SurvivePercentDataSource = dataSource;
        }

        private void InitKillDeathRatioChart(List<PlayerStatisticViewModel> statisticViewModels)
        {
            IEnumerable<DataPoint> erPoints = statisticViewModels.Select(x => new DataPoint(x.BattlesCount, x.KillDeathRatio));
            var dataSource = new EnumerableDataSource<DataPoint>(erPoints) { XMapping = x => x.X, YMapping = y => y.Y };
            dataSource.AddMapping(ShapeElementPointMarker.ToolTipTextProperty,
                                  point => String.Format(Resources.Resources.Chart_Tooltip_KillDeathRatio, point.X, point.Y));
            KillDeathRatioDataSource = dataSource;
        }

        private void InitAvgXPChart(List<PlayerStatisticViewModel> statisticViewModels)
        {
            IEnumerable<DataPoint> erPoints = statisticViewModels.Select(x => new DataPoint(x.BattlesCount, x.AvgXp));
            var dataSource = new EnumerableDataSource<DataPoint>(erPoints) { XMapping = x => x.X, YMapping = y => y.Y };
            dataSource.AddMapping(ShapeElementPointMarker.ToolTipTextProperty,
                                  point => String.Format(Resources.Resources.Chart_Tooltip_AvgXp, point.X, point.Y));
            AvgXPDataSource = dataSource;
        }

        private void InitAvgSpottedChart(List<PlayerStatisticViewModel> statisticViewModels)
        {
            IEnumerable<DataPoint> erPoints = statisticViewModels.Select(x => new DataPoint(x.BattlesCount, x.AvgSpotted));
            var dataSource = new EnumerableDataSource<DataPoint>(erPoints) { XMapping = x => x.X, YMapping = y => y.Y };
            dataSource.AddMapping(ShapeElementPointMarker.ToolTipTextProperty,
                                  point => String.Format(Resources.Resources.Chart_Tooltip_AvgSpotted, point.X, point.Y));
            AvgSpottedDataSource = dataSource;
        }

        private void InitRatingChart(List<PlayerStatisticViewModel> statisticViewModels)
        {
            IEnumerable<DataPoint> erPoints = statisticViewModels.Select(x => new DataPoint(x.BattlesCount, x.EffRating));
            var dataSource = new EnumerableDataSource<DataPoint>(erPoints) { XMapping = x => x.X, YMapping = y => y.Y };
            dataSource.AddMapping(ShapeElementPointMarker.ToolTipTextProperty,
                                  point => String.Format(Resources.Resources.ChartTooltipFormat_Rating, point.X, point.Y));

            RatingDataSource = dataSource;

            IEnumerable<DataPoint> wn6Points = statisticViewModels.Select(x => new DataPoint(x.BattlesCount, x.WN6Rating));
            dataSource = new EnumerableDataSource<DataPoint>(wn6Points) { XMapping = x => x.X, YMapping = y => y.Y };
            dataSource.AddMapping(ShapeElementPointMarker.ToolTipTextProperty,
                                  point => String.Format(Resources.Resources.ChartTooltipFormat_Rating, point.X, point.Y));

            WN6RatingDataSource = dataSource;
        }

        private void InitWinPercentChart(List<PlayerStatisticViewModel> statisticViewModels)
        {
            IEnumerable<DataPoint> erPoints = statisticViewModels.Select(x => new DataPoint(x.BattlesCount, x.WinsPercent));
            var dataSource = new EnumerableDataSource<DataPoint>(erPoints) { XMapping = x => x.X, YMapping = y => y.Y };
            dataSource.AddMapping(ShapeElementPointMarker.ToolTipTextProperty,
                                  point => String.Format(Resources.Resources.ChartTooltipFormat_WinPercent, point.X, point.Y));
            WinPercentDataSource = dataSource;
        }

        private void InitAvgDamageChart(List<PlayerStatisticViewModel> statisticViewModels)
        {
            IEnumerable<DataPoint> erPoints = statisticViewModels.Select(x => new DataPoint(x.BattlesCount, x.AvgDamageDealt));
            var dataSource = new EnumerableDataSource<DataPoint>(erPoints) { XMapping = x => x.X, YMapping = y => y.Y };
            dataSource.AddMapping(ShapeElementPointMarker.ToolTipTextProperty,
                                  point => String.Format(Resources.Resources.ChartTooltipFormat_AvgDamage, point.X, point.Y));
            AvgDamageDataSource = dataSource;
        }

        public void InitLastUsedTanksChart(PlayerStatisticViewModel playerStatistic, List<TankStatisticRowViewModel> tanks)
        {
            IEnumerable<TankStatisticRowViewModel> viewModels = tanks.Where(x => x.Updated > playerStatistic.PreviousDate);
            IEnumerable<SellInfo> items = viewModels.Select(x => new SellInfo { TankName = x.Tank, WinPercent = x.WinsPercentForPeriod, Battles = x.BattlesCountDelta });
            LastUsedTanksDataSource = items.ToList();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
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

        public void InitCharts(PlayerStatisticViewModel playerStatistic, List<TankStatisticRowViewModel> tanks)
        {
            List<PlayerStatisticViewModel> statisticViewModels = playerStatistic.GetAll();
            InitRatingChart(statisticViewModels);
            InitWinPercentChart(statisticViewModels);
            InitAvgDamageChart(statisticViewModels);
            InitAvgXPChart(statisticViewModels);
            InitAvgSpottedChart(statisticViewModels);
            InitKillDeathRatioChart(statisticViewModels);
            InitSurvivePercentChart(statisticViewModels);
            InitEfficiencyByTierChart(tanks);
            InitEfficiencyByTypeChart(tanks);
            InitEfficiencyByCountryChart(tanks);
            InitBattlesByTierChart(tanks);
            InitBattlesByTypeChart(tanks);
            InitBattlesByCountryChart(tanks);
            InitLastUsedTanksChart(playerStatistic, tanks);
        }
    }
}
