using System.Windows;
using System.Windows.Input;

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
            KeyDown += Window_KeyDown;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Close();
            }
        }
    }
}
