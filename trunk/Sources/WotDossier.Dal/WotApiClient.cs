using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;
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

        private const string URL_GET_PLAYER_INFO = @"http://api.worldoftanks.{3}/{1}/account/info/?application_id={2}&account_id={0}";
        private const string URL_GET_PLAYER_RATINGS = @"http://api.worldoftanks.{3}/{1}/account/ratings/?application_id={2}&account_id={0}";
        private const string URL_GET_PLAYER_TANKS = @"http://api.worldoftanks.{3}/{1}/account/tanks/?application_id={2}&account_id={0}";
        private const string URL_GET_CLAN_INFO = @"http://api.worldoftanks.{3}/{1}/clan/info/?application_id={2}&clan_id={0}";
        private const string URL_SEARCH_PLAYER = @"http://api.worldoftanks.{3}/{1}/account/list/?application_id={2}&search={0}&limit={4}";
        private const string URL_SEARCH_CLAN = @"http://api.worldoftanks.{3}/{1}/clan/list/?application_id={2}&search={0}&limit={4}";
        private const string REPLAY_DATABLOCK_2 = "datablock_2";

        private static readonly object _syncObject = new object();
        private static volatile WotApiClient _instance = new WotApiClient();

        private readonly Dictionary<int, TankDescription> _tanksDictionary;
        private readonly Dictionary<string, TankIcon> _iconsDictionary = new Dictionary<string, TankIcon>();
        private readonly Dictionary<string, Map> _maps = new Dictionary<string, Map>();
        private Dictionary<int, TankServerInfo> _serverTanksDictionary;
        private Dictionary<string, RatingExpectancy> _ratingExpectations;

        /// <summary>
        /// Tanks dictionary
        /// KEY - tankid, countryid
        /// </summary>
        public Dictionary<int, TankDescription> TanksDictionary
        {
            get { return _tanksDictionary; }
        }

        public Dictionary<string, TankIcon> IconsDictionary
        {
            get { return _iconsDictionary; }
        }

        public Dictionary<int, TankServerInfo> ServerTanksDictionary
        {
            get { return _serverTanksDictionary; }
        }

        public Dictionary<string, Map> Maps
        {
            get { return _maps; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        private WotApiClient()
        {
            _ratingExpectations = ReadRatingExpectationsDictionary();
            _tanksDictionary = ReadTanksDictionary();
            _serverTanksDictionary = ReadServerTanksDictionary();
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

        public List<TankJson> ReadTanks(string path)
        {
            List<TankJson> tanks = new List<TankJson>();

            using (StreamReader re = new StreamReader(path))
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

        public List<TankJsonV2> ReadTanksV2(string path)
        {
            List<TankJsonV2> tanks = new List<TankJsonV2>();

            using (StreamReader re = new StreamReader(path))
            {
                JsonTextReader reader = new JsonTextReader(re);
                JsonSerializer se = new JsonSerializer();
                JObject parsedData = (JObject)se.Deserialize(reader);

                JToken tanksDataV2 = parsedData["tanks_v2"];

                foreach (JToken jToken in tanksDataV2)
                {
                    JProperty property = (JProperty)jToken;
                    string raw = property.Value.ToString();
                    TankJsonV2 tank = JsonConvert.DeserializeObject<TankJsonV2>(raw);
                    tank.Raw = WotApiHelper.Zip(JsonConvert.SerializeObject(tank));
                    ExtendPropertiesData(tank);
                    tanks.Add(tank);
                }
            }
            return tanks;
        }

        public void ExtendPropertiesData(TankJson tank)
        {
            tank.Description = _tanksDictionary[tank.UniqueId()];
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
                                     Icon = TanksDictionary[uniqueId].Icon,
                                     TankUniqueId = uniqueId,
                                     Count = Convert.ToInt32(x[2]),
                                     Type = TanksDictionary[uniqueId].Type,
                                     Tier = TanksDictionary[uniqueId].Tier,
                                     KilledByTankUniqueId = tank.UniqueId(),
                                     Tank = x[3]
                                 };
                        }).ToList();
        }

        public void ExtendPropertiesData(TankJsonV2 tank)
        {
            tank.Description = _tanksDictionary[tank.UniqueId()];
            tank.Frags =
                tank.FragsList.Select(
                    x =>
                    {
                        int countryId = Convert.ToInt32(x[0]);
                        int tankId = Convert.ToInt32(x[1]);
                        int uniqueId = Utils.ToUniqueId(countryId, tankId);
                        return new FragsJson
                        {
                            CountryId = countryId,
                            TankId = tankId,
                            Icon = TanksDictionary[uniqueId].Icon,
                            TankUniqueId = uniqueId,
                            Count = Convert.ToInt32(x[2]),
                            Type = TanksDictionary[uniqueId].Type,
                            Tier = TanksDictionary[uniqueId].Tier,
                            KilledByTankUniqueId = tank.UniqueId(),
                            Tank = x[3]
                        };
                    }).ToList();
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

        private Dictionary<int, TankDescription> ReadTanksDictionary()
        {
            List<TankDescription> tanks = new List<TankDescription>();
            using (StreamReader re = new StreamReader(@"External\tanks.json"))
            {
                JsonTextReader reader = new JsonTextReader(re);
                JsonSerializer se = new JsonSerializer();
                var parsedData = se.Deserialize<JArray>(reader);
                foreach (JToken jToken in parsedData)
                {
                    string json = jToken.ToString();

                    TankDescription tank = JsonConvert.DeserializeObject<TankDescription>(json);
                    tank.CountryCode = WotApiHelper.GetCountryNameCode(tank.CountryId);

                    TankIcon icon = JsonConvert.DeserializeObject<TankIcon>(json);
                    icon.CountryCode = tank.CountryCode;
                    _iconsDictionary.Add(icon.IconId, icon);
                    
                    tank.Icon = icon;

                    if (_ratingExpectations.ContainsKey(tank.Icon.IconOrig))
                    {
                        tank.Expectancy = _ratingExpectations[tank.Icon.IconOrig];
                    }

                    tanks.Add(tank);
                }
            }

            return tanks.ToDictionary(x => x.UniqueId());
        }

        private Dictionary<int, TankServerInfo> ReadServerTanksDictionary()
        {
            using (StreamReader re = new StreamReader(@"External\server_tanks.json"))
            {
                JsonTextReader reader = new JsonTextReader(re);
                JsonSerializer se = new JsonSerializer();
                var parsedData = se.Deserialize<JObject>(reader);
                return JsonConvert.DeserializeObject<Dictionary<int, TankServerInfo>>(parsedData["data"].ToString());
            }
        }

        private Dictionary<string, RatingExpectancy> ReadRatingExpectationsDictionary()
        {
            try
            {
                using (StreamReader re = new StreamReader(@"External\tanks_expectations.json"))
                {
                    JsonTextReader reader = new JsonTextReader(re);
                    JsonSerializer se = new JsonSerializer();
                    var parsedData = se.Deserialize<JArray>(reader);
                    return JsonConvert.DeserializeObject<List<RatingExpectancy>>(parsedData.ToString()).ToDictionary(x => x.Icon, x => x);
                }
            }
            catch (Exception e)
            {
                _log.Error(e);
            }
            return new Dictionary<string, RatingExpectancy>();
        }

        public static Dictionary<string, Map> ReadMaps()
        {
            List<Map> maps = null;
            using (StreamReader re = new StreamReader(@"External\maps.json"))
            {
                JsonTextReader reader = new JsonTextReader(re);
                JsonSerializer se = new JsonSerializer();
                maps = se.Deserialize<List<Map>>(reader);
            }

            List<Map> list = (maps ?? new List<Map>());
            int i = 1;
            list.ForEach( x => x.localizedmapname = Resources.Resources.ResourceManager.GetString("Map_" + x.mapidname) ?? x.mapname);
            list = list.OrderByDescending(x => x.localizedmapname).ToList();
            list.ForEach(x => x.mapid = i++);
            return list.ToDictionary(x => x.mapidname, y => y);
        }

        /// <summary>
        /// Loads player stat from server
        /// </summary>
        /// <exception cref="PlayerInfoLoadException"></exception>
        public PlayerStat LoadPlayerStat(AppSettings settings, int playerId)
        {
            if (settings == null || string.IsNullOrEmpty(settings.Server))
            {
                return null;
            }
            
            Stream stream;

            try
            {
                string url = string.Format(URL_GET_PLAYER_INFO, playerId, WotDossierSettings.ApiVersion, WotDossierSettings.GetAppId(settings.Server), settings.Server);
                WebRequest request = HttpWebRequest.Create(url);
                WebResponse response = request.GetResponse();
                stream = response.GetResponseStream();

                using (StreamReader streamReader = new StreamReader(stream))
                {
                    JsonTextReader reader = new JsonTextReader(streamReader);
                    JsonSerializer se = new JsonSerializer();
                    PlayerStat playerStat = se.Deserialize<PlayerStat>(reader);
                    playerStat.dataField = playerStat.data[playerId];
                    playerStat.dataField.ratings = GetPlayerRatings(settings, playerId);
                    playerStat.dataField.vehicles = GetPlayerTanks(settings, playerId);
                    if (playerStat.dataField.clan != null)
                    {
                        playerStat.dataField.clanData = LoadClan(settings, playerStat.dataField.clan.clan_id);
                    }
                    return playerStat;
                }
            }
            catch (Exception e)
            {
                _log.Error("Can't get player info from server", e);
                throw new PlayerInfoLoadException("Error on getting player data from server", e);
            }
        }

        private List<VehicleStat> GetPlayerTanks(AppSettings settings, int playerId)
        {
            try
            {
                string url = string.Format(URL_GET_PLAYER_TANKS, playerId, WotDossierSettings.ApiVersion, WotDossierSettings.GetAppId(settings.Server), settings.Server);
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

                        if (parsedData["data"].Any())
                        {
                            List<VehicleStat> tanks = JsonConvert.DeserializeObject<List<VehicleStat>>(parsedData["data"][playerId.ToString()].ToString());
                            foreach (VehicleStat tank in tanks)
                            {
                                if (ServerTanksDictionary.ContainsKey(tank.tank_id))
                                {
                                    tank.tank = ServerTanksDictionary[tank.tank_id];
                                }
                            }
                            return tanks;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                _log.Error("Error on player tanks loading", e);
            }

            return null;
        }

        private Ratings GetPlayerRatings(AppSettings settings, int playerId)
        {
            try
            {
                string url = string.Format(URL_GET_PLAYER_RATINGS, playerId, WotDossierSettings.ApiVersion, WotDossierSettings.GetAppId(settings.Server), settings.Server);
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

                        if (parsedData["data"].Any())
                        {
                            return JsonConvert.DeserializeObject<Ratings>(parsedData["data"][playerId.ToString()].ToString());
                        }
                    }
                }
            }
            catch (Exception e)
            {
                _log.Error("Error on player search", e);
            }

            return null;
        }

        /// <summary>
        /// Loads player stat from server
        /// </summary>
        /// <exception cref="PlayerInfoLoadException"></exception>
        public ClanData LoadClan(AppSettings settings, int clanId)
        {
            if (settings == null || string.IsNullOrEmpty(settings.Server))
            {
                return null;
            }

            try
            {
                string url = string.Format(URL_GET_CLAN_INFO, clanId, WotDossierSettings.ApiVersion, WotDossierSettings.GetAppId(settings.Server), settings.Server);
                WebRequest request = HttpWebRequest.Create(url);
                WebResponse response = request.GetResponse();
                Stream stream = response.GetResponseStream();
                
                using (StreamReader streamReader = new StreamReader(stream))
                {
                    JsonTextReader reader = new JsonTextReader(streamReader);
                    JsonSerializer se = new JsonSerializer();
                    JObject parsedData = (JObject)se.Deserialize(reader);
                    ClanData clan = JsonConvert.DeserializeObject<ClanData>(parsedData["data"][clanId.ToString()].ToString());
                    return clan;
                }
            }
            catch (Exception e)
            {
                _log.Error("Can't get clan info from server", e);
            }
            return null;
        }

        /// <summary>
        /// Searches the player.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>First found player</returns>
        public PlayerSearchJson SearchPlayer(AppSettings settings, string playerName)
        {
#if DEBUG
            return new PlayerSearchJson { created_at = 0, id = 10800699, name = "rembel"};
#else
            try
            {
                string url = string.Format(URL_SEARCH_PLAYER, playerName, WotDossierSettings.ApiVersion, WotDossierSettings.GetAppId(settings.Server), settings.Server, 1);
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

                        return JsonConvert.DeserializeObject<PlayerSearchJson>(parsedData["data"].FirstOrDefault().ToString());
                    }
                }
            }
            catch (Exception e)
            {
                _log.Error("Error on player search", e);
            }

            return null;
