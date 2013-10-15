using System.IO;
using System.IO.Compression;
using System.Text;
using Newtonsoft.Json;

namespace WotDossier.Dal
{
    public class WotApiHelper
    {
        public static string GetCountryNameCode(int countryid)
        {
            switch (countryid)
            {
                case 0:
                    return "ussr";
                case 1:
                    return "germany";
                case 2:
                    return "usa";
                case 3:
                    return "china";
                case 4:
                    return "france";
                case 5:
                    return "uk";
                case 6:
                    return "japan";
            }
            return string.Empty;
        }

        public static int GetCountryId(string countryCode)
        {
            switch (countryCode.ToLower())
            {
                case "ussr":
                    return 0;
                case "germany":
                    return 1;
                case "usa":
                    return 2;
                case "china":
                    return 3;
                case "france":
                    return 4;
                case "uk":
                    return 5;
                case "jp":
                    return 6;
            }
            return -1;
        }

        public static int GetCountryIdBy2Letters(string countryCode)
        {
            switch (countryCode.ToLower())
            {
                case "ru":
                    return 0;
                case "de":
                    return 1;
                case "us":
                    return 2;
                case "ch":
                    return 3;
                case "fr":
                    return 4;
                case "uk":
                    return 5;
                case "jp":
                    return 6;
            }
            return -1;
        }

        public static byte[] Zip(string value)
        {
            using (var ms = new MemoryStream())
            {
                using (var zip = new GZipStream(ms, CompressionMode.Compress))
                using (var writer = new StreamWriter(zip, Encoding.UTF8))
                {
                    writer.Write(value);
                }
                return ms.ToArray();
            }
        }

        public static string UnZip(byte[] byteArray)
        {
            using (var ms = new MemoryStream(byteArray))
            using (var zip = new GZipStream(ms, CompressionMode.Decompress))
            using (var sr = new StreamReader(zip, Encoding.UTF8))
            {
                return sr.ReadToEnd();
            }
        }

        public static T UnZipObject<T>(byte[] byteArray)
        {
            string json = UnZip(byteArray);
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}