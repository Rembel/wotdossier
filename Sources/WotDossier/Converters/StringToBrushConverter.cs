using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace WotDossier.Converters
{
    public class StringToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return InternalConvert(value, targetType, parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public object InternalConvert(object value, Type targetType, object parameter)
        {
            if (value == null)
            {
                return null;
            }

            string colorName = value.ToString();
            SolidColorBrush scb = new SolidColorBrush();
            switch (colorName as string)
            {
                case "Magenta":
                    scb.Color = Colors.Magenta;
                    return scb;
                case "Purple":
                    scb.Color = Colors.Purple;
                    return scb;
                case "Brown":
                    scb.Color = Colors.Brown; 
                    return scb;
                case "Orange":
                    scb.Color = Colors.Orange;
                    return scb;
                case "Blue":
                    scb.Color = Colors.Blue;
                    return scb;
                case "Red":
                    scb.Color = Colors.Red;
                    return scb;
                case "Yellow":
                    scb.Color = Colors.Yellow;
                    return scb;
                case "Green":
                    scb.Color = Colors.Green;
                    return scb;
                default:
                    return null;
            }
        }
    }
}