#endif
        }

        /// <summary>
        /// Searches the player.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>First found player</returns>
        public List<PlayerSearchJson> SearchPlayer(AppSettings settings, string playerName, int limit)
        {
            try
            {
                string url = string.Format(URL_SEARCH_PLAYER, playerName, WotDossierSettings.ApiVersion, WotDossierSettings.GetAppId(settings.Server), settings.Server, limit);
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

                        if (parsedData["status"].ToString() != "error" && parsedData["data"].Any())
                        {
                            return JsonConvert.DeserializeObject<List<PlayerSearchJson>>(parsedData["data"].ToString());
                        }
                    }
                }
            }
            catch (Exception e)
            {
                _log.Error("Error on player search", e);
            }

            return null;
        }

        /// <summary>
        /// Searches the player.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>First found player</returns>
        public List<ClanSearchJson> SearchClan(AppSettings settings, string clanName, int count)
        {
            try
            {
                string url = string.Format(URL_SEARCH_CLAN, clanName, WotDossierSettings.ApiVersion, WotDossierSettings.GetAppId(settings.Server), settings.Server, count);
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

                        if (parsedData["status"].ToString() != "error" && parsedData["data"].Any())
                        {
                            return JsonConvert.DeserializeObject<List<ClanSearchJson>>(parsedData["data"].ToString());
                        }
                    }
                }
            }
            catch (Exception e)
            {
                _log.Error("Error on clan search", e);
            }
            return null;
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
                if (parsedData != null && ((IDictionary<string, JToken>)parsedData).ContainsKey(REPLAY_DATABLOCK_2))
                {
                    CommandResult result = new CommandResult();
                    result.Damage = parsedData[REPLAY_DATABLOCK_2][0].ToObject<Damaged>();
                    result.Vehicles = parsedData[REPLAY_DATABLOCK_2][1].ToObject<Dictionary<long, Vehicle>>();
                    result.Frags = parsedData[REPLAY_DATABLOCK_2][2].ToObject<Dictionary<long, FragsCount>>();
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
                if (buffer[0] != 0x21){
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
                        commandResult.Vehicles = parsedData[1].ToObject<Dictionary<long, Vehicle>>();
                        commandResult.Frags = parsedData[2].ToObject<Dictionary<long, FragsCount>>();
                    }
                }
                catch (Exception e)
                {
                    _log.InfoFormat("Error on replay file read. Incorrect file format({0})", e, replayFileInfo.FullName);
                }

                if (firstBlock != null || commandResult != null)
                {
                    return new Replay {datablock_1 = firstBlock, CommandResult = commandResult};
                }
            }
            return null;
        }

        public Dictionary<string, VStat> ReadVstat()
        {
            using (StreamReader streamReader = new StreamReader(@"Data\vstat.json"))
            {
                JsonTextReader reader = new JsonTextReader(streamReader);
                JsonSerializer se = new JsonSerializer();
                var parsedData = se.Deserialize<Dictionary<string, VStat>>(reader);
                return parsedData;
            }
        }

        public Dictionary<int, TankDescription> ReadTankNominalDamage()
        {
            Dictionary<int, TankDescription> dictionary = new Dictionary<int, TankDescription>();
            using (StreamReader streamReader = new StreamReader(@"Data\TankNominalDamage.xml"))
            {
                XmlDocument document = new XmlDocument();
                document.Load(streamReader);

                XmlNodeList xmlNodeList = document.SelectNodes("damage/tr");

                foreach (XmlNode node in xmlNodeList)
                {
                    XmlNodeList values = node.SelectNodes("td");
                    if (values != null)
                    {
                        TankDescription description = new TankDescription();
                        description.CountryId = WotApiHelper.GetCountryIdBy2Letters(values[2].InnerText);
                        description.Tier = int.Parse(values[3].InnerText);
                        description.Type = (int)Enum.Parse(typeof(TankType), values[4].InnerText);
                        double nominalDamage = double.Parse(values[5].InnerText, CultureInfo.InvariantCulture);
                        dictionary.Add(description.UniqueId(), description);
                    }
                }
            }

            return dictionary;
        }
    }
}
