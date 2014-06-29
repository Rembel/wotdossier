﻿using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace WotDossier.Converters.Color
{
    /// <summary>
    /// Converts Win percent value to <see cref="SolidColorBrush"/>
    /// </summary>
    public class PercentToColorConverter : IValueConverter
    {
        private static readonly PercentToColorConverter defaultInstance = new PercentToColorConverter();

        /// <summary>
        /// Gets the default instance.
        /// </summary>
        /// <value>
        /// The default.
        /// </value>
        public static PercentToColorConverter Default { get { return defaultInstance; } }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double eff = (double)value;
            if (eff >= 64.5)
            {
                return EffRangeBrushes.Purple;
            }
            if (eff >= 56.5)
            {
                return EffRangeBrushes.Blue;
            }
            if (eff >= 51.5)
            {
                return EffRangeBrushes.Green;
            }
            if (eff >= 48.5)
            {
                return EffRangeBrushes.Yellow;
            }
            if (eff >= 46.5)
            {
                return EffRangeBrushes.Orange;
            }
            return EffRangeBrushes.Red;
        }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
