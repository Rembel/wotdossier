using System;
using System.Globalization;
using System.Windows.Data;
using OzWild.Applications;
using OzWild.Domain.Entities;

namespace OzWild.Gui.Converters
{
    public class ToolTipConverter : IMultiValueConverter
    {
        private static readonly ToolTipConverter defaultInstance = new ToolTipConverter();

        public static ToolTipConverter Default { get { return defaultInstance; } }


        public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
        {
            string toolTip = null;
            if (value[0] != null && value[1] != null && value[1] is IToolTipDataProvider)
            {
                var toolTipDataProvider = value[1] as IToolTipDataProvider;
                if (value[0] is KitLot)
                {
                    toolTip = toolTipDataProvider.CreateKitLotToolTip(value[0] as KitLot);
                }
                else if (value[0] is SampleDetail)
                {
                    toolTip = toolTipDataProvider.CreateSampleToolTip(value[0] as SampleDetail);
                }
                else if (value[0] is Well)
                {
                    toolTip = toolTipDataProvider.CreateWellToolTip(value[0] as Well);
                }
            }
            return String.IsNullOrEmpty(toolTip) ? null : toolTip;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
