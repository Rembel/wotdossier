using System.Collections;
using System.Windows;
using System.Windows.Controls;
using OxyPlot;

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
        }
    }
}
