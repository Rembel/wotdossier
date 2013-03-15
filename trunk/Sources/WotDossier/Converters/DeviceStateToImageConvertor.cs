using System;
using System.Globalization;
using System.Windows.Data;
using OzWild.Applications.StateMachine;
using OzWild.Common;
using OzWild.Domain.Entities;

namespace OzWild.Gui.Converters
{
    public class DeviceStateToImageConvertor : IValueConverter
    {
        /// <summary>
        /// Converts a value. 
        /// </summary>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        /// <param name="value">The value produced by the binding source.</param><param name="targetType">The type of the binding target property.</param><param name="parameter">The converter parameter to use.</param><param name="culture">The culture to use in the converter.</param>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Device device = value as Device;
            if(device != null)
            {
                if (device.RealDevice != null)
                {
                    //lid closed
                    if (device.RealDevice.LidState != LidState.Open)
                    {
                        if (device.RealDevice.DeviceState.Id == (int) DeviceState.Test)
                        {
                            return "/Mds;component/Resources/Images/Device/DeviceLidCloseSmallBlue.png";
                        }
                        if (device.RealDevice.DeviceState.Id == (int) DeviceState.Diagnose)
                        {
                            return "/Mds;component/Resources/Images/Device/DeviceLidCloseSmall.png";
                        }
                        if (device.RealDevice.DeviceState.Id == (int) DeviceState.Failed)
                        {
                            return "/Mds;component/Resources/Images/Device/DeviceLidCloseSmallRed.png";
                        }
                        if (device.RealDevice.DeviceState.Id == (int) DeviceState.TestComplete)
                        {
                            return "/Mds;component/Resources/Images/Device/DeviceLidCloseSmallRed.png";
                        }
                        if (device.RealDevice.DeviceState.Id == (int) DeviceState.Ready)
                        {
                            return "/Mds;component/Resources/Images/Device/DeviceLidCloseSmallGreen.png";
                        }
                        if (device.RealDevice.DeviceState.Id == (int) DeviceState.Heat)
                        {
                            return "/Mds;component/Resources/Images/Device/DeviceLidCloseSmallOrange.png";
                        }
                        if (device.RealDevice.DeviceState.Id == (int) DeviceState.Connected)
                        {
                            return "/Mds;component/Resources/Images/Device/DeviceLidCloseSmall.png";
                        }
                        if (device.RealDevice.DeviceState.Id == (int)DeviceState.Standby)
                        {
                            return "/Mds;component/Resources/Images/Device/DeviceLidCloseSmall.png";
                        }
                    }
                    else
                    {
                        if (device.RealDevice.DeviceState.Id == (int) DeviceState.Failed)
                        {
                            return "/Mds;component/Resources/Images/Device/DeviceLidOpenSmallRed.png";
                        }

                        if (device.RealDevice.DeviceState.Id == (int) DeviceState.Ready)
                        {
                            return "/Mds;component/Resources/Images/Device/DeviceLidOpenSmallGreen.png";
                        }

                        if (device.RealDevice.DeviceState.Id == (int) DeviceState.Heat)
                        {
                            return "/Mds;component/Resources/Images/Device/DeviceLidOpenSmallOrange.png";
                        }
                        if (device.RealDevice.DeviceState.Id == (int) DeviceState.Connected)
                        {
                            return "/Mds;component/Resources/Images/Device/DeviceLidOpenSmall.png";
                        }
                        if (device.RealDevice.DeviceState.Id == (int)DeviceState.Standby)
                        {
                            return "/Mds;component/Resources/Images/Device/DeviceLidOpenSmall.png";
                        }
                    }
                }
                return "/Mds;component/Resources/Images/Device/Devices.png";
            }
            return string.Empty;
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
