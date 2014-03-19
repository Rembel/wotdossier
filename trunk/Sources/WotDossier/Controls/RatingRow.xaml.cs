using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WotDossier.Controls
{
    /// <summary>
    /// Interaction logic for RatingRow.xaml
    /// </summary>
    public partial class RatingRow : UserControl
    {
        public static readonly DependencyProperty ImageProperty =
            DependencyProperty.Register("Image", typeof (ImageSource), typeof (RatingRow), new PropertyMetadata(default(ImageSource)));

        public ImageSource Image
        {
            get { return (ImageSource) GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }

        public static readonly DependencyProperty ShortNameProperty =
            DependencyProperty.Register("ShortName", typeof (string), typeof (RatingRow), new PropertyMetadata(default(string)));

        public string ShortName
        {
            get { return (string) GetValue(ShortNameProperty); }
            set { SetValue(ShortNameProperty, value); }
        }

        public static readonly DependencyProperty FullNameProperty =
            DependencyProperty.Register("FullName", typeof (string), typeof (RatingRow), new PropertyMetadata(default(string)));

        public string FullName
        {
            get { return (string) GetValue(FullNameProperty); }
            set { SetValue(FullNameProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof (string), typeof (RatingRow), new PropertyMetadata(default(string)));

        public string Value
        {
            get { return (string) GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty PlaceProperty =
            DependencyProperty.Register("Place", typeof (string), typeof (RatingRow), new PropertyMetadata(default(string)));

        public string Place
        {
            get { return (string) GetValue(PlaceProperty); }
            set { SetValue(PlaceProperty, value); }
        }

        public static readonly DependencyProperty ValueDeltaProperty =
            DependencyProperty.Register("ValueDelta", typeof(string), typeof(RatingRow), new PropertyMetadata(default(string)));

        public string ValueDelta
        {
            get { return (string)GetValue(ValueDeltaProperty); }
            set { SetValue(ValueDeltaProperty, value); }
        }

        public static readonly DependencyProperty PlaceDeltaProperty =
            DependencyProperty.Register("PlaceDelta", typeof(int), typeof(RatingRow), new PropertyMetadata(default(int)));

        public int PlaceDelta
        {
            get { return (int)GetValue(PlaceDeltaProperty); }
            set { SetValue(PlaceDeltaProperty, value); }
        }

        public static readonly DependencyProperty ValueDeltaForegroundProperty =
            DependencyProperty.Register("ValueDeltaForeground", typeof (Brush), typeof (RatingRow), new PropertyMetadata(default(Brush)));

        public Brush ValueDeltaForeground
        {
            get { return (Brush) GetValue(ValueDeltaForegroundProperty); }
            set { SetValue(ValueDeltaForegroundProperty, value); }
        }

        public static readonly DependencyProperty PlaceDeltaForegroundProperty =
            DependencyProperty.Register("PlaceDeltaForeground", typeof (Brush), typeof (RatingRow), new PropertyMetadata(default(Brush)));

        public Brush PlaceDeltaForeground
        {
            get { return (Brush) GetValue(PlaceDeltaForegroundProperty); }
            set { SetValue(PlaceDeltaForegroundProperty, value); }
        }

        public RatingRow()
        {
            InitializeComponent();
        }
    }
}
