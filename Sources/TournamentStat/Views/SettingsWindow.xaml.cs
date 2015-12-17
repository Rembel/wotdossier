using System.Windows;

namespace TournamentStat.Views
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class SettingsWindow : Window, Applications.View.ISettingsView
    {
        public SettingsWindow()
        {
            InitializeComponent();
            Owner = Application.Current.MainWindow;
        }
    }
}
