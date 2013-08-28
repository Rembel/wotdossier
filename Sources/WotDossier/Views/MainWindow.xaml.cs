using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Research.DynamicDataDisplay.Charts;
using Microsoft.Research.DynamicDataDisplay.Charts.Axes;
using WotDossier.Applications.View;
using WotDossier.Domain;

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

            // Enable "minimize to tray" behavior for this Window
            MinimizeToTray.Enable(this);
        }

        private void MenuItemAbout_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow aboutWindow = new AboutWindow();
            aboutWindow.Owner = this;
            aboutWindow.ShowDialog();
        }
    }

    public class TextLabelProvider : LabelProviderBase<double>
    {
        /// <summary>
        /// Creates labels by given ticks info.
        ///             Is not intended to be called from your code.
        /// </summary>
        /// <param name="ticksInfo">The ticks info.</param>
        /// <returns>
        /// Array of <see cref="T:System.Windows.UIElement"/>s, which are axis labels for specified axis ticks.
        /// </returns>
        public override UIElement[] CreateLabels(ITicksInfo<double> ticksInfo)
        {
            if (ticksInfo == null)
                throw new ArgumentNullException("ticks");

            return ticksInfo.Ticks.Select(num => new TextBlock
            {
                Text = GetTankTypeName((int)num)
            }).ToArray();
        }

        public string GetTankTypeName(int type)
        {
            return Resources.Resources.ResourceManager.GetString("TankType_" + ((TankType) type).ToString());
        }
    }
}
