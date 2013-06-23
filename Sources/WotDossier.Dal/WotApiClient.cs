using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Common.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WotDossier.Common;
using WotDossier.Domain;
using System.Linq;
using WotDossier.Domain.Player;
using WotDossier.Domain.Replay;
using WotDossier.Domain.Tank;
using Vehicle = WotDossier.Domain.Replay.Vehicle;

namespace WotDossier.Dal
{
    /// <summary>
    /// Web Client for WoT web api
    /// https://gist.github.com/bartku/2419852
    /// </summary>
    public class WotApiClient
    {
        private static readonly ILog _log = LogManager.GetLogger("WotApiClient");

        private const string URL_GET_PLAYER_INFO = @"http://api.worldoftanks.{3}/community/accounts/{0}/api/{1}/?source_token={2}";
        private const string URL_SEARCH_PLAYER = @"http://api.worldoftanks.{3}/community/accounts/api/{1}/?source_token={2}&search={0}&offset=0&limit=1";

        private static readonly object _syncObject = new object();
        private static volatile WotApiClient _instance = new WotApiClient();

        private readonly Dictionary<int, TankInfo> _tanksDictionary;
        private readonly Dictionary<string, TankIcon> _iconsDictionary = new Dictionary<string, TankIcon>();
        private readonly List<Map> _maps = new List<Map>();
        
        /// <summary>
        /// Tanks dictionary
        /// KEY - tankid, countryid
        /// </summary>
        public Dictionary<int, TankInfo> TanksDictionary
        {
            get { return _tanksDictionary; }
        }

        public Dictionary<string, TankIcon> IconsDictionary
        {
            get { return _iconsDictionary; }
        }

