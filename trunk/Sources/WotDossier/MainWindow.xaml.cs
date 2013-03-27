using System.Windows;
using Microsoft.Research.DynamicDataDisplay;
using WotDossier.Applications.View;
using WotDossier.Applications.ViewModel;
using WotDossier.Applications.ViewModel.Rows;
using WotDossier.Framework.EventAggregator;

namespace WotDossier
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IShellView
    {
        public MainWindow()
        {
            InitializeComponent();
            EventAggregatorFactory.EventAggregator.GetEvent<OpenTankStatisticEvent>().Subscribe(OnOpenTankStatistic);
        }

        
        private void OnOpenTankStatistic(TankStatisticRowViewModel obj)
        {
            TankStatisticViewModel tankStatisticViewModel = new TankStatisticViewModel(new TankStatisticWindow(), obj);
            tankStatisticViewModel.Show();

        }

        private void MenuItemSettings_Click(object sender, RoutedEventArgs e)
        {
            SettingsViewModel model = new SettingsViewModel(new Settings());
            model.Show();
        }

        public ChartPlotter ChartRating
        {
            get { return Chart; }
        }

        public ChartPlotter ChartWinPercent
        {
            get { return Chart2; }
        }

        public ChartPlotter ChartAvgDamage
        {
            get { return Chart3; }
        }

        private void MenuItemAbout_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow aboutWindow = new AboutWindow();
            aboutWindow.Owner = this;
            aboutWindow.ShowDialog();
        }
    }
}
