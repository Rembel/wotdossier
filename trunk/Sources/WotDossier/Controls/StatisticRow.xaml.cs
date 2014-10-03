using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WotDossier.Controls
{
    /// <summary>
    /// Interaction logic for CompareRow.xaml
    /// </summary>
    public partial class StatisticRow : UserControl
    {
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof (string), typeof (StatisticRow), new PropertyMetadata(default(string)));

        public string Header
        {
            get { return (string) GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(string), typeof(StatisticRow), new PropertyMetadata(default(string)));

        public string Value
        {
            get { return (string)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueTooltipProperty =
            DependencyProperty.Register("ValueTooltip", typeof(string), typeof(StatisticRow), new PropertyMetadata(default(string)));

        public string ValueTooltip
        {
            get { return (string)GetValue(ValueTooltipProperty); }
            set { SetValue(ValueTooltipProperty, value); }
        }

        public static readonly DependencyProperty DeltaProperty =
            DependencyProperty.Register("Delta", typeof(string), typeof(StatisticRow), new PropertyMetadata(default(string)));

        public string Delta
        {
            get { return (string)GetValue(DeltaProperty); }
            set { SetValue(DeltaProperty, value); }
        }

        public static readonly DependencyProperty DeltaTooltipProperty =
            DependencyProperty.Register("DeltaTooltip", typeof(string), typeof(StatisticRow), new PropertyMetadata(default(string)));

        public string DeltaTooltip
        {
            get { return (string)GetValue(DeltaTooltipProperty); }
            set { SetValue(DeltaTooltipProperty, value); }
        }

        public static readonly DependencyProperty ValueForegroundProperty =
            DependencyProperty.Register("ValueForeground", typeof(Brush), typeof(StatisticRow), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(186,191,186))));

        public Brush ValueForeground
        {
            get { return (Brush)GetValue(ValueForegroundProperty); }
            set { SetValue(ValueForegroundProperty, value); }
        }

        public static readonly DependencyProperty DeltaForegroundProperty =
            DependencyProperty.Register("DeltaForeground", typeof(Brush), typeof(StatisticRow), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(186, 191, 186))));

        public Brush DeltaForeground
        {
            get { return (Brush)GetValue(DeltaForegroundProperty); }
            set { SetValue(DeltaForegroundProperty, value); }
        }

        public static readonly DependencyProperty HeaderMarginProperty =
            DependencyProperty.Register("HeaderMargin", typeof (Thickness), typeof (StatisticRow), new PropertyMetadata(default(Thickness)));

        public Thickness HeaderMargin
        {
            get { return (Thickness) GetValue(HeaderMarginProperty); }
            set { SetValue(HeaderMarginProperty, value); }
        }

        public StatisticRow()
        {
            InitializeComponent();
        }
    }
}
