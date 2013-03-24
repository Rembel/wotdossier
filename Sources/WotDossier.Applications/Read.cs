using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Windows;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WotDossier.Domain;
using System.Linq;
using WotDossier.Domain.Player;
using WotDossier.Domain.Rows;

namespace WotDossier.Applications
{

    public class Read
    {
        private const string URL_GET_PLAYER_INFO = @"http://api.worldoftanks.{3}/community/accounts/{0}/api/{1}/?source_token={2}";
        private const string URL_SEARCH_PLAYER = @"http://api.worldoftanks.{3}/community/accounts/api/{1}/?source_token={2}&search={0}&offset=0&limit=1";

        private static readonly object _syncObject = new object();
        private static volatile Read _instance = new Read();

        private static readonly Dictionary<KeyValuePair<int, int>, TankInfo> _tankDictionary;
        private static readonly Dictionary<string, TankContour> _contoursDictionary;
        
        public static Dictionary<KeyValuePair<int, int>, TankInfo> TankDictionary
        {
            get { return _tankDictionary; }
        }

        public static Dictionary<string, TankContour> ContoursDictionary
        {
            get { return _contoursDictionary; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        static Read()
        {
            _tankDictionary = ReadTanksLibrary();
            _contoursDictionary = ReadTankContours();
        }

        public static Read Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_syncObject)
                    {
                        if (_instance == null)
                        {
                            _instance = new Read();
                        }
                    }
                }
                return _instance;
            }
        }

        public static List<Tank> ReadTanks(string json)
        {
            List<Tank> tanks = new List<Tank>();

            using (StreamReader re = new StreamReader(json))
            {
                JsonTextReader reader = new JsonTextReader(re);
                JsonSerializer se = new JsonSerializer();
                JObject parsedData = (JObject)se.Deserialize(reader);
                //JToken headerData = parsedData["header"];
                JToken tanksData = parsedData["tanks"];

                foreach (JToken jToken in tanksData)
                {
                    JProperty property = (JProperty)jToken;
                    string raw = property.Value.ToString();
                    Tank tank = JsonConvert.DeserializeObject<Tank>(raw);
                    tank.Name = tank.Common.tanktitle;
                    tank.Info = _tankDictionary[new KeyValuePair<int, int>(tank.Common.tankid, tank.Common.countryid)];
                    tank.TankContour = GetTankContour(tank);
                    tank.Frags =
                        tank.Kills.Select(
                            x =>
                            new Frag
                                {
                                    CountryId = Convert.ToInt32(x[0]),
                                    TankId = Convert.ToInt32(x[1]),
                                    Count = Convert.ToInt32(x[2]),
                                    Name = x[3]
                                });
                    tank.Raw = Zip(raw);
                    tanks.Add(tank);
                }
            }
            return tanks;
        }

        public static TankContour GetTankContour(Tank tank)
        {
            return GetTankContour(tank.Info);
        }

        public static TankContour GetTankContour(TankInfo tank)
        {
            string key = string.Format("{0}_{1}", tank.countryCode, tank.icon.ToLowerInvariant());
            if (_contoursDictionary.ContainsKey(key))
            {
                return _contoursDictionary[key];
            }
            return TankContour.Empty;
        }

        private static string GetCountryNameCode(int countryid)
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
            }
            return string.Empty;
        }

        private static Dictionary<KeyValuePair<int, int>, TankInfo> ReadTanksLibrary()
        {
            List<TankInfo> tanks = new List<TankInfo>();
            using (StreamReader re = new StreamReader(@"External\tanks.json"))
            {
                JsonTextReader reader = new JsonTextReader(re);
                JsonSerializer se = new JsonSerializer();
                var parsedData = se.Deserialize<JArray>(reader);
                foreach (JToken jToken in parsedData)
                {
                    TankInfo tank = JsonConvert.DeserializeObject<TankInfo>(jToken.ToString());
                    tank.countryCode = GetCountryNameCode(tank.countryid);
                    tanks.Add(tank);
                }
            }

            return tanks.ToDictionary(x => new KeyValuePair<int, int>(x.tankid, x.countryid));
        }

        private static Dictionary<string, TankContour> ReadTankContours()
        {
            List<TankContour> tanks = new List<TankContour>();
            using (StreamReader re = new StreamReader(@"External\contour.json"))
            {
                JsonTextReader reader = new JsonTextReader(re);
                JsonSerializer se = new JsonSerializer();
                var parsedData = se.Deserialize<JArray>(reader);
                foreach (JToken jToken in parsedData)
                {
                    TankContour tank = JsonConvert.DeserializeObject<TankContour>(jToken.ToString());
                    tanks.Add(tank);
                }
            }

            return tanks.ToDictionary(x => x.iconid.ToLowerInvariant());
        }

        /// <summary>
        /// https://gist.github.com/bartku/2419852
        /// </summary>
        /// <returns></returns>
        public static PlayerStat LoadPlayerStat(AppSettings settings)
        {
            if (settings == null || string.IsNullOrEmpty(settings.PlayerId) || string.IsNullOrEmpty(settings.Server))
            {
                return null;
            }
            
#if DEBUG
            long playerId = 10800699;
            using (StreamReader streamReader = new StreamReader(@"stat.json"))
#else
            long playerId = GetPlayerId(settings);
            string url = string.Format(URL_GET_PLAYER_INFO, playerId, WotDossierSettings.ApiVersion, WotDossierSettings.SourceToken, settings.Server);
            WebRequest request = HttpWebRequest.Create(url);
            WebResponse response;
            
            try
            {
                response = request.GetResponse();
            }
            catch (Exception e)
            {
                MessageBox.Show("Can't get player info from server", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
            
            Stream stream = response.GetResponseStream();

            if (stream == null)
            {
                return null;
            }

            using (StreamReader streamReader = new StreamReader(stream))
#endif
            {
                JsonTextReader reader = new JsonTextReader(streamReader);
                JsonSerializer se = new JsonSerializer();
                PlayerStat loadPlayerStat = se.Deserialize<PlayerStat>(reader);
                loadPlayerStat.data.id = (int)playerId;
                return loadPlayerStat;
            }
        }

//#if DEBUG
        /// <summary>
        /// https://gist.github.com/bartku/2419852
        /// </summary>
        /// <returns></returns>
        public static PlayerStat LoadPrevPlayerStat(AppSettings settings)
        {
            if (settings == null || string.IsNullOrEmpty(settings.PlayerId) || string.IsNullOrEmpty(settings.Server))
            {
                return null;
            }

            using (StreamReader streamReader = new StreamReader(@"stat_prev.json"))
            {
                JsonTextReader reader = new JsonTextReader(streamReader);
                JsonSerializer se = new JsonSerializer();
                return se.Deserialize<PlayerStat>(reader);
            }
        }
//#endif

        public static long GetPlayerId(AppSettings settings)
        {
            string url = string.Format(URL_SEARCH_PLAYER, settings.PlayerId, WotDossierSettings.SearchApiVersion, WotDossierSettings.SourceToken, settings.Server);
            WebRequest request = HttpWebRequest.Create(url);
            WebResponse response = request.GetResponse();
            using (Stream stream = response.GetResponseStream())
            {
                StreamReader streamReader = new StreamReader(stream);
                JsonTextReader reader = new JsonTextReader(streamReader);
                JsonSerializer se = new JsonSerializer();
                JObject parsedData = (JObject)se.Deserialize(reader);
                return (long)((JValue)parsedData["data"]["items"][0].Last.First).Value;
            }
        }

        public static byte[] Zip(string value)
        {
            //Transform string into byte[]  
            byte[] byteArray = new byte[value.Length];
            int index = 0;
            foreach (char item in value)
            {
                byteArray[index++] = (byte)item;
            }

            //Prepare for compress
            MemoryStream ms = new MemoryStream();
            System.IO.Compression.GZipStream sw = new System.IO.Compression.GZipStream(ms,
                System.IO.Compression.CompressionMode.Compress);

            //Compress
            sw.Write(byteArray, 0, byteArray.Length);
            //Close, DO NOT FLUSH cause bytes will go missing...
            sw.Close();

            byteArray = ms.ToArray();
            ms.Close();
            sw.Dispose();
            ms.Dispose();
            return byteArray;
        }

        public static string UnZip(byte[] byteArray)
        {
            //Prepare for decompress
            MemoryStream ms = new MemoryStream(byteArray);
            System.IO.Compression.GZipStream sr = new System.IO.Compression.GZipStream(ms,
                System.IO.Compression.CompressionMode.Decompress);

            //Reset variable to collect uncompressed result
            byteArray = new byte[byteArray.Length];

            //Decompress
            int rByte = sr.Read(byteArray, 0, byteArray.Length);

            //Transform byte[] unzip data to string
            System.Text.StringBuilder sB = new System.Text.StringBuilder(rByte);
            //Read the number of bytes GZipStream red and do not a for each bytes in
            //resultByteArray;
            for (int i = 0; i < rByte; i++)
            {
                sB.Append((char)byteArray[i]);
            }
            sr.Close();
            ms.Close();
            sr.Dispose();
            ms.Dispose();
            return sB.ToString();
        }
    }
}
