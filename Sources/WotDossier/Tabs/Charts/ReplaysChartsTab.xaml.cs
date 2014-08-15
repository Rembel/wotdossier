using System.Windows.Controls;
using WotDossier.Dal;
using WotDossier.UI;

namespace WotDossier.Tabs.Charts
{
    /// <summary>
    ///     Interaction logic for ReplaysChartsTab.xaml
    /// </summary>
    public partial class ReplaysChartsTab : UserControl
    {
        public ReplaysChartsTab()
        {
            InitializeComponent();

            ChartHelper.ConfigureChart(BattlesCountByMap);
            ChartHelper.ConfigureChart(BattlesWinPercentByMap);

            BattlesCountByMap.Children.Add(ChartHelper.GetMapChartVerticalAxis());
            BattlesCountByMap.MaxY = Dictionaries.Instance.Maps.Count + 1;
            BattlesWinPercentByMap.Children.Add(ChartHelper.GetMapChartVerticalAxis());
            BattlesWinPercentByMap.MaxY = Dictionaries.Instance.Maps.Count + 1;
        }
    }
}