using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Research.DynamicDataDisplay;
using Microsoft.Research.DynamicDataDisplay.Charts;
using Microsoft.Research.DynamicDataDisplay.Charts.Axes;
using Microsoft.Research.DynamicDataDisplay.Charts.Axes.GenericLocational;
using WotDossier.Applications.View;
using WotDossier.Common;
using WotDossier.Dal;
using WotDossier.Domain;
using WotDossier.Framework;

namespace WotDossier.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [Export(typeof(IShellView))]
    public partial class MainWindow : Window, IShellView
    {
        public MainWindow()
        {
            InitializeComponent();

            ConfigureChart(EByTier);
            ConfigureChart(EByType);
            ConfigureChart(EByCountry);
            ConfigureChart(battlesByTier);
            ConfigureChart(battlesByType);
            ConfigureChart(battlesByCountry);
            ConfigureChart(BattlesCountByMap);
            ConfigureChart(BattlesWinPercentByMap);

            BattlesCountByMap.Children.Add(GetMapChartVerticalAxis());
            BattlesCountByMap.MaxY = Dictionaries.Instance.Maps.Count + 1;
            BattlesWinPercentByMap.Children.Add(GetMapChartVerticalAxis());
            BattlesWinPercentByMap.MaxY = Dictionaries.Instance.Maps.Count + 1;

            // Enable "minimize to tray" behavior for this Window
            MinimizeToTray.Enable(this);
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

        private void MenuItemAbout_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow aboutWindow = new AboutWindow();
            aboutWindow.Owner = this;
            aboutWindow.ShowDialog();
        }

        private void PrintWindow_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            int i = 0;

            string fileName = string.Format(WotDossierSettings.FILE_NAME_FORMAT, i);

            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), fileName);

            while (File.Exists(path))
            {
                i++;
                fileName = string.Format(WotDossierSettings.FILE_NAME_FORMAT, i);
                path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), fileName);
            }

            Util.SaveWindow(this,96, path);
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
            return Resources.Resources.ResourceManager.GetEnumResource((TankType) type);
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
}