        public List<Map> Maps
        {
            get { return _maps; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        private WotApiClient()
        {
            _tanksDictionary = ReadTanksDictionary();
            _maps = ReadMaps();
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
            tank.Info = _tanksDictionary[tank.UniqueId()];
            tank.Icon = GetTankIcon(tank);
            tank.Frags =
                tank.Kills.Select(
                    x =>
                        {
                            int countryId = Convert.ToInt32(x[0]);
                            int tankId = Convert.ToInt32(x[1]);
                            int uniqueId = Utils.ToUniqueId(countryId, tankId);
                            return new FragsJson
                                 {
                                     CountryId = countryId,
                                     TankId = tankId,
                                     Icon = GetTankIcon(TanksDictionary[uniqueId]),
                                     TankUniqueId = uniqueId,
                                     Count = Convert.ToInt32(x[2]),
                                     Type = TanksDictionary[uniqueId].type,
                                     Tier = TanksDictionary[uniqueId].tier,
                                     KilledByTankUniqueId = tank.UniqueId(),
                                     Tank = x[3]
                                 };
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

        public TankIcon GetTankIcon(string playerVehicle)
        {
            string replace = playerVehicle.Replace(":", "_").Replace("-", "_").Replace(" ", "_").Replace(".", "_").ToLower();
            if (IconsDictionary.ContainsKey(replace))
            {
                return IconsDictionary[replace];
            }
            return TankIcon.Empty;
        }

        private Dictionary<int, TankInfo> ReadTanksDictionary()
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

                    TankIcon tankIcon = new TankIcon
                        {
                            country_code = tank.countryCode,
                            country_id = tank.countryid,
                            iconid = string.Format("{0}_{1}", tank.countryCode, tank.icon)
                        };
                    _iconsDictionary.Add(tankIcon.iconid, tankIcon);
                }
            }

            return tanks.ToDictionary(x => x.UniqueId());
        }

        public static List<Map> ReadMaps()
        {
            List<Map> maps = null;
            using (StreamReader re = new StreamReader(@"External\maps.json"))
            {
                JsonTextReader reader = new JsonTextReader(re);
                JsonSerializer se = new JsonSerializer();
                maps = se.Deserialize<List<Map>>(reader);
            }

            return maps ?? new List<Map>();
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
#if DEBUG
            return new PlayerSearchJson { created_at = 0, id = 10800699, name = "rembel"};
#endif

            string url = string.Format(URL_SEARCH_PLAYER, settings.PlayerId, WotDossierSettings.SearchApiVersion, WotDossierSettings.SourceToken, settings.Server);
            WebRequest request = HttpWebRequest.Create(url);
            WebResponse response = request.GetResponse();
            using (Stream stream = response.GetResponseStream())
            {
                if (stream != null)
                {
                    StreamReader streamReader = new StreamReader(stream);
                    JsonTextReader reader = new JsonTextReader(streamReader);
                    JsonSerializer se = new JsonSerializer();
                    JObject parsedData = (JObject)se.Deserialize(reader);

                    if (parsedData["data"]["items"].Any())
                    {
                        return JsonConvert.DeserializeObject<PlayerSearchJson>(parsedData["data"]["items"][0].ToString());
                    }
                }
                return null;
            }
        }

        public Replay ReadReplay(string json)
        {
            Replay replay;

            using (StreamReader re = new StreamReader(json))
            {
                JsonTextReader reader = new JsonTextReader(re);
                JsonSerializer se = new JsonSerializer();
                replay = se.Deserialize<Replay>(reader);
            }

            using (StreamReader re = new StreamReader(json))
            {
                JsonTextReader reader = new JsonTextReader(re);
                JsonSerializer se = new JsonSerializer();
                JObject parsedData = (JObject)se.Deserialize(reader);
                if (parsedData.Count > 2)
                {
                    CommandResult result = new CommandResult();
                    result.Damage = parsedData["datablock_2"][0].ToObject<Damaged>();
                    result.Vehicles = parsedData["datablock_2"][1].ToObject<Dictionary<int, Vehicle>>();
                    result.Frags = parsedData["datablock_2"][2].ToObject<Dictionary<int, FragsCount>>();
                    replay.CommandResult = result;
                }
            }

            return replay;
        }

        public Replay ReadReplay2Blocks(FileInfo replayFileInfo)
        {
            string path = replayFileInfo.FullName;
            string str = string.Empty;
            string str2 = string.Empty;
            if (File.Exists(path))
            {
                FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
                int count = 0;
                byte[] buffer = new byte[4];
                stream.Read(buffer, 0, 4);
                if (buffer[0] != 0x21)
                {
                    stream.Read(buffer, 0, 4);
                    stream.Read(buffer, 0, 4);
                    count = ((buffer[0] + (0x100 * buffer[1])) + (0x10000 * buffer[2])) + (0x1000000 * buffer[3]);
                }
                byte[] buffer2 = new byte[count];
                stream.Read(buffer2, 0, count);
                ASCIIEncoding encoding = new ASCIIEncoding();
                str = encoding.GetString(buffer2);
                if (count > 0)
                {
                    stream.Read(buffer, 0, 4);
                    count = ((buffer[0] + (0x100 * buffer[1])) + (0x10000 * buffer[2])) + (0x1000000 * buffer[3]);
                    buffer2 = new byte[count];
                    stream.Read(buffer2, 0, count);
                    str2 = encoding.GetString(buffer2);
                }
                stream.Close();

                FirstBlock firstBlock = null;
                CommandResult commandResult = null;

                if (str.Length > 0)
                {
                    firstBlock = JsonConvert.DeserializeObject<FirstBlock>(str);
                }

                try
                {
                    var reader = new JsonTextReader(new StringReader(str2));
                    var se = new JsonSerializer();
                    var parsedData = (JArray)se.Deserialize(reader);
                    if (parsedData.Count > 2)
                    {
                        commandResult = new CommandResult();
                        commandResult.Damage = parsedData[0].ToObject<Damaged>();
                        commandResult.Vehicles = parsedData[1].ToObject<Dictionary<int, Vehicle>>();
                        commandResult.Frags = parsedData[2].ToObject<Dictionary<int, FragsCount>>();
                    }
                }
                catch (Exception e)
                {
                    _log.Error("Error on replay file read. Incorrect file format", e);
                    return null;
                }

                return new Replay { datablock_1 = firstBlock, CommandResult = commandResult };
            }
            return null;
        }
    }
}
