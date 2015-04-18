using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using WotDossier.Applications.ViewModel.Statistic;
using WotDossier.Framework.Interpolation;

namespace WotDossier.Applications.ViewModel.Chart
{
    public class CommonChartsViewModel : INotifyPropertyChanged
    {
        private List<DateDataPoint> _ratingDataSource;
        private List<DateDataPoint> _wnRatingDataSource;
        private List<DateDataPoint> _winPercentDataSource;
        private List<DateDataPoint> _avgDamageDataSource;
        private List<DateDataPoint> _avgXpDataSource;
        private List<DateDataPoint> _killDeathRatioDataSource;
        private List<DateDataPoint> _survivePercentDataSource;
        private List<DateDataPoint> _avgSpottedDataSource;

        /// <summary>
        /// Gets or sets the rating data source.
        /// </summary>
        /// <value>
        /// The rating data source.
        /// </value>
        public List<DateDataPoint> RatingDataSource
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
        public List<DateDataPoint> WnRatingDataSource
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
        public List<DateDataPoint> WinPercentDataSource
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
        public List<DateDataPoint> AvgDamageDataSource
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
        public List<DateDataPoint> AvgXPDataSource
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
        public List<DateDataPoint> AvgSpottedDataSource
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
        public List<DateDataPoint> KillDeathRatioDataSource
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
        public List<DateDataPoint> SurvivePercentDataSource
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
            List<StatisticViewModelBase> statisticViewModels = statistic.GetAllSlices<StatisticViewModelBase>();
            RatingDataSource = GetDataSource1(statisticViewModels, x => x.EffRating, Resources.Resources.ChartTooltipFormat_Rating);
            WnRatingDataSource = GetDataSource1(statisticViewModels, x => x.WN8Rating, Resources.Resources.ChartTooltipFormat_Rating);
            WinPercentDataSource = GetDataSource1(statisticViewModels, x => x.WinsPercent, Resources.Resources.ChartTooltipFormat_WinPercent);
            AvgDamageDataSource = GetDataSource1(statisticViewModels, x => x.AvgDamageDealt, Resources.Resources.ChartTooltipFormat_AvgDamage);
            AvgXPDataSource = GetDataSource1(statisticViewModels, x => x.AvgXp, Resources.Resources.Chart_Tooltip_AvgXp);
            AvgSpottedDataSource = GetDataSource1(statisticViewModels, x => x.AvgSpotted, Resources.Resources.Chart_Tooltip_AvgSpotted);
            KillDeathRatioDataSource = GetDataSource1(statisticViewModels, x => x.KillDeathRatio, Resources.Resources.Chart_Tooltip_KillDeathRatio);
            SurvivePercentDataSource = GetDataSource1(statisticViewModels, x => x.SurvivedBattlesPercent, Resources.Resources.Chart_Tooltip_Survive);
        }

        protected static List<DateDataPoint> InterpolatePoints(List<DateDataPoint> erPoints)
        {
            List<DateDataPoint> dataPoints;

            if (erPoints.Count() > 10)
            {
                int step = (int) (erPoints.Count()*0.15);

                dataPoints = erPoints.Where((x, i) => i % step == 0).ToList();
            }
            else
            {
                return erPoints;
            }

            DateDataPoint first = erPoints.First();
            DateDataPoint last = erPoints.Last();

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
            return erPoints.Select(x => new DateDataPoint(x.X, interpolation.Interpolate(x.X), x.Date)).OrderBy(x => x.X).ToList();
        }

        private List<DateDataPoint> GetDataSource1(List<StatisticViewModelBase> statisticViewModels,
            Func<StatisticViewModelBase, double> predicate, string tooltip)
        {
            List<DateDataPoint> erPoints = statisticViewModels.Select(x => new DateDataPoint(x.BattlesCount, predicate(x), x.Updated)).Where(x => x.X > 0 & x.Y > 0).ToList();
            return InterpolatePoints(erPoints);
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