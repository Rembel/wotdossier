using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using OxyPlot;
using OxyPlot.Wpf;
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

        public IPlotController Controller { get; set; }

        #endregion public string Header

        #endregion public type DataSource

        public LineChartControl()
        {
            InitializeComponent();
            Controller = new PlotController();
            // add a tracker command to the mouse enter event
            Controller.BindMouseEnter(PlotCommands.HoverPointsOnlyTrack);

            //double startx = double.NaN;

            //// Create a command that adds points to the scatter series
            //var command = new DelegatePlotCommand<OxyMouseDownEventArgs>(
            //    (v, c, a) =>
            //    {
            //        startx = range.InternalAnnotation.InverseTransform(a.Position).X;
            //        range.MinimumX = startx;
            //        range.MaximumX = startx + 100;
            //        Plot.InvalidatePlot(true);
            //        a.Handled = true;
            //    });

            //Controller.BindMouseDown(OxyMouseButton.Left, command);

            //Controller.AddMouseManipulator(new );

            //command = new DelegatePlotCommand<OxyMouseDownEventArgs>(
            //    (v, c, a) =>
            //    {
            //        startx = range.InternalAnnotation.InverseTransform(a.Position).X;
            //        range.MinimumX = startx;
            //        range.MaximumX = startx;
            //        Plot.InvalidatePlot(true);
            //        a.Handled = true;
            //    });

            //Controller.BindMouseMove(OxyMouseButton.Left, command);
            var plotCommand = new DelegatePlotCommand<OxyMouseDownEventArgs>((view, controller, args) => controller.AddMouseManipulator(view, new CustomMouseManipulator(view, range), args));
            Controller.BindMouseDown(OxyMouseButton.Left, plotCommand);
        }
    }

    public class CustomMouseManipulator : MouseManipulator
    {
        private readonly RectangleAnnotation _range;
        private double startx;
        private Series currentSeries;
        private TrackerHitResult startHitResult;

        public CustomMouseManipulator(IPlotView plotView, RectangleAnnotation range) : base(plotView)
        {
            _range = range;
        }

        public override void Started(OxyMouseEventArgs e)
        {
            startHitResult = GetPoint(e);
            startx = _range.InternalAnnotation.InverseTransform(e.Position).X;
            _range.MinimumX = startx;
            _range.MaximumX = startx;
            PlotView.InvalidatePlot(true);
            e.Handled = true;
            base.Started(e);
        }

        public override void Completed(OxyMouseEventArgs e)
        {
            //if (!double.IsNaN(startx))
            //{
            //    var x = _range.InternalAnnotation.InverseTransform(e.Position).X;
            //    _range.MinimumX = Math.Min(x, startx);
            //    _range.MaximumX = Math.Max(x, startx);
            //    _range.Text = string.Format("∫ cos(x) dx =  {0:0.00}", Math.Sin(_range.MaximumX) - Math.Sin(_range.MinimumX));
            //    //PlotView.Subtitle = string.Format("Integrating from {0:0.00} to {1:0.00}", range.MinimumX, range.MaximumX);
            //    PlotView.InvalidatePlot(true);
            //    e.Handled = true;
            //}
            startx = double.NaN;
            base.Completed(e);
        }

        public override void Delta(OxyMouseEventArgs e)
        {
            e.Handled = true;

            var trackerHitResult = GetPoint(e);
            if (trackerHitResult == null || startHitResult == null)
            {
                return;
            }
            
            if (!double.IsNaN(startx))
            {
                var x = _range.InternalAnnotation.InverseTransform(e.Position).X;
                _range.MinimumX = Math.Min(x, startx);
                _range.MaximumX = Math.Max(x, startx);
                if (trackerHitResult.DataPoint.X - startHitResult.DataPoint.X > 0)
                {
                    _range.Text =
                        string.Format(
                            trackerHitResult.XAxis.Title + " =  {0:0.00} \n" + PlotView.ActualModel.Title +
                            " =  {1:0.00}", trackerHitResult.DataPoint.X - startHitResult.DataPoint.X,
                            trackerHitResult.DataPoint.Y - startHitResult.DataPoint.Y);
                }
                else
                {
                    _range.Text = string.Empty;
                }
                PlotView.InvalidatePlot(true);
            }
            base.Delta(e);
        }

        private TrackerHitResult GetPoint(OxyMouseEventArgs e)
        {
            if (this.currentSeries == null)
            {
                // get the nearest
                this.currentSeries = this.PlotView.ActualModel != null
                    ? this.PlotView.ActualModel.GetSeriesFromPoint(e.Position)
                    : null;
            }

            var actualModel = this.PlotView.ActualModel;
            if (actualModel == null || currentSeries == null)
            {
                return null;
            }

            if (!actualModel.PlotArea.Contains(e.Position.X, e.Position.Y))
            {
                return null;
            }

            return currentSeries.GetNearestPoint(e.Position, false);
        }
    }
}
