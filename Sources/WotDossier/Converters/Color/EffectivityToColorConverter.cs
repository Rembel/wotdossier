using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace WotDossier.Converters
{
    public class EffectivityToColorConverter : IValueConverter
    {
        private static readonly EffectivityToColorConverter defaultInstance = new EffectivityToColorConverter();

        public static EffectivityToColorConverter Default { get { return defaultInstance; } }


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double? eff = value as double?;
            if (eff != null)
            {
                if (eff >= 1735)
                    return Brushes.Purple;
                if (eff >= 1460)
                    return Brushes.CornflowerBlue;
                if (eff >= 1140)
                    return Brushes.Lime;
                if (eff >= 860)
                    return Brushes.Yellow;
                if (eff >= 630)
                    return Brushes.DarkOrange;
            }
            return Brushes.Red;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
