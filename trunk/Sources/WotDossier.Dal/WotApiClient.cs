using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using Common.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WotDossier.Common;
using WotDossier.Domain;
using System.Linq;
using WotDossier.Domain.Dossier.AppSpot;
using WotDossier.Domain.Replay;
using WotDossier.Domain.Server;
using WotDossier.Domain.Tank;
using Player = WotDossier.Domain.Server.Player;
using Vehicle = WotDossier.Domain.Server.Vehicle;

namespace WotDossier.Dal
{
    /// <summary>
    /// Web Client for WoT web api
    /// https://gist.github.com/bartku/2419852 
    /// </summary>
    public class WotApiClient
    {
        public static Version JsonFormatedResultsMinVersion = new Version("0.8.11.0");

        private static readonly ILog _log = LogManager.GetCurrentClassLogger();

        private const string URL_API = @"https://api.worldoftanks.{0}/{1}/{2}";
        private const string REPLAY_DATABLOCK_2 = "datablock_2";
        private const string CONTENT_TYPE = "application/x-www-form-urlencoded";
        private const string PARAM_APPID = "application_id";
        private const string PARAM_SEARCH = "search";
        private const string PARAM_ACCOUNT_ID = "account_id";
        private const string PARAM_IN_GARAGE = "in_garage";
        private const string PARAM_MEMBER_ID = "member_id";
        private const string PARAM_TYPE = "type";
        private const string PARAM_CLAN_ID = "clan_id";
        private const string PARAM_FIELDS = "fields";
        private const string PARAM_LIMIT = "limit";
        private const string METHOD_ACCOUNT_INFO = "account/info/";
        private const string METHOD_TANKS_STATS = "tanks/stats/";
        private const string METHOD_ACCOUNT_TANKS = "account/tanks/";
        private const string METHOD_ACCOUNT_ACHIEVEMENTS = "account/achievements/";
        private const string METHOD_RATINGS_ACCOUNTS = "ratings/accounts/";
        private const string METHOD_CLAN_INFO = "clan/info/";
        private const string METHOD_CLAN_MEMBERSINFO = "clan/membersinfo/";
        private const string METHOD_ACCOUNT_LIST = "account/list/";
        private const string METHOD_CLAN_LIST = "clan/list/";

