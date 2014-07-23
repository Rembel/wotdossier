using System.Windows.Controls;
using WotDossier.UI;

namespace WotDossier.Tabs.Charts
{
    /// <summary>
    /// Interaction logic for WinPercentChartsTab.xaml
    /// </summary>
    public partial class WinPercentChartsTab : UserControl
    {
        public WinPercentChartsTab()
        {
            InitializeComponent();

            ChartHelper.ConfigureChart(WinPercentByTier);
            ChartHelper.ConfigureChart(WinPercentByType);
            ChartHelper.ConfigureChart(WinPercentByCountry);
        }
    }
}
