using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Research.DynamicDataDisplay;
using Microsoft.Research.DynamicDataDisplay.Charts;
using Microsoft.Research.DynamicDataDisplay.Charts.Axes;
using Microsoft.Research.DynamicDataDisplay.Charts.Axes.GenericLocational;
using WotDossier.Common;
using WotDossier.Dal;
using WotDossier.Domain;

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

            ConfigureChart(EByTier);
            ConfigureChart(EByType);
            ConfigureChart(EByCountry);

            ConfigureChart(WinPercentByTier);
            ConfigureChart(WinPercentByType);
            ConfigureChart(WinPercentByCountry);

            ConfigureChart(battlesByTier);
            ConfigureChart(battlesByType);
            ConfigureChart(battlesByCountry);

            ConfigureChart(BattlesCountByMap);
            ConfigureChart(BattlesWinPercentByMap);

            BattlesCountByMap.Children.Add(GetMapChartVerticalAxis());
            BattlesCountByMap.MaxY = Dictionaries.Instance.Maps.Count + 1;
            BattlesWinPercentByMap.Children.Add(GetMapChartVerticalAxis());
            BattlesWinPercentByMap.MaxY = Dictionaries.Instance.Maps.Count + 1;
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

        private void ConfigureChart(ChartPlotter chart)
        {
            chart.Children.Remove(chart.DefaultContextMenu);
            chart.Children.Remove(chart.MouseNavigation);
            chart.Children.Remove(chart.KeyboardNavigation);
        }
    }

    public class TankTypeLabelProvider : LabelProviderBase<int>
    {
        /// <summary>
        /// Creates labels by given ticks info.
        ///             Is not intended to be called from your code.
        /// </summary>
        /// <param name="ticksInfo">The ticks info.</param>
        /// <returns>
        /// Array of <see cref="T:System.Windows.UIElement"/>s, which are axis labels for specified axis ticks.
        /// </returns>
        public override UIElement[] CreateLabels(ITicksInfo<int> ticksInfo)
        {
            if (ticksInfo == null)
                throw new ArgumentNullException("ticks");

            return ticksInfo.Ticks.Select(num => new TextBlock
            {
                Text = GetTankTypeName(num)
            }).ToArray();
        }

        public string GetTankTypeName(int type)
        {
            return Resources.Resources.ResourceManager.GetEnumResource((TankType)type);
        }
    }

    public class CountryLabelProvider : LabelProviderBase<int>
    {
        /// <summary>
        /// Creates labels by given ticks info.
        ///             Is not intended to be called from your code.
        /// </summary>
        /// <param name="ticksInfo">The ticks info.</param>
        /// <returns>
        /// Array of <see cref="T:System.Windows.UIElement"/>s, which are axis labels for specified axis ticks.
        /// </returns>
        public override UIElement[] CreateLabels(ITicksInfo<int> ticksInfo)
        {
            if (ticksInfo == null)
                throw new ArgumentNullException("ticks");

            return ticksInfo.Ticks.Select(num => new TextBlock
            {
                Text = GetCountryName(num),
                LayoutTransform = new RotateTransform(-90)
            }).ToArray();
        }

        public string GetCountryName(int type)
        {
            return Resources.Resources.ResourceManager.GetEnumResource((Country)type);
        }
    }

    public class TierLabelProvider : LabelProviderBase<int>
    {
        /// <summary>
        /// Creates labels by given ticks info.
        ///             Is not intended to be called from your code.
        /// </summary>
        /// <param name="ticksInfo">The ticks info.</param>
        /// <returns>
        /// Array of <see cref="T:System.Windows.UIElement"/>s, which are axis labels for specified axis ticks.
        /// </returns>
        public override UIElement[] CreateLabels(ITicksInfo<int> ticksInfo)
        {
            if (ticksInfo == null)
                throw new ArgumentNullException("ticks");

            return ticksInfo.Ticks.Select(num => new TextBlock { Text = GetCountryName(num) }).ToArray();
        }

        public string GetCountryName(int tier)
        {
            if (tier > 0 && tier < 11)
            {
                return tier.ToString();
            }
            return string.Empty;
        }
    }
}
