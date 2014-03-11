using System.Windows;
using System.Windows.Controls;

namespace WotDossier.Controls
{
    /// <summary>
    /// Interaction logic for MedalAchievement.xaml
    /// </summary>
    public partial class MedalAchievement : UserControl
    {
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(string), typeof(MedalAchievement), new PropertyMetadata(default(string)));

        public string Source
        {
            get { return (string)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(int), typeof(MedalAchievement), new PropertyMetadata(default(int)));

        public static readonly DependencyProperty MinValueProperty =
            DependencyProperty.Register("MinValue", typeof(int), typeof(MedalAchievement), new PropertyMetadata(default(int)));

        public int Value
        {
            get { return (int)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public int MinValue
        {
            get { return (int)GetValue(MinValueProperty); }
            set { SetValue(MinValueProperty, value); }
        }

        public static readonly DependencyProperty DeltaProperty =
            DependencyProperty.Register("Delta", typeof (int), typeof (MedalAchievement), new PropertyMetadata(default(int)));

        public int Delta
        {
            get { return (int) GetValue(DeltaProperty); }
            set { SetValue(DeltaProperty, value); }
        }

        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.Register("Description", typeof (string), typeof (MedalAchievement), new PropertyMetadata(default(string)));

        public string Description
        {
            get { return (string) GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }

        public MedalAchievement()
        {
            InitializeComponent();
        }
    }
}
