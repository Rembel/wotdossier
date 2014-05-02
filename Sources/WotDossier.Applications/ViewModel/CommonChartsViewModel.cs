using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Microsoft.Research.DynamicDataDisplay.DataSources;
using Microsoft.Research.DynamicDataDisplay.PointMarkers;
using WotDossier.Framework.Interpolation;

namespace WotDossier.Applications.ViewModel
{
    public class CommonChartsViewModel : INotifyPropertyChanged
    {
        private EnumerableDataSource<DataPoint> _ratingDataSource;
        private EnumerableDataSource<DataPoint> _wnRatingDataSource;
        private EnumerableDataSource<DataPoint> _winPercentDataSource;
        private EnumerableDataSource<DataPoint> _avgDamageDataSource;
        private EnumerableDataSource<DataPoint> _avgXpDataSource;
        private EnumerableDataSource<DataPoint> _killDeathRatioDataSource;
        private EnumerableDataSource<DataPoint> _survivePercentDataSource;
        private EnumerableDataSource<DataPoint> _avgSpottedDataSource;

        /// <summary>
        /// Gets or sets the rating data source.
        /// </summary>
        /// <value>
        /// The rating data source.
        /// </value>
        public EnumerableDataSource<DataPoint> RatingDataSource
        {
            get { return _ratingDataSource; }
            set
            {
                _ratingDataSource = value;
                RaisePropertyChanged("RatingDataSource");
            }
        }

        /// <summary>
        /// Gets or sets the W n7 rating data source.
        /// </summary>
        /// <value>
        /// The W n7 rating data source.
        /// </value>
        public EnumerableDataSource<DataPoint> WnRatingDataSource
        {
            get { return _wnRatingDataSource; }
            set
            {
                _wnRatingDataSource = value;
                RaisePropertyChanged("WnRatingDataSource");
            }
        }

        /// <summary>
        /// Gets or sets the win percent data source.
        /// </summary>
        /// <value>
        /// The win percent data source.
        /// </value>
        public EnumerableDataSource<DataPoint> WinPercentDataSource
        {
            get { return _winPercentDataSource; }
            set
            {
                _winPercentDataSource = value;
                RaisePropertyChanged("WinPercentDataSource");
            }
        }

        /// <summary>
        /// Gets or sets the avg damage data source.
        /// </summary>
        /// <value>
        /// The avg damage data source.
        /// </value>
        public EnumerableDataSource<DataPoint> AvgDamageDataSource
        {
            get { return _avgDamageDataSource; }
            set
            {
                _avgDamageDataSource = value;
                RaisePropertyChanged("AvgDamageDataSource");
            }
        }

        /// <summary>
        /// Gets or sets the avg XP data source.
        /// </summary>
        /// <value>
        /// The avg XP data source.
        /// </value>
        public EnumerableDataSource<DataPoint> AvgXPDataSource
        {
            get { return _avgXpDataSource; }
            set
            {
                _avgXpDataSource = value;
                RaisePropertyChanged("AvgXPDataSource");
            }
        }

        /// <summary>
        /// Gets or sets the avg spotted data source.
        /// </summary>
        /// <value>
        /// The avg spotted data source.
        /// </value>
        public EnumerableDataSource<DataPoint> AvgSpottedDataSource
        {
            get { return _avgSpottedDataSource; }
            set
            {
                _avgSpottedDataSource = value;
                RaisePropertyChanged("AvgSpottedDataSource");
            }
        }

        /// <summary>
        /// Gets or sets the kill death ratio data source.
        /// </summary>
        /// <value>
        /// The kill death ratio data source.
        /// </value>
        public EnumerableDataSource<DataPoint> KillDeathRatioDataSource
        {
            get { return _killDeathRatioDataSource; }
            set
            {
                _killDeathRatioDataSource = value;
                RaisePropertyChanged("KillDeathRatioDataSource");
            }
        }

        /// <summary>
        /// Gets or sets the survive percent data source.
        /// </summary>
        /// <value>
        /// The survive percent data source.
        /// </value>
        public EnumerableDataSource<DataPoint> SurvivePercentDataSource
        {
            get { return _survivePercentDataSource; }
            set
            {
                _survivePercentDataSource = value;
                RaisePropertyChanged("SurvivePercentDataSource");
            }
        }

        /// <summary>
        /// Inits the charts.
        /// </summary>
        /// <param name="statistic">The statistic.</param>
        public void InitCharts(StatisticViewModelBase statistic)
        {
            List<StatisticViewModelBase> statisticViewModels = statistic.GetAllSlices();
            RatingDataSource = GetDataSource(statisticViewModels, x => x.EffRating, Resources.Resources.ChartTooltipFormat_Rating);
            WnRatingDataSource = GetDataSource(statisticViewModels, x => x.WN8Rating, Resources.Resources.ChartTooltipFormat_Rating);
            WinPercentDataSource = GetDataSource(statisticViewModels, x => x.WinsPercent, Resources.Resources.ChartTooltipFormat_WinPercent);
            AvgDamageDataSource = GetDataSource(statisticViewModels, x => x.AvgDamageDealt, Resources.Resources.ChartTooltipFormat_AvgDamage);
            AvgXPDataSource = GetDataSource(statisticViewModels, x => x.AvgXp, Resources.Resources.Chart_Tooltip_AvgXp);
            AvgSpottedDataSource = GetDataSource(statisticViewModels, x => x.AvgSpotted, Resources.Resources.Chart_Tooltip_AvgSpotted);
            KillDeathRatioDataSource = GetDataSource(statisticViewModels, x => x.KillDeathRatio, Resources.Resources.Chart_Tooltip_KillDeathRatio);
            SurvivePercentDataSource = GetDataSource(statisticViewModels, x => x.SurvivedBattlesPercent, Resources.Resources.Chart_Tooltip_Survive);
        }

        protected static IEnumerable<DataPoint> InterpolatePoints(List<DataPoint> erPoints)
        {
            List<DataPoint> dataPoints;

            if (erPoints.Count > 10)
            {
                int step = (int) (erPoints.Count*0.15);

                dataPoints = erPoints.Where((x, i) => i % step == 0).ToList();
            }
            else
            {
                return erPoints;
            }

            DataPoint first = erPoints.First();
            DataPoint last = erPoints.Last();

            if (!dataPoints.Contains(first))
            {
                dataPoints.Insert(0, first);
            }

            if (!dataPoints.Contains(last))
            {
                dataPoints.Add(last);
            }

            AkimaSplineInterpolation interpolation = new AkimaSplineInterpolation(dataPoints.Select(x => x.X).ToList(),
                dataPoints.Select(x => x.Y).ToList());
            return erPoints.Select(x => new DataPoint(x.X, interpolation.Interpolate(x.X))).OrderBy(x => x.X).ToList();
        }

        private EnumerableDataSource<DataPoint> GetDataSource(List<StatisticViewModelBase> statisticViewModels,
            Func<StatisticViewModelBase, double> predicate, string tooltip)
        {
            List<DataPoint> erPoints = statisticViewModels.Select(x => new DataPoint(x.BattlesCount, predicate(x))).Where(x => x.X > 0 & x.Y > 0).ToList();
            var dataSource = new EnumerableDataSource<DataPoint>(InterpolatePoints(erPoints)) { XMapping = x => x.X, YMapping = y => y.Y };
            dataSource.AddMapping(ShapeElementPointMarker.ToolTipTextProperty, point => String.Format(tooltip, point.X, point.Y));
            return dataSource;
        }

        /// <summary>
        /// Occurs when [property changed].
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the property changed.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}