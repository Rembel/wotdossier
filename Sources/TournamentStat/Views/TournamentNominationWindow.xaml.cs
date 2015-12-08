using System.Windows;

namespace TournamentStat.Views
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class TournamentNominationWindow : Window, Applications.View.ITournamentNominationView
    {
        public TournamentNominationWindow()
        {
            InitializeComponent();
            Owner = Application.Current.MainWindow;
        }
    }
}
