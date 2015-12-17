using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using WotDossier.Applications.ViewModel;
using WotDossier.Domain;

namespace WotDossier.Converters
{
    /// <summary>
    /// Bool to visibility
    /// </summary>
    public class BattleModeToVisibilityConverter : IValueConverter
    {
        private static readonly BattleModeToVisibilityConverter _defaultInstance = new BattleModeToVisibilityConverter();

        /// <summary>
        /// Gets the default instance.
        /// </summary>
        /// <value>
        /// The default.
        /// </value>
        public static BattleModeToVisibilityConverter Default { get { return _defaultInstance; } }

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
            var battleMode = (Enum)value;
            if (parameter != null)
            {
                return battleMode.Equals(parameter) ? Visibility.Visible : Visibility.Collapsed;
            }
            return Visibility.Visible;
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
