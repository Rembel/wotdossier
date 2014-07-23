using System.Windows.Controls;
using WotDossier.UI;

namespace WotDossier.Tabs.Charts
{
    /// <summary>
    ///     Interaction logic for BattlesCountChartsTab.xaml
    /// </summary>
    public partial class BattlesCountChartsTab : UserControl
    {
        public BattlesCountChartsTab()
        {
            InitializeComponent();

            ChartHelper.ConfigureChart(battlesByTier);
            ChartHelper.ConfigureChart(battlesByType);
            ChartHelper.ConfigureChart(battlesByCountry);
        }
    }
}