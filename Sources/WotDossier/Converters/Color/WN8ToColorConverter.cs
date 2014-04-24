using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace WotDossier.Converters.Color
{
    /// <summary>
    /// Converts WN8 rating value to <see cref="SolidColorBrush"/>
    /// </summary>
    public class WN8ToColorConverter : IValueConverter
    {
        private static readonly WN8ToColorConverter _defaultInstance = new WN8ToColorConverter();

        /// <summary>
        /// Gets the default instance.
        /// </summary>
        /// <value>
        /// The default.
        /// </value>
        public static WN8ToColorConverter Default { get { return _defaultInstance; } }
        
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
            double? eff = value as double?;
            if (eff != null)
            {
                if (eff >= 2540)
                    return EffRangeBrushes.Purple;
                if (eff >= 1965)
                    return EffRangeBrushes.Blue;
                if (eff >= 1310)
                    return EffRangeBrushes.Green;
                if (eff >= 750)
                    return EffRangeBrushes.Yellow;
                if (eff >= 310)
                    return EffRangeBrushes.Orange;
                if (eff >= 0)
                    return EffRangeBrushes.Red;
            }
            return EffRangeBrushes.Black;
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
