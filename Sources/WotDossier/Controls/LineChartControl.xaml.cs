using System.Collections;
using System.Windows;
using System.Windows.Controls;

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

        #endregion public string Header

        #endregion public type DataSource

        public LineChartControl()
        {
            InitializeComponent();
        }
    }
}
