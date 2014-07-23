using System.Windows.Controls;
using WotDossier.Dal;
using WotDossier.UI;

namespace WotDossier.Tabs
{
    /// <summary>
    /// Interaction logic for Tab.xaml
    /// </summary>
    public partial class ChartsTab : UserControl
    {
        public ChartsTab()
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
