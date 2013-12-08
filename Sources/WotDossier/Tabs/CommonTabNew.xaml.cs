using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WotDossier.Tabs
{
    /// <summary>
    /// Interaction logic for CommonTabNew.xaml
    /// </summary>
    public partial class CommonTabNew : UserControl
    {
        public CommonTabNew()
        {
            InitializeComponent();
            Chart4.AxisGrid.DrawHorizontalTicks = false;
            Chart4.AxisGrid.DrawVerticalTicks = false;
            Chart4.Children.Remove(Chart4.MouseNavigation);
            Chart4.Children.Remove(Chart4.DefaultContextMenu);

            
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.FrameworkElement.Initialized"/> event. This method is invoked whenever <see cref="P:System.Windows.FrameworkElement.IsInitialized"/> is set to true internally. 
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.RoutedEventArgs"/> that contains the event data.</param>
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            InvalidateVisual();
        }

        private void Hyperlink_OnClick(object sender, RoutedEventArgs e)
        {
            Hyperlink hyperlink = e.OriginalSource as Hyperlink;
            if (hyperlink != null && hyperlink.NavigateUri != null)
            {
                Process.Start(hyperlink.NavigateUri.ToString());
            }
        }
    }
}
