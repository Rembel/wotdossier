using System.Collections.Generic;
using System.Linq;
using Microsoft.Research.DynamicDataDisplay;
using Microsoft.Research.DynamicDataDisplay.Charts;
using Microsoft.Research.DynamicDataDisplay.Charts.Axes.GenericLocational;
using WotDossier.Dal;
using WotDossier.Domain;

namespace WotDossier.UI
{
    public class ChartHelper
    {
        public static VerticalAxis GetMapChartVerticalAxis()
        {
            VerticalAxis axis = new VerticalAxis();

            List<Map> list = Dictionaries.Instance.Maps.Values.ToList();

            GenericLocationalLabelProvider<Map, double> labelProvider = new GenericLocationalLabelProvider<Map, double>(list, city => city.LocalizedMapName);
            GenericLocationalTicksProvider<Map, double> ticksProvider = new GenericLocationalTicksProvider<Map, double>(list, city => city.MapId);

            axis.LabelProvider = labelProvider;
            axis.TicksProvider = ticksProvider;
            return axis;
        }

        public static void ConfigureChart(ChartPlotter chart)
        {
            chart.Children.Remove(chart.DefaultContextMenu);
            chart.Children.Remove(chart.MouseNavigation);
            chart.Children.Remove(chart.KeyboardNavigation);
        }
    }
}
