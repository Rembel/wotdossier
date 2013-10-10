using System;
using System.Globalization;
using System.Windows.Data;
using WotDossier.Converters.Color;

namespace WotDossier.Converters
{
    public class PercentToColorConverter : IValueConverter
    {
        private static readonly PercentToColorConverter defaultInstance = new PercentToColorConverter();

        public static PercentToColorConverter Default { get { return defaultInstance; } }


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double eff = (double)value;
            if (eff >= 64)
            {
                return EffRangeBrushes.Purple;
            }
            if (eff >= 57)
            {
                return EffRangeBrushes.Blue;
            }
            if (eff >= 52)
            {
                return EffRangeBrushes.Green;
            }
            if (eff >= 49)
            {
                return EffRangeBrushes.Yellow;
            }
            if (eff >= 47)
            {
                return EffRangeBrushes.Orange;
            }
            return EffRangeBrushes.Red;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
