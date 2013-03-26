using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using Common.Logging;
using WotDossier.Applications;
using WotDossier.Dal;
using WotDossier.Domain;

namespace WotDossier.Converters
{
    public class ClanImageUrlToSourceConverter : IValueConverter
    {
        protected static readonly ILog _log = LogManager.GetLogger("ClanImageUrlToSourceConverter");

        private static readonly ClanImageUrlToSourceConverter _defaultInstance = new ClanImageUrlToSourceConverter();

        public static ClanImageUrlToSourceConverter Default { get { return _defaultInstance; } }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string url = (string) value;
            
            if (!string.IsNullOrEmpty(url))
            {
                string fileName = url.Split('/').Last();

                string dir = Environment.CurrentDirectory + @"\IconsCache\";
                string path = dir + fileName;
                if (!File.Exists(path))
                {
                    try
                    {
                        if (!Directory.Exists(dir))
                        {
                            Directory.CreateDirectory(dir);
                        }

                        SettingsReader reader = new SettingsReader(WotDossierSettings.SettingsPath);
                        AppSettings appSettings = reader.Get();
                        WebRequest request = HttpWebRequest.Create(string.Format("http://worldoftanks.{1}{0}", url, appSettings.Server));
                        WebResponse response;
                        response = request.GetResponse();
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
                        MessageBox.Show("Can't get or save player clan icon from server", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        _log.Error("Error on clan icon load", e);
                        return null;
                    }
                }
                BitmapImage imageSource = new BitmapImage(new Uri(path));
                return imageSource;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
