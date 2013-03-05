using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace WotDossier.Converters
{
    [ValueConversion(typeof(int), typeof(Int32Rect))]
    public class MarkOfMasteryImageConverter : IValueConverter
    {
        private static MarkOfMasteryImageConverter _default = new MarkOfMasteryImageConverter();

        public static MarkOfMasteryImageConverter Default
        {
            get { return _default; }
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
            int mark = (int)value;
            Int32Rect rect = Int32Rect.Empty;
            switch (mark)
            {
                case 1:
                    rect = new Int32Rect(0, 0, 20, 20);
                    break;
                case 2:
                    rect = new Int32Rect(0, 0, 20, 20);
                    break;
                case 3:
                    rect = new Int32Rect(0, 0, 20, 20);
                    break;
                case 4:
                    rect = new Int32Rect(0, 0, 20, 20);
                    break;
            }
            // Create an Image element.
            Image croppedImage = new Image();

            // Create a CroppedBitmap based off of a xaml defined resource.
            //CroppedBitmap cb = new CroppedBitmap(new BitmapImage(new Uri("pack://application:,,,/WotDossier;component/Resources/Images/award-images.png")), rect);       //select region rect

            CroppedBitmap cb = new CroppedBitmap(ToBitmapSource(Resources.Resources.award_images), rect);       //select region rect
            croppedImage.Source = cb;
            return croppedImage;
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

        /// <summary>
        /// Converts a <see cref="System.Drawing.Bitmap"/> into a WPF <see cref="BitmapSource"/>.
        /// </summary>
        /// <remarks>Uses GDI to do the conversion. Hence the call to the marshalled DeleteObject.
        /// </remarks>
        /// <param name="source">The source bitmap.</param>
        /// <returns>A BitmapSource</returns>
        public static BitmapSource ToBitmapSource(System.Drawing.Bitmap source)
        {
            BitmapSource bitSrc = null;

            var hBitmap = source.GetHbitmap();

            try
            {
                bitSrc = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    hBitmap,
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());
            }
            catch (Win32Exception)
            {
                bitSrc = null;
            }
            finally
            {
                NativeMethods.DeleteObject(hBitmap);
            }

            return bitSrc;
        }

        /// <summary>
        /// FxCop requires all Marshalled functions to be in a class called NativeMethods.
        /// </summary>
        internal static class NativeMethods
        {
            [DllImport("gdi32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool DeleteObject(IntPtr hObject);
        }
    }
}