        private static readonly object _syncObject = new object();
        private static volatile WotApiClient _instance = new WotApiClient();

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        private WotApiClient()
        {
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
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

        /// <summary>
        /// Reads the dossier application spot tanks.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public List<TankJson> ReadDossierAppSpotTanks(string data)
        {
            List<TankJson> tanks = new List<TankJson>();

            JObject parsedData = JsonConvert.DeserializeObject<JObject>(data);

            JToken tanksData = parsedData["tanks"];

            foreach (JToken jToken in tanksData)
            {
                Tank appSpotTank = jToken.ToObject<Tank>();
                TankJson tank = DataMapper.Map(appSpotTank);
                tank.Raw = CompressHelper.CompressObject(tank);
                if (ExtendPropertiesData(tank))
                {
                    tanks.Add(tank);
                }
            }

            return tanks;
        }

        /// <summary>
        /// Reads the tanks from cache.
        /// </summary>
        /// <param name="path">The path to parsed cache file.</param>
        /// <returns></returns>
        public List<TankJson> ReadTanksCache(string path)
        {
            _log.Trace("ReadTanksCache start");
            List<TankJson> tanks = new List<TankJson>();

            using (StreamReader re = new StreamReader(path))
            {
                JsonTextReader reader = new JsonTextReader(re);
                JsonSerializer se = new JsonSerializer();
                JObject parsedData = (JObject)se.Deserialize(reader);

                foreach (JToken tanksData in new[] { parsedData["tanks"], parsedData["tanks_v2"] })
                {
                    foreach (JToken jToken in tanksData)
                    {
                        JProperty property = (JProperty)jToken;
                        int version = property.Value["common"].ToObject<CommonJson>().basedonversion;
                        TankJson tank = DataMapper.Map(property.Value, version);
                        tank.Raw = CompressHelper.Compress(JsonConvert.SerializeObject(tank));
                        if (ExtendPropertiesData(tank))
                        {
                            tanks.Add(tank);
                        }
                    }
                }
            }
            _log.Trace("ReadTanksCache end");
            return tanks;
        }

        /// <summary>
        /// Extends the properties data with Description and FragsList.
        /// </summary>
        /// <param name="tank">The tank.</param>
        /// <returns></returns>
        public bool ExtendPropertiesData(TankJson tank)
        {
            if (Dictionaries.Instance.Tanks.ContainsKey(tank.UniqueId()) && !Dictionaries.Instance.NotExistsedTanksList.Contains(tank.UniqueId()))
            {
                tank.Description = Dictionaries.Instance.Tanks[tank.UniqueId()];
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
                                Icon = Dictionaries.Instance.Tanks[uniqueId].Icon,
                                TankUniqueId = uniqueId,
                                Count = Convert.ToInt32(x[2]),
                                Type = Dictionaries.Instance.Tanks[uniqueId].Type,
                                Tier = Dictionaries.Instance.Tanks[uniqueId].Tier,
                                KilledByTankUniqueId = tank.UniqueId(),
                                Tank = Dictionaries.Instance.Tanks[uniqueId].Title
                            };
                        }).ToList();
                return true;
            }
            _log.WarnFormat("Found unknown tank:\n{0}", JsonConvert.SerializeObject(tank.Common, Formatting.Indented));
            return false;
        }

        /// <summary>
        /// Loads player stat from server
        /// </summary>
        /// <exception cref="PlayerInfoLoadException"></exception>
        public Player LoadPlayerStat(int playerId, AppSettings settings)
        {
            return LoadPlayerStat(playerId, settings, true);
        }

        /// <summary>
        /// Loads player stat from server
        /// </summary>
        /// <exception cref="PlayerInfoLoadException"></exception>
        public Player LoadPlayerStat(int playerId, AppSettings settings, bool loadVehicles)
        {
            if (settings == null || string.IsNullOrEmpty(settings.Server))
            {
                return null;
            }

            try
            {
                var playerStat = Request<Player>(METHOD_ACCOUNT_INFO, new Dictionary<string, object>
                {
                    {PARAM_APPID, AppConfigSettings.GetAppId(settings.Server)},
                    {PARAM_ACCOUNT_ID, playerId},
                }, settings);
                playerStat.dataField = playerStat.data[playerId];
                playerStat.dataField.ratings = GetPlayerRatings(playerId, settings);
                playerStat.dataField.achievements = GetPlayerAchievements(playerId, settings);
                if (loadVehicles)
                {
                    playerStat.dataField.vehicles = GetPlayerTanks(playerId, settings);
                }

                Clan clanMemberInfo = GetClanMemberInfo(playerId, settings);

                if (clanMemberInfo != null)
                {
                    playerStat.dataField.clan = clanMemberInfo;
                    playerStat.dataField.clanData = LoadClan(clanMemberInfo.clan_id,
                        new[] {"abbreviation", "name", "clan_id", "description", "emblems"}, settings);
                }
                return playerStat;
            }
            catch (Exception e)
            {
                _log.Error("Can't get player info from server", e);
                return null;
            }
        }

        /// <summary>
        /// Gets the clan member information.
        /// </summary>
        /// <param name="playerId">The player identifier.</param>
        /// <param name="settings">The settings.</param>
        /// <returns></returns>
        private Clan GetClanMemberInfo(int playerId, AppSettings settings)
        {
            try
            {
                JObject parsedData = Request<JObject>(METHOD_CLAN_MEMBERSINFO, new Dictionary<string, object>
                {
                    {PARAM_APPID, AppConfigSettings.GetAppId(settings.Server)},
                    {PARAM_MEMBER_ID, playerId},
                }, settings);

                if (parsedData["data"].Any())
                {
                    return parsedData["data"][playerId.ToString(CultureInfo.InvariantCulture)].ToObject<Clan>();
                }
            }
            catch (Exception e)
            {
                _log.Error("Error on player tanks loading", e);
            }

            return null;
        }

        /// <summary>
        /// Gets the player tanks.
        /// </summary>
        /// <param name="playerId">The player identifier.</param>
        /// <param name="settings">The settings.</param>
        /// <returns></returns>
        private List<Vehicle> GetPlayerTanks(int playerId, AppSettings settings)
        {
            try
            {
                JObject parsedData = Request<JObject>(METHOD_TANKS_STATS, new Dictionary<string, object>
                {
                    {PARAM_APPID, AppConfigSettings.GetAppId(settings.Server)},
                    {PARAM_ACCOUNT_ID, playerId},
                    //{PARAM_IN_GARAGE, 1},
                }, settings);

                if (parsedData["data"].Any())
                {
                    List<Vehicle> tanks = parsedData["data"][playerId.ToString(CultureInfo.InvariantCulture)].ToObject<List<Vehicle>>();
                    foreach (Vehicle tank in tanks)
                    {
                        if (Dictionaries.Instance.ServerTanks.ContainsKey(tank.tank_id))
                        {
                            tank.tank = Dictionaries.Instance.ServerTanks[tank.tank_id];
                            tank.description = Dictionaries.Instance.Tanks.Values.FirstOrDefault(x => x.CompDescr == tank.tank_id);
                        }
                        else
                        {
                            _log.WarnFormat("Unknown tank id found [{0}] on get player server tank statistic", tank.tank_id);
                        }
                    }
                    return tanks;
                }
            }
            catch (Exception e)
            {
                _log.Error("Error on player tanks loading", e);
            }

            return null;
        }

        private Ratings GetPlayerRatings(int playerId, AppSettings settings)
        {
            try
            {
                JObject parsedData = Request<JObject>(METHOD_RATINGS_ACCOUNTS, new Dictionary<string, object>
                {
                    {PARAM_APPID, AppConfigSettings.GetAppId(settings.Server)},
                    {PARAM_ACCOUNT_ID, playerId},
                    {PARAM_TYPE, "all"},
                }, settings);

                if (parsedData["data"].Any())
                {
                    return parsedData["data"][playerId.ToString(CultureInfo.InvariantCulture)].ToObject<Ratings>();
                }
            }
            catch (Exception e)
            {
                _log.Error("Error on player search", e);
            }

            return null;
        }

        private Achievements GetPlayerAchievements(int playerId, AppSettings settings)
        {
            try
            {
                JObject parsedData = Request<JObject>(METHOD_ACCOUNT_ACHIEVEMENTS, new Dictionary<string, object>
                {
                    {PARAM_APPID, AppConfigSettings.GetAppId(settings.Server)},
                    {PARAM_ACCOUNT_ID, playerId}
                }, settings);

                if (parsedData["data"].Any())
                {
                    return parsedData["data"][playerId.ToString(CultureInfo.InvariantCulture)]["achievements"].ToObject<Achievements>();
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
        public ClanData LoadClan(int clanId, AppSettings settings)
        {
            return LoadClan(clanId, null, settings);
        }

        /// <summary>
        /// Loads player stat from server
        /// </summary>
        /// <exception cref="PlayerInfoLoadException"></exception>
        public ClanData LoadClan(int clanId, string[] fields, AppSettings settings)
        {
            if (settings == null || string.IsNullOrEmpty(settings.Server))
            {
                return null;
            }

            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>
                {
                    {PARAM_APPID, AppConfigSettings.GetAppId(settings.Server)},
                    {PARAM_CLAN_ID, clanId},
                };

                if (fields != null)
                {
                    dictionary.Add(PARAM_FIELDS, string.Join(",", fields));
                }

                JObject parsedData = Request<JObject>(METHOD_CLAN_INFO, dictionary, settings);
                return parsedData["data"][clanId.ToString(CultureInfo.InvariantCulture)].ToObject<ClanData>();
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
        /// <param name="playerName">Name of the player.</param>
        /// <param name="settings">The settings.</param>
        /// <returns>
        /// First found player
        /// </returns>
        public PlayerSearchJson SearchPlayer(string playerName, AppSettings settings)
        {
            List<PlayerSearchJson> list = SearchPlayer(playerName, 1, settings);
            if (list != null)
            {
                return list.FirstOrDefault();
            }
            return null;
        }

        /// <summary>
        /// Searches the player.
        /// </summary>
        /// <param name="playerName">Name of the player.</param>
        /// <param name="limit">The limit.</param>
        /// <param name="settings">The settings.</param>
        /// <returns>
        /// Found players
        /// </returns>
        public List<PlayerSearchJson> SearchPlayer(string playerName, int limit, AppSettings settings)
        {
#if DEBUG
            return new List<PlayerSearchJson> {new PlayerSearchJson {id = 10800699, nickname = "rembel"}};
#else
            try
            {
                JObject parsedData = Request<JObject>(METHOD_ACCOUNT_LIST, new Dictionary<string, object>
                {
                    {PARAM_APPID, AppConfigSettings.GetAppId(settings.Server)},
                    {PARAM_SEARCH, playerName},
                    {PARAM_LIMIT, limit},
                }, settings);
                return parsedData["data"].ToObject<List<PlayerSearchJson>>();
            }
            catch (Exception e)
            {
                _log.Error("Error on player search", e);
            }

            return null;
#endif
        }

        /// <summary>
        /// Search clans.
        /// </summary>
        /// <param name="clanName">Name of the clan.</param>
        /// <param name="count">The count.</param>
        /// <param name="settings">The settings.</param>
        /// <returns>Found clans</returns>
        public List<ClanSearchJson> SearchClan(string clanName, int count, AppSettings settings)
        {
            try
            {
                JObject parsedData = Request<JObject>(METHOD_CLAN_LIST, new Dictionary<string, object>
                {
                    {PARAM_APPID, AppConfigSettings.GetAppId(settings.Server)},
                    {PARAM_SEARCH, clanName},
                    {PARAM_LIMIT, count}
                }, settings);

                if (parsedData["status"].ToString() != "error" && parsedData["data"].Any())
                {
                    return parsedData["data"].ToObject<List<ClanSearchJson>>();
                }
            }
            catch (Exception e)
            {
                _log.Error("Error on clan search", e);
            }
            return null;
        }

        /// <summary>
        /// Loads the replay.
        /// </summary>
        /// <param name="replayFilePath">The replay file path.</param>
        /// <returns></returns>
        public Replay LoadReplay(string replayFilePath)
        {
            Replay replay;

            using (StreamReader re = new StreamReader(replayFilePath))
            {
                JsonTextReader reader = new JsonTextReader(re);
                JsonSerializer se = new JsonSerializer();
                replay = se.Deserialize<Replay>(reader);
            }

            using (StreamReader re = new StreamReader(replayFilePath))
            {
                JsonTextReader reader = new JsonTextReader(re);
                JsonSerializer se = new JsonSerializer();
                JObject parsedData = (JObject)se.Deserialize(reader);
                if (parsedData != null && ((IDictionary<string, JToken>)parsedData).ContainsKey(REPLAY_DATABLOCK_2))
                {
                    replay.datablock_battle_result_plain = parsedData[REPLAY_DATABLOCK_2][0].ToObject<PlayerResult>();
                    replay.datablock_1.vehicles = parsedData[REPLAY_DATABLOCK_2][1].ToObject<Dictionary<long, Domain.Replay.Vehicle>>();
                }
            }

            return replay;
        }

        /// <summary>
        /// Reads the replay statistic blocks.
        /// </summary>
        /// <param name="file">The replay file.</param>
        /// <returns></returns>
        public Replay ReadReplayStatisticBlocks(FileInfo file)
        {
            string path = file.FullName;
            string str = string.Empty;
            string str2 = string.Empty;
            if (File.Exists(path))
            {
                FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
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
                PlayerResult commandResult = null;
                BattleResult battleResult = null;

                if (str.Length > 0)
                {
                    firstBlock = JsonConvert.DeserializeObject<FirstBlock>(str);
                }

                if (firstBlock != null)
                {

                    try
                    {
                        var reader = new JsonTextReader(new StringReader(str2));
                        var se = new JsonSerializer();
                        var parsedData = (JArray) se.Deserialize(reader);
                        if (parsedData.Count > 0)
                        {
                            if (firstBlock.Version < JsonFormatedResultsMinVersion)
                            {
                                commandResult = parsedData[0].ToObject<PlayerResult>();
                            }
                            else
                            {
                                battleResult = parsedData[0].ToObject<BattleResult>();
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        _log.InfoFormat("Error on replay file read. Incorrect file format({0})", e,
                            file.FullName);
                    }

                    return new Replay
                    {
                        datablock_1 = firstBlock,
                        datablock_battle_result_plain = commandResult,
                        datablock_battle_result = battleResult
                    };
                }
            }
            return null;
        }

        /// <summary>
        /// Requests the specified method from wot api.
        /// </summary>
        /// <typeparam name="T">Return type</typeparam>
        /// <param name="method">The API method.</param>
        /// <param name="parameters">The method parameters.</param>
        /// <param name="settings">The app settings.</param>
        /// <returns></returns>
        /// <exception cref="ApiRequestException">Api request exception</exception>
        public T Request<T>(string method, Dictionary<string, object> parameters, AppSettings settings)
        {
            try
            {
                string url = string.Format(URL_API, settings.Server, AppConfigSettings.ApiVersion, method);
                WebRequest request = HttpWebRequest.Create(url);
                request.Proxy.Credentials = CredentialCache.DefaultCredentials;
                request.Method = WebRequestMethods.Http.Post;
                request.ContentType = CONTENT_TYPE;
                request.Timeout = 5000;
                string queryParameters = string.Join("&", parameters.Select(x => string.Format("{0}={1}", x.Key, x.Value)));
                byte[] encodedBytes = Encoding.GetEncoding("utf-8").GetBytes(queryParameters);
                request.ContentLength = encodedBytes.Length;
                Stream newStream = request.GetRequestStream();
                newStream.Write(encodedBytes, 0, encodedBytes.Length);
                newStream.Close();

                WebResponse response = request.GetResponse();
                using (Stream stream = response.GetResponseStream())
                {
                    var streamReader = new StreamReader(stream);
                    var reader = new JsonTextReader(streamReader);
                    var se = new JsonSerializer();
                    return se.Deserialize<T>(reader);
                }
            }
            catch (Exception e)
            {
                _log.ErrorFormat("Error on execute api method - {0}", e, method);
                throw new ApiRequestException("Api request exception", e);
            }
        }
    }
}
