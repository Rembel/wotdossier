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
        private const string URL_GET_PLAYER_INFO = @"http://worldoftanks.{3}/community/accounts/{0}/api/{1}/?source_token={2}";
        private const string URL_SEARCH_PLAYER = "http://worldoftanks.{3}/community/accounts/api/{1}/?source_token={2}&search={0}&offset=0&limit=1";

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
                    Tank tank = JsonConvert.DeserializeObject<Tank>(property.Value.ToString());
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
                return se.Deserialize<PlayerStat>(reader);
            }
        }

        private static long GetPlayerId(AppSettings settings)
        {
            return 10800699;
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
    }
}
