using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Media;
using Microsoft.Research.DynamicDataDisplay;
using Microsoft.Research.DynamicDataDisplay.DataSources;
using Microsoft.Research.DynamicDataDisplay.PointMarkers;
using WotDossier.Applications.View;
using WotDossier.Applications.ViewModel.Rows;
using WotDossier.Framework.Applications;

namespace WotDossier.Applications.ViewModel
{
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [Export(typeof(TankStatisticViewModel))]
    public class TankStatisticViewModel : ViewModel<ITankStatisticView>
    {
        private TankStatisticRowViewModel _tankStatistic;

        public TankStatisticRowViewModel TankStatistic
        {
            get { return _tankStatistic; }
            set
            {
                _tankStatistic = value;
                RaisePropertyChanged("TankStatistic");
            }
        }

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

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModel&lt;TView&gt;"/> class and
        /// attaches itself as <c>DataContext</c> to the view.
        /// </summary>
        /// <param name="view">The view.</param>
        [ImportingConstructor]
        public TankStatisticViewModel([Import(typeof(ITankStatisticView))]ITankStatisticView view)
            : base(view)
        {
        }

        public virtual void Show()
        {
            ViewTyped.Show();
            InitChart(TankStatistic.GetAll());
        }

        private void InitChart(IEnumerable<StatisticViewModelBase> statisticViewModels)
        {
            InitRatingChart(statisticViewModels);
            InitWinPercentChart(statisticViewModels);
            InitAvgDamageChart(statisticViewModels);
        }

        private void InitRatingChart(IEnumerable<StatisticViewModelBase> statisticViewModels)
        {
            DataRect dataRect = DataRect.Create(0, 0, 100000, 2500);
            ChartRating.Viewport.Domain = dataRect;
            ChartRating.Viewport.Visible = dataRect;

            ChartRating.RemoveUserElements();

            IEnumerable<DataPoint> erPoints = statisticViewModels.Select(x => new DataPoint(x.BattlesCount, x.EffRating));
            var dataSource = new EnumerableDataSource<DataPoint>(erPoints) { XMapping = x => x.X, YMapping = y => y.Y };
            dataSource.AddMapping(ShapeElementPointMarker.ToolTipTextProperty, point => String.Format(Resources.Resources.ChartTooltipFormat_Rating, point.X, point.Y));
            SolidColorBrush brush = new SolidColorBrush { Color = Colors.Blue };
            Pen lineThickness = new Pen(brush, 2);
            ElementPointMarker circlePointMarker = new CircleElementPointMarker { Size = 7, Fill = brush, Brush = brush };
            ChartRating.AddLineGraph(dataSource, lineThickness, circlePointMarker, new PenDescription(Resources.Resources.ChartLegend_ER));

            IEnumerable<DataPoint> wn6Points = statisticViewModels.Select(x => new DataPoint(x.BattlesCount, x.WN6Rating));
            dataSource = new EnumerableDataSource<DataPoint>(wn6Points) { XMapping = x => x.X, YMapping = y => y.Y };
            dataSource.AddMapping(ShapeElementPointMarker.ToolTipTextProperty,
                                  point => String.Format(Resources.Resources.ChartTooltipFormat_Rating, point.X, point.Y));
            brush = new SolidColorBrush { Color = Colors.Green };
            lineThickness = new Pen(brush, 2);
            circlePointMarker = new CircleElementPointMarker { Size = 7, Fill = brush, Brush = brush };
            ChartRating.AddLineGraph(dataSource, lineThickness, circlePointMarker, new PenDescription(Resources.Resources.ChartLegend_WN6Rating));

            ChartRating.FitToView();
        }

        private void InitWinPercentChart(IEnumerable<StatisticViewModelBase> statisticViewModels)
        {
            ChartWinPercent.RemoveUserElements();

            IEnumerable<DataPoint> erPoints = statisticViewModels.Select(x => new DataPoint(x.BattlesCount, x.WinsPercent));
            var dataSource = new EnumerableDataSource<DataPoint>(erPoints) { XMapping = x => x.X, YMapping = y => y.Y };
            dataSource.AddMapping(ShapeElementPointMarker.ToolTipTextProperty, point => String.Format(Resources.Resources.ChartTooltipFormat_WinPercent, point.X, point.Y));
            SolidColorBrush brush = new SolidColorBrush { Color = Colors.Blue };
            Pen lineThickness = new Pen(brush, 2);
            ElementPointMarker circlePointMarker = new CircleElementPointMarker { Size = 7, Fill = brush, Brush = brush };
            ChartWinPercent.AddLineGraph(dataSource, lineThickness, circlePointMarker, new PenDescription(Resources.Resources.ChartLegend_WinPercent));

            ChartWinPercent.FitToView();
        }

        private void InitAvgDamageChart(IEnumerable<StatisticViewModelBase> statisticViewModels)
        {
            ChartAvgDamage.RemoveUserElements();

            IEnumerable<DataPoint> erPoints = statisticViewModels.Select(x => new DataPoint(x.BattlesCount, x.AvgDamageDealt));
            var dataSource = new EnumerableDataSource<DataPoint>(erPoints) { XMapping = x => x.X, YMapping = y => y.Y };
            dataSource.AddMapping(ShapeElementPointMarker.ToolTipTextProperty, point => String.Format(Resources.Resources.ChartTooltipFormat_AvgDamage, point.X, point.Y));
            SolidColorBrush brush = new SolidColorBrush { Color = Colors.Blue };
            Pen lineThickness = new Pen(brush, 2);
            ElementPointMarker circlePointMarker = new CircleElementPointMarker { Size = 7, Fill = brush, Brush = brush };
            ChartAvgDamage.AddLineGraph(dataSource, lineThickness, circlePointMarker, new PenDescription(Resources.Resources.ChartLegend_AvgDamage));

            ChartAvgDamage.FitToView();
        }
    }
}
