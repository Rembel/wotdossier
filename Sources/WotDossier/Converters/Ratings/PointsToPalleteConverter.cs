using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using De.TorstenMandelkow.MetroChart;
using WotDossier.Applications.Logic;
using WotDossier.Applications.ViewModel.Chart;

namespace WotDossier.Converters.Ratings
{
    public class PointsToPalleteConverter : IValueConverter
    {
        private static readonly PointsToPalleteConverter _defaultInstance = new PointsToPalleteConverter();

        /// <summary>
        /// Gets the default instance.
        /// </summary>
        /// <value>
        /// The default.
        /// </value>
        public static PointsToPalleteConverter Default { get { return _defaultInstance; } }
        
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
            if (value == null)
            {
                return null;
            }

            var points = (IList<IDataPoint>)value;

            var collection = new ResourceDictionaryCollection();

            int i = 1;

            foreach (var point in points)
            {
                var ratingStrategy = RatingsManager.Get((Rating) parameter);

                var convert = ratingStrategy.GetBrush((double?) point.GetValue());
                var resourceDictionary = new ResourceDictionary {{"Brush" + i, convert}};
                i++;
                collection.Add(resourceDictionary);
            }

            if (collection.Count == 0)
            {
                collection.Add(new ResourceDictionary { { "Brush1", new SolidColorBrush(Colors.Transparent) } });
            }

            return collection;
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
