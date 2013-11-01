using System.Windows;
using System.Windows.Controls;

namespace WotDossier.Controls
{
    /// <summary>
    /// Interaction logic for CompareRow.xaml
    /// </summary>
    public partial class AchievementCompareRow : UserControl
    {
        public static readonly DependencyProperty AchievementIconSourceProperty =
            DependencyProperty.Register("AchievementIconSource", typeof (string), typeof (AchievementCompareRow), new PropertyMetadata(default(string)));

        public string AchievementIconSource
        {
            get { return (string) GetValue(AchievementIconSourceProperty); }
            set { SetValue(AchievementIconSourceProperty, value); }
        }

        public static readonly DependencyProperty FirstValueProperty =
            DependencyProperty.Register("FirstValue", typeof(int), typeof(AchievementCompareRow), new PropertyMetadata(default(int)));

        public int FirstValue
        {
            get { return (int)GetValue(FirstValueProperty); }
            set { SetValue(FirstValueProperty, value); }
        }

        public static readonly DependencyProperty SecondValueProperty =
            DependencyProperty.Register("SecondValue", typeof(int), typeof(AchievementCompareRow), new PropertyMetadata(default(int)));

        public int SecondValue
        {
            get { return (int)GetValue(SecondValueProperty); }
            set { SetValue(SecondValueProperty, value); }
        }

        public AchievementCompareRow()
        {
            InitializeComponent();
        }
    }
}
