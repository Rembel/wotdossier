using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using Common.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WotDossier.Domain;
using System.Linq;
using WotDossier.Domain.Server;
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
        private static readonly ILog _log = LogManager.GetCurrentClassLogger();

        private const string URL_API = @"https://api.worldoftanks.{0}/{1}";
        private const string CONTENT_TYPE = "application/x-www-form-urlencoded";
        private const string PARAM_APPID = "application_id";
        private const string PARAM_SEARCH = "search";
        private const string PARAM_MAP_ID = "map_id";
        private const string PARAM_PROVINCE_ID = "province_id";
        private const string PARAM_ACCOUNT_ID = "account_id";
        private const string PARAM_IN_GARAGE = "in_garage";
        private const string PARAM_MEMBER_ID = "member_id";
        private const string PARAM_TYPE = "type";
        private const string PARAM_CLAN_ID = "clan_id";
        private const string PARAM_FIELDS = "fields";
        private const string PARAM_LIMIT = "limit";
        private const string METHOD_ACCOUNT_INFO = "wot/account/info/";
        private const string METHOD_TANKS_STATS = "wot/tanks/stats/";
        private const string METHOD_TANKS_ACHIEVEMENTS = "wot/tanks/achievements/";
        private const string METHOD_ACCOUNT_TANKS = "wot/account/tanks/";
        private const string METHOD_ACCOUNT_ACHIEVEMENTS = "wot/account/achievements/";
        private const string METHOD_RATINGS_ACCOUNTS = "wot/ratings/accounts/";
        private const string METHOD_CLAN_INFO = "wgn/clans/info/";
        private const string METHOD_CLAN_MEMBERSINFO = "wgn/clans/membersinfo/";
        private const string METHOD_GLOBALWAR_FRONTS = "wot/globalmap/fronts/";
        private const string METHOD_ACCOUNT_LIST = "wot/account/list/";
        private const string METHOD_CLAN_LIST = "wgn/clans/list/";
        private const string METHOD_GLOBALWAR_BATTLES = "wot/globalmap/clanbattles/";
        private const string METHOD_GLOBALWAR_PROVINCES = "wot/globalmap/provinceinfo/";

        private static readonly object _syncObject = new object();
        private static volatile WotApiClient _instance = new WotApiClient();

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        private WotApiClient()
        {
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
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
        /// Loads player stat from server
        /// </summary>
        /// <param name="playerId">The player identifier.</param>
        /// <param name="settings">The settings.</param>
        /// <param name="options">The statictic load options.</param>
        /// <param name="fields">The profile fields.</param>
        /// <returns></returns>
        /// <exception cref="PlayerInfoLoadException"></exception>
        public Player LoadPlayerStat(int playerId, AppSettings settings, PlayerStatLoadOptions options, string[] fields = null)
        {
            if (settings == null || string.IsNullOrEmpty(settings.Server))
            {
                return null;
            }

            Player response = null;
            try
            {
                var parameters = new Dictionary<string, object>
                {
                    {PARAM_APPID, AppConfigSettings.GetAppId(settings.Server)},
                    {PARAM_ACCOUNT_ID, playerId},
                };

                if (fields != null)
                {
                    parameters.Add(PARAM_FIELDS, string.Join(",", fields));
                }

                response = Request<Player>(METHOD_ACCOUNT_INFO, parameters, settings);
                response.server = settings.Server;
                response.dataField = response.data[playerId];
                
                if ((options & PlayerStatLoadOptions.LoadRatings) == PlayerStatLoadOptions.LoadRatings)
                {
                    response.dataField.ratings = GetPlayerRatings(playerId, settings);
                }
                if ((options & PlayerStatLoadOptions.LoadAchievments) == PlayerStatLoadOptions.LoadAchievments)
                {
                    response.dataField.achievements = GetPlayerAchievements(playerId, settings);
                }
                if ((options & PlayerStatLoadOptions.LoadVehicles) == PlayerStatLoadOptions.LoadVehicles)
                {
                    response.dataField.vehicles = GetPlayerTanks(playerId, settings);
                }

                return response;
            }
            catch (Exception e)
            {
                _log.ErrorFormat("Can't get player[{1}:{2}] info from server: \n{0}", e, response, settings.Server, playerId);
                return null;
            }
        }

        /// <summary>
        /// Gets the clan member information.
        /// </summary>
        /// <param name="playerId">The player identifier.</param>
        /// <param name="settings">The settings.</param>
        /// <returns></returns>
        public ClanMemberInfo GetClanMemberInfo(int playerId, AppSettings settings)
        {
            JObject response = null;

            try
            {
                response = Request<JObject>(METHOD_CLAN_MEMBERSINFO, new Dictionary<string, object>
                {
                    {PARAM_APPID, AppConfigSettings.GetAppId(settings.Server)},
                    {PARAM_ACCOUNT_ID, playerId},
                }, settings);

                if (response["data"].Any())
                {
                    var clanMemberInfo = response["data"][playerId.ToString(CultureInfo.InvariantCulture)].ToObject<ClanMemberInfo>();

                    if (clanMemberInfo != null && clanMemberInfo.clan.clan_id > 0)
                    {
                        clanMemberInfo.clan = LoadClan(clanMemberInfo.clan.clan_id,
                            new[] {"tag", "name", "clan_id", "description", "emblems"}, settings);

                        IEnumerable<GlobalMapFront> mapIds = GetFronts(settings);

                        List<BattleJson> result = new List<BattleJson>();
                        result.AddRange(GetBattles(clanMemberInfo.clan.clan_id, settings));
                        clanMemberInfo.clan.Battles = result;

                        return clanMemberInfo;
                    }
                }
            }
            catch (Exception e)
            {
                _log.ErrorFormat("Error on clan member[{1}:{2}] info loading: \n{0}", e, response, settings.Server, playerId);
            }

            return null;
        }

        private IEnumerable<GlobalMapFront> GetFronts(AppSettings settings)
        {
            JObject response = null;

            try
            {
                response = Request<JObject>(METHOD_GLOBALWAR_FRONTS, new Dictionary<string, object>
                {
                    {PARAM_APPID, AppConfigSettings.GetAppId(settings.Server)},
                }, settings);

                if (response["data"].Any())
                {
                    return response["data"].Children().Select(x => x.ToObject<GlobalMapFront>()).ToArray();
                }
            }
            catch (Exception e)
            {
                _log.ErrorFormat("Error on Maps info loading for server {1}: \n{0}", e, response, settings.Server);
            }
            return new GlobalMapFront[0];
        }

        /// <summary>
        /// Gets the player tanks.
        /// </summary>
        /// <param name="playerId">The player identifier.</param>
        /// <param name="settings">The settings.</param>
        /// <returns></returns>
        private List<Vehicle> GetPlayerTanks(int playerId, AppSettings settings)
        {
            JObject response = null;
            try
            {
                response = Request<JObject>(METHOD_TANKS_STATS, new Dictionary<string, object>
                {
                    {PARAM_APPID, AppConfigSettings.GetAppId(settings.Server)},
                    {PARAM_ACCOUNT_ID, playerId},
                    //{PARAM_IN_GARAGE, 1},
                }, settings);

                if (response["data"].Any())
                {
                    var achievements = GetPlayerTanksAchievements(playerId, settings);

                    List<Vehicle> tanks = response["data"][playerId.ToString(CultureInfo.InvariantCulture)].ToObject<List<Vehicle>>();
                    if (tanks != null)
                    {
                        foreach (Vehicle tank in tanks)
                        {
                            tank.description =
                                Dictionaries.Instance.Tanks.Values.FirstOrDefault(x => x.CompDescr == tank.tank_id);
                            var vehicleAchievements = achievements.FirstOrDefault(x => x.tank_id == tank.tank_id) ??
                                                      new VehicleAchievements {achievements = new MedalAchievements()};
                            tank.achievements = vehicleAchievements.achievements;

                            if (tank.description == null)
                            {
                                _log.WarnFormat(
                                    "Unknown tank id found [{0}] on get player[{1}:{2}] server tank statistic",
                                    tank.tank_id, settings.Server, playerId);
                            }
                        }
                    }

                    return tanks;
                }
            }
            catch (Exception e)
            {
                _log.ErrorFormat("Error on player[{1}:{2}] tanks loading: \n{0}", e, response, settings.Server, playerId);
            }

            return new List<Vehicle>();
        }

        /// <summary>
        /// Gets the player tanks achievements.
        /// </summary>
        /// <param name="playerId">The player identifier.</param>
        /// <param name="settings">The settings.</param>
        /// <returns></returns>
        private List<VehicleAchievements> GetPlayerTanksAchievements(int playerId, AppSettings settings)
        {
            JObject response = null;
            try
            {
                response = Request<JObject>(METHOD_TANKS_ACHIEVEMENTS, new Dictionary<string, object>
                {
                    {PARAM_APPID, AppConfigSettings.GetAppId(settings.Server)},
                    {PARAM_ACCOUNT_ID, playerId},
                    {PARAM_FIELDS, "achievements,tank_id"},
                }, settings);

                if (response["data"].Any())
                {
                    return response["data"][playerId.ToString(CultureInfo.InvariantCulture)].ToObject<List<VehicleAchievements>>();
                }
            }
            catch (Exception e)
            {
                _log.ErrorFormat("Error on player[{1}:{2}] tanks achievements loading: \n{0}", e, response, settings.Server, playerId);
            }

            return new List<VehicleAchievements>();
        }

        private Ratings GetPlayerRatings(int playerId, AppSettings settings)
        {
            JObject response = null;
            try
            {
                response = Request<JObject>(METHOD_RATINGS_ACCOUNTS, new Dictionary<string, object>
                {
                    {PARAM_APPID, AppConfigSettings.GetAppId(settings.Server)},
                    {PARAM_ACCOUNT_ID, playerId},
                    {PARAM_TYPE, "all"},
                }, settings);

                if (response["data"].Any())
                {
                    return response["data"][playerId.ToString(CultureInfo.InvariantCulture)].ToObject<Ratings>();
                }
            }
            catch (Exception e)
            {
                _log.ErrorFormat("Error on get player[{1}:{2}] ratings: \n{0}", e, response, settings.Server, playerId);
            }

            return null;
        }

        private Achievements GetPlayerAchievements(int playerId, AppSettings settings)
        {
            JObject response = null;
            try
            {
                response = Request<JObject>(METHOD_ACCOUNT_ACHIEVEMENTS, new Dictionary<string, object>
                {
                    {PARAM_APPID, AppConfigSettings.GetAppId(settings.Server)},
                    {PARAM_ACCOUNT_ID, playerId}
                }, settings);

                if (response["data"].Any())
                {
                    return response["data"][playerId.ToString(CultureInfo.InvariantCulture)].ToObject<Achievements>();
                }
            }
            catch (Exception e)
            {
                _log.ErrorFormat("Error on get player[{1}:{2}] achievements: \n{0}", e, response, settings.Server, playerId);
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

            JObject response = null;
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

                response = Request<JObject>(METHOD_CLAN_INFO, dictionary, settings);
                var clanData = response["data"][clanId.ToString(CultureInfo.InvariantCulture)].ToObject<ClanData>();

                if (clanData.members != null)
                {
                    Dictionary<string, object> memberInfoParams = new Dictionary<string, object>
                    {
                        {PARAM_APPID, AppConfigSettings.GetAppId(settings.Server)},
                        {PARAM_FIELDS, "account_id,nickname"},
                        {PARAM_ACCOUNT_ID, string.Join(",", clanData.members.Select(x => x.account_id))},
                    };

                    var nicknames = Request<JObject>(METHOD_ACCOUNT_INFO, memberInfoParams, settings)["data"].ToObject<Dictionary<int, ClanMember>>();

                    foreach (var member in clanData.members)
                    {
                        member.nickname = nicknames[member.account_id].nickname;
                    }
                }

                return clanData;
            }
            catch (Exception e)
            {
                _log.ErrorFormat("Can't get clan[{1}:{2}] info from server: \n{0}", e, response, settings.Server, clanId);
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
            return new List<PlayerSearchJson> {new PlayerSearchJson {account_id = 10800699, nickname = "rembel"}};
#else
            JObject response = null;
            try
            {
                response = Request<JObject>(METHOD_ACCOUNT_LIST, new Dictionary<string, object>
                {
                    {PARAM_APPID, AppConfigSettings.GetAppId(settings.Server)},
                    {PARAM_SEARCH, playerName},
                    {PARAM_LIMIT, limit},
                }, settings);
                return response["data"].ToObject<List<PlayerSearchJson>>();
            }
            catch (Exception e)
            {
                _log.ErrorFormat("Error on player[{1}:{2}] search: \n{0}", e, response, settings.Server, playerName);
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
            JObject response = null;
            try
            {
                response = Request<JObject>(METHOD_CLAN_LIST, new Dictionary<string, object>
                {
                    {PARAM_APPID, AppConfigSettings.GetAppId(settings.Server)},
                    {PARAM_SEARCH, clanName},
                    {PARAM_LIMIT, count}
                }, settings);

                if (response["status"].ToString() != "error" && response["data"].Any())
                {
                    return response["data"].ToObject<List<ClanSearchJson>>();
                }
            }
            catch (Exception e)
            {
                _log.ErrorFormat("Error on clan[{1}:{2}] search: \n{0}", e, response, settings.Server, clanName);
            }
            return null;
        }

        /// <summary>
        /// Search clans.
        /// </summary>
        /// <param name="provinceIds">The province ids.</param>
        /// <param name="mapId">The map id.</param>
        /// <param name="settings">The settings.</param>
        /// <returns>
        /// Found clans
        /// </returns>
        public Dictionary<string, ProvinceSearchJson> GetProvinces(string[] provinceIds, string mapId, AppSettings settings)
        {
            JObject response = null;
            try
            {
                response = Request<JObject>(METHOD_GLOBALWAR_PROVINCES, new Dictionary<string, object>
                {
                    {PARAM_APPID, AppConfigSettings.GetAppId(settings.Server)},
                    {PARAM_MAP_ID, mapId},
                    {PARAM_PROVINCE_ID, string.Join(",", provinceIds)}
                }, settings);

                if (response["status"].ToString() != "error" && response["data"].Any())
                {
                    return response["data"].ToObject<Dictionary<string, ProvinceSearchJson>>();
                }
            }
            catch (Exception e)
            {
                _log.ErrorFormat("Error on get provinces: \n{0}", e, response);
            }
            return new Dictionary<string, ProvinceSearchJson>();
        }

        /// <summary>
        /// Search clans.
        /// </summary>
        /// <param name="clanId">The clan id.</param>
        /// <param name="mapId">The map id.</param>
        /// <param name="settings">The settings.</param>
        /// <returns>
        /// Found battles
        /// </returns>
        public List<BattleJson> GetBattles(int clanId, AppSettings settings)
        {
            JObject response = null;
            try
            {
                response = Request<JObject>(METHOD_GLOBALWAR_BATTLES, new Dictionary<string, object>
                {
                    {PARAM_APPID, AppConfigSettings.GetAppId(settings.Server)},
                    {PARAM_CLAN_ID, clanId}
                }, settings);

                if (response["status"].ToString() != "error" && response["data"].Any())
                {
                    var battles = response["data"][clanId.ToString()].ToObject<List<BattleJson>>();
                    return battles;
                }
            }
            catch (Exception e)
            {
                _log.ErrorFormat("Error on get battles: \n{0}", e, response);
            }
            return new List<BattleJson>();
        }

        private List<ProvinceSearchJson> GetProvinceDescriptions(string[] provinces, Dictionary<string, ProvinceSearchJson> provinceDescriptions)
        {
            var list = new List<ProvinceSearchJson>();

            foreach (var province in provinces)
            {
                if (provinceDescriptions.ContainsKey(province))
                {
                    list.Add(provinceDescriptions[province]);
                }
            }
            
            return list;
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
                string url = string.Format(URL_API, settings.Server, method);
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

    [Flags]
    public enum PlayerStatLoadOptions
    {
        /// <summary>
        /// Load common stat
        /// </summary>
        LoadCommon = 0,
        /// <summary>
        /// Load vehicles stat
        /// </summary>
        LoadVehicles = 1,
        /// <summary>
        /// Load achievments
        /// </summary>
        LoadAchievments = 2,
        /// <summary>
        /// Load ratings
        /// </summary>
        LoadRatings = 4
    }

}
