using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Microsoft.Research.DynamicDataDisplay;
using Microsoft.Research.DynamicDataDisplay.Charts;
using Microsoft.Research.DynamicDataDisplay.Charts.Axes.GenericLocational;
using WotDossier.Applications.View;
using WotDossier.Dal;
using WotDossier.Domain;

namespace WotDossier.ReplaysManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IShellView
    {
        public MainWindow()
        {
            InitializeComponent();

            ConfigureChart(BattlesCountByMap);
            ConfigureChart(BattlesWinPercentByMap);

            BattlesCountByMap.Children.Add(GetMapChartVerticalAxis());
            BattlesCountByMap.MaxY = Dictionaries.Instance.Maps.Count + 1;
            BattlesWinPercentByMap.Children.Add(GetMapChartVerticalAxis());
            BattlesWinPercentByMap.MaxY = Dictionaries.Instance.Maps.Count + 1;

            // Enable "minimize to tray" behavior for this Window
            MinimizeToTray.Enable(this);
        }

        private void ConfigureChart(ChartPlotter chart)
        {
            chart.Children.Remove(chart.DefaultContextMenu);
            chart.Children.Remove(chart.MouseNavigation);
            chart.Children.Remove(chart.KeyboardNavigation);
        }

        private static VerticalAxis GetMapChartVerticalAxis()
        {
            VerticalAxis axis = new VerticalAxis();

            List<Map> list = Dictionaries.Instance.Maps.Values.ToList();

            GenericLocationalLabelProvider<Map, double> labelProvider = new GenericLocationalLabelProvider<Map, double>(list, city => city.localizedmapname);
            GenericLocationalTicksProvider<Map, double> ticksProvider = new GenericLocationalTicksProvider<Map, double>(list, city => city.mapid);

            axis.LabelProvider = labelProvider;
            axis.TicksProvider = ticksProvider;
            return axis;
        }
    }
}
