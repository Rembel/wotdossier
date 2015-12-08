using System.Windows;
using TournamentStat.Applications.View;
using WotDossier;

namespace TournamentStat.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IShellView
    {
        public MainWindow()
        {
            InitializeComponent();
            // Enable "minimize to tray" behavior for this Window
            MinimizeToTray.Enable(this);
        }
    }
}
