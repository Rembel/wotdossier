using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace WotDossier.Converters
{
    public class StringToDataTemplateConverter : IValueConverter
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
            if(value == null)
            {
                return null;
            }

            var resources = Application.Current.Resources.MergedDictionaries.ToList();

            foreach (var dict in resources)
            {
                foreach (var objkey in dict.Keys)
                {
                    if (objkey.ToString() == value.ToString())
                    {
                        return dict[objkey] as DataTemplate;
                    }
                }
            }

            return null;
        }
    }
}
