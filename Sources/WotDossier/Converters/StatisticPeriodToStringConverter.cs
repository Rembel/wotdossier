using System;
using System.Globalization;
using System.Windows.Data;
using WotDossier.Common;
using WotDossier.Domain;

namespace WotDossier.Converters
{
    public class StatisticPeriodToStringConverter : IValueConverter
    {
        private static readonly StatisticPeriodToStringConverter _defaultInstance = new StatisticPeriodToStringConverter();

        public static StatisticPeriodToStringConverter Default { get { return _defaultInstance; } }

        /// <summary>
        /// Converts a value. 
        /// </summary>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        /// <param name="value">The value produced by the binding source.</param><param name="targetType">The type of the binding target property.</param><param name="parameter">The converter parameter to use.</param><param name="culture">The culture to use in the converter.</param>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && !string.IsNullOrEmpty(value.ToString()))
            {
                StatisticPeriod statisticPeriod = (StatisticPeriod)value;
                return Resources.Resources.ResourceManager.GetEnumResource(statisticPeriod);
            }
            return "N/A";
        }

        /// <summary>
        /// Converts a value. 
        /// </summary>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        /// <param name="value">The value that is produced by the binding target.</param><param name="targetType">The type to convert to.</param><param name="parameter">The converter parameter to use.</param><param name="culture">The culture to use in the converter.</param>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
