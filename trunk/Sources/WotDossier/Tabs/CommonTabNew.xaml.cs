﻿using System;
using System.Windows.Controls;

namespace WotDossier.Tabs
{
    /// <summary>
    /// Interaction logic for CommonTabNew.xaml
    /// </summary>
    public partial class CommonTabNew : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommonTabNew"/> class.
        /// </summary>
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
    }
}
