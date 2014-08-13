using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace WotDossier.Converters
{
    public class ValueNearestPercentFormater : IMultiValueConverter
    {
        private static readonly ValueNearestPercentFormater _default = new ValueNearestPercentFormater();
        
        /// <summary>
        /// Gets the default.
        /// </summary>
        /// <value>
        /// The default.
        /// </value>
        public static ValueNearestPercentFormater Default
        {
            get { return _default; }
        }

        /// <summary>
        /// Converts source values to a value for the binding target. The data binding engine calls this method when it propagates the values from source bindings to the binding target.
        /// </summary>
        /// <returns>
        /// A converted value.If the method returns null, the valid null value is used.A return value of <see cref="T:System.Windows.DependencyProperty"/>.<see cref="F:System.Windows.DependencyProperty.UnsetValue"/> indicates that the converter did not produce a value, and that the binding will use the <see cref="P:System.Windows.Data.BindingBase.FallbackValue"/> if it is available, or else will use the default value.A return value of <see cref="T:System.Windows.Data.Binding"/>.<see cref="F:System.Windows.Data.Binding.DoNothing"/> indicates that the binding does not transfer the value or use the <see cref="P:System.Windows.Data.BindingBase.FallbackValue"/> or the default value.
        /// </returns>
        /// <param name="values">The array of values that the source bindings in the <see cref="T:System.Windows.Data.MultiBinding"/> produces. The value <see cref="F:System.Windows.DependencyProperty.UnsetValue"/> indicates that the source binding has no value to provide for conversion.</param><param name="targetType">The type of the binding target property.</param><param name="parameter">The converter parameter to use.</param><param name="culture">The culture to use in the converter.</param>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if(values == null || (values.Length > 0 && values[0] == DependencyProperty.UnsetValue))
            {
                return String.Empty;
            }

            int val = (int)values[0];
            double percent = (double)values[1];
            bool? showAllNearest = (bool?)parameter;

            double percentNearestValue = GetPercentNearestValue(percent);

            IEnumerable<double> nearestValues;
            if (showAllNearest == true)
            {
                nearestValues = GetNearestValues(percentNearestValue);
            }
            else
            {
                nearestValues = new List<double>{percentNearestValue};
            }

            StringBuilder builder = new StringBuilder();

            foreach (double nearestValue in nearestValues)
            {
                double b = GetBattlesToNearest(nearestValue, percent, val);
                builder.AppendLine(string.Format("{0:### ### ###} ({1:0.00}% - {2} -> {3:0.0}%)", val, percent, b, nearestValue));    
            }

            return builder.ToString().Trim();
        }

        private static IEnumerable<double> GetNearestValues(double minValue)
        {
            return new List<double> { 46, 48, 50, 55, 60, 65, 70, 75 }.Where(x => x > minValue);
        }

        private int GetBattlesToNearest(double nearestPercent, double currentPercent, double value)
        {
            double result = 100*value*(nearestPercent/currentPercent - 1.0)/(100 - nearestPercent);

            int number = (int)result + 1;

            return number;
        }

        private double GetPercentNearestValue(double percent)
        {
            double nearest = (int)percent;
            nearest = nearest + 0.5;
            if (nearest < percent)
            {
                nearest = nearest + 0.5;
            }
            return nearest;
        }

        /// <summary>
        /// Converts a binding target value to the source binding values.
        /// </summary>
        /// <returns>
        /// An array of values that have been converted from the target value back to the source values.
        /// </returns>
        /// <param name="value">The value that the binding target produces.</param><param name="targetTypes">The array of types to convert to. The array length indicates the number and types of values that are suggested for the method to return.</param><param name="parameter">The converter parameter to use.</param><param name="culture">The culture to use in the converter.</param>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
