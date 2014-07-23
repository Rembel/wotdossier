using System.Windows.Controls;
using WotDossier.UI;

namespace WotDossier.Tabs.Charts
{
    /// <summary>
    ///     Interaction logic for EffectivityChartsTab.xaml
    /// </summary>
    public partial class EffectivityChartsTab : UserControl
    {
        public EffectivityChartsTab()
        {
            InitializeComponent();

            ChartHelper.ConfigureChart(EByTier);
            ChartHelper.ConfigureChart(EByType);
            ChartHelper.ConfigureChart(EByCountry);
        }
    }
}