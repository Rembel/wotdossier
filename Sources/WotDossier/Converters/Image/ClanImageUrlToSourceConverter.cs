using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using Common.Logging;
using WotDossier.Applications.Model;

namespace WotDossier.Converters
{
    public class ClanImageUrlToSourceConverter : IValueConverter
    {
        private static readonly ILog _log = LogManager.GetCurrentClassLogger();

        private static readonly ClanImageUrlToSourceConverter _defaultInstance = new ClanImageUrlToSourceConverter();

        /// <summary>
        /// Gets the default instance.
        /// </summary>
        /// <value>
        /// The default.
        /// </value>
        public static ClanImageUrlToSourceConverter Default { get { return _defaultInstance; } }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ClanModel clan = (ClanModel)value;

            if (clan == null)
            {
                return null;
            }

            string url = clan.Emblems.large;

            if (!string.IsNullOrEmpty(url))
            {
                string fileName = clan.Abbreviation.Replace("[", string.Empty).Replace("]", string.Empty);

                string dir = Environment.CurrentDirectory + @"\IconsCache\";
                string path = dir + fileName + ".png";
                if (!File.Exists(path))
                {
                    try
                    {
                        if (!Directory.Exists(dir))
                        {
                            Directory.CreateDirectory(dir);
                        }

                        WebRequest request = HttpWebRequest.Create(url);
                        request.Proxy.Credentials = CredentialCache.DefaultCredentials;
                        WebResponse response = request.GetResponse();
                        Stream responseStream = response.GetResponseStream();

                        if (responseStream != null)
                        {
                            using (var streamReader = new BinaryReader(responseStream))
                            {
                                Byte[] lnByte = streamReader.ReadBytes(1 * 1024 * 1024 * 10);
                                using (FileStream destinationFile = File.Create(path))
                                {
                                    destinationFile.Write(lnByte, 0, lnByte.Length);
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        _log.Error("Error on clan icon load", e);
                        return null;
                    }
                }
                BitmapImage imageSource = new BitmapImage(new Uri(path));
                return imageSource;
            }

            return null;
        }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        /// <exception cref="System.NotSupportedException"></exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
