using System;
using System.Globalization;
using System.Windows.Data;
using OzWild.Applications.StateMachine;
using OzWild.Common.Interfaces;

namespace OzWild.Gui.Converters
{
    public class DeviceStateToStringConvertor : IValueConverter
    {
        private static readonly DeviceStateToStringConvertor _defaultInstance = new DeviceStateToStringConvertor();

        public static DeviceStateToStringConvertor DefaultInstance
        {
            get { return _defaultInstance; }
        }

        /// <summary>
        /// Converts a value. 
        /// </summary>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        /// <param name="value">The value produced by the binding source.</param><param name="targetType">The type of the binding target property.</param><param name="parameter">The converter parameter to use.</param><param name="culture">The culture to use in the converter.</param>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            IDevice device = value as IDevice;
            if (device != null)
            {
                if (device.DeviceState.Id == (int)DeviceState.Standby)
                {
                    return "Standby";
                }
                if (device.DeviceState.Id == (int) DeviceState.Test)
                {
                    return "Test";
                }
                if (device.DeviceState.Id == (int) DeviceState.Diagnose)
                {
                    return "Diagnose";
                }
                if (device.DeviceState.Id == (int) DeviceState.Failed)
                {
                    return "Failed";
                }
                if (device.DeviceState.Id == (int) DeviceState.TestComplete)
                {
                    return "TestComplete";
                }
                if (device.DeviceState.Id == (int) DeviceState.Ready)
                {
                    return "Ready";
                }
                if (device.DeviceState.Id == (int) DeviceState.Heat)
                {
                    return "Heat";
                }
                if (device.DeviceState.Id == (int) DeviceState.Connected)
                {
                    return "Connected";
                }
            }
            return "Unconnected";
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
            return value;
        }
    }
}
