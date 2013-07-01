using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using Common.Logging;
using WotDossier.Applications;
using WotDossier.Applications.ViewModel;
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
            PlayerStatisticClanViewModel clan = (PlayerStatisticClanViewModel) value;

            if (clan == null)
            {
                return null;
            }

            string url = clan.large;

            if (!string.IsNullOrEmpty(url))
            {
                string fileName = clan.abbreviation.Replace("[", string.Empty).Replace("]", string.Empty);

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
                        MessageBox.Show(Resources.Resources.ClanImageUrlToSourceConverter_Convert_Can_t_get_or_save_player_clan_icon_from_server, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
