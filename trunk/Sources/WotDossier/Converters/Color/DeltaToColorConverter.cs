using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace WotDossier.Converters.Color
{
    public class DeltaToColorConverter : IValueConverter
    {
        private static readonly DeltaToColorConverter defaultInstance = new DeltaToColorConverter();

        public static DeltaToColorConverter Default { get { return defaultInstance; } }


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double delta;
            if(value is int)
            {
                delta = (int)value;
            }
            else
            {
                delta = (double)value;
            }
            if (Math.Abs(delta - 0.0) < 0.001)
                return new SolidColorBrush(System.Windows.Media.Color.FromRgb(186, 191, 186));
            bool negativBetter = parameter != null ? bool.Parse(parameter.ToString()) : false;
            if ((delta > 0.0 && !negativBetter)
                || (delta < 0.0 && negativBetter))
                return Brushes.LimeGreen;
            return Brushes.Red;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
