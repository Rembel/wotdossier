using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Common.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WotDossier.Domain;
using System.Linq;
using WotDossier.Domain.Player;
using WotDossier.Domain.Tank;

namespace WotDossier.Dal
{
    /// <summary>
    /// Web Client for WoT web api
    /// https://gist.github.com/bartku/2419852
    /// </summary>
    public class WotApiClient
    {
        protected static readonly ILog _log = LogManager.GetLogger("WotApiClient");

        private const string URL_GET_PLAYER_INFO = @"http://api.worldoftanks.{3}/community/accounts/{0}/api/{1}/?source_token={2}";
        private const string URL_SEARCH_PLAYER = @"http://api.worldoftanks.{3}/community/accounts/api/{1}/?source_token={2}&search={0}&offset=0&limit=1";

        private static readonly object _syncObject = new object();
        private static volatile WotApiClient _instance = new WotApiClient();

        private static readonly Dictionary<KeyValuePair<int, int>, TankInfo> _tanksDictionary;
        private static readonly Dictionary<string, TankIcon> _iconsDictionary;
        
        public static Dictionary<KeyValuePair<int, int>, TankInfo> TanksDictionary
        {
            get { return _tanksDictionary; }
        }

        public static Dictionary<string, TankIcon> IconsDictionary
        {
            get { return _iconsDictionary; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        static WotApiClient()
        {
            _tanksDictionary = ReadTanks();
            _iconsDictionary = ReadTankIcons();
        }

        public static WotApiClient Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_syncObject)
                    {
                        if (_instance == null)
                        {
                            _instance = new WotApiClient();
                        }
                    }
                }
                return _instance;
            }
        }

        public List<TankJson> ReadTanks(string json)
        {
            List<TankJson> tanks = new List<TankJson>();

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
                    TankJson tank = JsonConvert.DeserializeObject<TankJson>(raw);
                    tank.Raw = WotApiHelper.Zip(JsonConvert.SerializeObject(tank));
                    ExtendPropertiesData(tank);
                    tanks.Add(tank);
                }
            }
            return tanks;
        }

        public void ExtendPropertiesData(TankJson tank)
        {
            tank.Info = _tanksDictionary[new KeyValuePair<int, int>(tank.Common.tankid, tank.Common.countryid)];
            tank.Icon = GetTankIcon(tank);
            tank.Frags =
                tank.Kills.Select(
                    x =>
                    new FragsJson
                        {
                            CountryId = Convert.ToInt32(x[0]),
                            TankId = Convert.ToInt32(x[1]),
                            Count = Convert.ToInt32(x[2]),
                            Name = x[3]
                        });
        }

        public TankIcon GetTankIcon(TankJson tank)
        {
            return GetTankIcon(tank.Info);
        }

        public TankIcon GetTankIcon(TankInfo tank)
        {
            string key = string.Format("{0}_{1}", tank.countryCode, tank.icon.ToLowerInvariant());
            if (_iconsDictionary.ContainsKey(key))
            {
                return _iconsDictionary[key];
            }
            return TankIcon.Empty;
        }

        private static Dictionary<KeyValuePair<int, int>, TankInfo> ReadTanks()
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
                    tank.countryCode = WotApiHelper.GetCountryNameCode(tank.countryid);
                    tanks.Add(tank);
                }
            }

            return tanks.ToDictionary(x => new KeyValuePair<int, int>(x.tankid, x.countryid));
        }

        private static Dictionary<string, TankIcon> ReadTankIcons()
        {
            List<TankIcon> tanks = new List<TankIcon>();
            using (StreamReader re = new StreamReader(@"External\contour.json"))
            {
                JsonTextReader reader = new JsonTextReader(re);
                JsonSerializer se = new JsonSerializer();
                var parsedData = se.Deserialize<JArray>(reader);
                foreach (JToken jToken in parsedData)
                {
                    TankIcon tank = JsonConvert.DeserializeObject<TankIcon>(jToken.ToString());
                    tanks.Add(tank);
                }
            }

            return tanks.ToDictionary(x => x.iconid.ToLowerInvariant());
        }

        /// <summary>
        /// Loads player stat from server
        /// </summary>
        /// <exception cref="PlayerInfoLoadException"></exception>
        public PlayerStat LoadPlayerStat(AppSettings settings)
        {
            if (settings == null || string.IsNullOrEmpty(settings.PlayerId) || string.IsNullOrEmpty(settings.Server))
            {
                return null;
            }
            
#if DEBUG
            long playerId = 10800699;
            using (StreamReader streamReader = new StreamReader(@"stat.json"))
#else
            PlayerSearchJson player = null;

            try
            {
                player = SearchPlayer(settings);
            }
            catch (Exception e)
            {
                _log.Error("Can't get player id from server", e);
                throw new PlayerInfoLoadException("Error on getting player data from server", e);
            }

            if (player == null)
            {
                return null;
            }

            Stream stream = null;
            long playerId = player.id;

            try
            {
                string url = string.Format(URL_GET_PLAYER_INFO, playerId, WotDossierSettings.ApiVersion, WotDossierSettings.SourceToken, settings.Server);
                WebRequest request = HttpWebRequest.Create(url);
                WebResponse response = request.GetResponse();
                stream = response.GetResponseStream();
            }
            catch (Exception e)
            {
                _log.Error("Can't get player info from server", e);
                throw new PlayerInfoLoadException("Error on getting player data from server", e);
            }

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

        /// <summary>
        /// Searches the player.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>First found player</returns>
        public PlayerSearchJson SearchPlayer(AppSettings settings)
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

                if (parsedData["data"]["items"].Any())
                {
                    return JsonConvert.DeserializeObject<PlayerSearchJson>(parsedData["data"]["items"][0].ToString());
                }
                return null;
            }
        }
    }
}
