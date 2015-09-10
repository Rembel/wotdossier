using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using OxyPlot;
using OxyPlot.Wpf;
using WotDossier.Converters.Color;
using PlotCommands = OxyPlot.PlotCommands;
using Series = OxyPlot.Series.Series;

namespace WotDossier.Controls
{
    /// <summary>
    /// Interaction logic for LineChartControl.xaml
    /// </summary>
    public partial class LineChartControl : UserControl
    {
        #region public type DataSource

        /// <summary>
        /// Identifies the DataSource dependency property.
        /// </summary>
        public static DependencyProperty DataSourceProperty =
            DependencyProperty.Register("DataSource", typeof(IEnumerable), typeof(LineChartControl), null);

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable DataSource
        {
            get { return (IEnumerable)GetValue(DataSourceProperty); }

            set { SetValue(DataSourceProperty, value); }
        }

        #region public string Header

        /// <summary>
        /// Identifies the Header dependency property.
        /// </summary>
        public static DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(string), typeof(LineChartControl), null);

        /// <summary>
        /// 
        /// </summary>
        public string Header
        {
            get { return (string) GetValue(HeaderProperty); }

            set { SetValue(HeaderProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="P:OxyPlot.Wpf.Series.TrackerFormatString"/> dependency property.
        /// 
        /// </summary>
        public static readonly DependencyProperty TrackerFormatStringProperty = DependencyProperty.Register("TrackerFormatString", typeof(string), typeof(LineChartControl), new PropertyMetadata((object)null));

        /// <summary>
        /// Gets or sets TrackerFormatString.
        /// 
        /// </summary>
        public string TrackerFormatString
        {
            get
            {
                return (string)GetValue(TrackerFormatStringProperty);
            }
            set
            {
                SetValue(TrackerFormatStringProperty, (object)value);
            }
        }

        #endregion public string Header

        #endregion public type DataSource

        public LineChartControl()
        {
            InitializeComponent();
            // add a tracker command to the mouse enter event
            Plot.ActualController.BindMouseEnter(PlotCommands.HoverPointsOnlyTrack);
            var plotCommand = new DelegatePlotCommand<OxyMouseDownEventArgs>((view, controller, args) => controller.AddMouseManipulator(view, new CustomMouseManipulator(view, range), args));
            Plot.ActualController.BindMouseDown(OxyMouseButton.Left, plotCommand);
        }
    }

    public class CustomMouseManipulator : MouseManipulator
    {
        private readonly RectangleAnnotation _range;
        private double _startx;
        private Series _currentSeries;
        private TrackerHitResult _startHitResult;

        public CustomMouseManipulator(IPlotView plotView, RectangleAnnotation range) : base(plotView)
        {
            _range = range;
        }

        public override void Started(OxyMouseEventArgs e)
        {
            _startHitResult = GetPoint(e);
            _startx = _range.InternalAnnotation.InverseTransform(e.Position).X;
            _range.MinimumX = _startx;
            _range.MaximumX = _startx;
            PlotView.InvalidatePlot();
            e.Handled = true;
            base.Started(e);
        }

        public override void Completed(OxyMouseEventArgs e)
        {
            _startx = double.NaN;
            base.Completed(e);
        }

        public override void Delta(OxyMouseEventArgs e)
        {
            e.Handled = true;

            var trackerHitResult = GetPoint(e);
            if (trackerHitResult == null || _startHitResult == null)
            {
                return;
            }
            
            if (!double.IsNaN(_startx))
            {
                var x = _range.InternalAnnotation.InverseTransform(e.Position).X;
                _range.MinimumX = Math.Min(x, _startx);
                _range.MaximumX = Math.Max(x, _startx);
                var xDelta = trackerHitResult.DataPoint.X - _startHitResult.DataPoint.X;
                if (xDelta > 0)
                {
                    var yDelta = trackerHitResult.DataPoint.Y - _startHitResult.DataPoint.Y;
                    _range.Text = string.Format("{2} =  {0:+#,0;-#,0;0} \n{3} =  {1:+#,0.00;-#,0.00;0}", xDelta, yDelta, trackerHitResult.XAxis.Title, PlotView.ActualModel.Title);
                    _range.TextColor = ((SolidColorBrush) DeltaToColorConverter.Default.Convert(yDelta, null, null, null)).Color;
                }
                else
                {
                    _range.Text = string.Empty;
                }
                PlotView.InvalidatePlot();
            }
            base.Delta(e);
        }

        private TrackerHitResult GetPoint(OxyMouseEventArgs e)
        {
            if (_currentSeries == null)
            {
                // get the nearest
                _currentSeries = PlotView.ActualModel != null
                    ? PlotView.ActualModel.GetSeriesFromPoint(e.Position)
                    : null;
            }

            var actualModel = PlotView.ActualModel;
            if (actualModel == null || _currentSeries == null)
            {
                return null;
            }

            if (!actualModel.PlotArea.Contains(e.Position.X, e.Position.Y))
            {
                return null;
            }

            return _currentSeries.GetNearestPoint(e.Position, false);
        }
    }
}
