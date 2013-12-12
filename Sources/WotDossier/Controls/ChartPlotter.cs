using System.Windows;
using Microsoft.Research.DynamicDataDisplay;

namespace WotDossier.Controls
{
    public class ChartPlotter : Microsoft.Research.DynamicDataDisplay.ChartPlotter
    {
        #region public double MaxY

        public const string MAX_Y_PROPERTY_NAME = "MaxY";

        /// <summary>
        /// Identifies the MaxY dependency property.
        /// </summary>
        public static DependencyProperty MaxYProperty =
            DependencyProperty.Register(MAX_Y_PROPERTY_NAME, typeof(double), typeof(ChartPlotter), new PropertyMetadata(double.MaxValue, ChangeMax));

        /// <summary>
        /// 
        /// </summary>
        public double MaxY
        {
            get { return (double)GetValue(MaxYProperty); }

            set { SetValue(MaxYProperty, value); }
        }

        #endregion public double MaxY

        #region public double MaxX

        public const string MAX_X_PROPERTY_NAME = "MaxX";

        /// <summary>
        /// Identifies the MinY dependency property.
        /// </summary>
        public static DependencyProperty MaxXProperty =
            DependencyProperty.Register(MAX_X_PROPERTY_NAME, typeof(double), typeof(ChartPlotter), new PropertyMetadata(double.MaxValue, ChangeMax));

        /// <summary>
        /// 
        /// </summary>
        public double MaxX
        {
            get { return (double)GetValue(MaxXProperty); }

            set { SetValue(MaxXProperty, value); }
        }

        public static readonly DependencyProperty MinXProperty =
            DependencyProperty.Register("MinX", typeof(double), typeof(ChartPlotter), new PropertyMetadata((double)0, ChangeMax));

        public double MinX
        {
            get { return (double) GetValue(MinXProperty); }
            set { SetValue(MinXProperty, value); }
        }

        #endregion public double MinY

        private static void ChangeMax(DependencyObject source, DependencyPropertyChangedEventArgs eventArgs)
        {
            ((ChartPlotter)source).ConfigureAxises();
        }

        private void ConfigureAxises()
        {
            DataRect dataRect = DataRect.Create(MinX, 0, MaxX, MaxY);
            Viewport.Domain = dataRect;
            Viewport.Visible = dataRect;
        }
    }
}
