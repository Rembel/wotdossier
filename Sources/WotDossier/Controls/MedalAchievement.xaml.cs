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

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof (string), typeof (MedalAchievement), new PropertyMetadata(default(string)));

        public string Text
        {
            get { return (string) GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public MedalAchievement()
        {
            InitializeComponent();
        }
    }
}
