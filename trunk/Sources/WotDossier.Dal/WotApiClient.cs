﻿using System;
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
using WotDossier.Domain.Dossier.AppSpot;
using WotDossier.Domain.Dossier.TankV29;
using WotDossier.Domain.Dossier.TankV65;
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

        private const string URL_API = @"https://api.worldoftanks.{0}/{1}/{2}";
        private const string REPLAY_DATABLOCK_2 = "datablock_2";
        private const string CONTENT_TYPE = "application/x-www-form-urlencoded";
        public const string PARAM_APPID = "application_id";
        public const string PARAM_SEARCH = "search";
        public const string PARAM_ACCOUNT_ID = "account_id";
        public const string PARAM_CLAN_ID = "clan_id";
        public const string PARAM_FIELDS = "fields";
        public const string PARAM_LIMIT = "limit";

        private static readonly object _syncObject = new object();
        private static volatile WotApiClient _instance = new WotApiClient();

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        private WotApiClient()
        {
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

        public List<TankJson> ReadDossierAppSpotTanks(string data)
        {
            List<TankJson> tanks = new List<TankJson>();

            JObject parsedData = JsonConvert.DeserializeObject<JObject>(data);

            JToken tanksData = parsedData["tanks"];

            foreach (JToken jToken in tanksData)
            {
                JProperty property = (JProperty)jToken;
                string raw = property.Value.ToString();
                Tank appSpotTank = JsonConvert.DeserializeObject<Tank>(raw);
                TankJson tank = TankJsonV2Converter.Convert(appSpotTank);
                tank.Raw = WotApiHelper.Zip(JsonConvert.SerializeObject(tank));
                ExtendPropertiesData(tank);
                tanks.Add(tank);
            }

            return tanks;
        }

        public List<TankJson> ReadTanksV2(string path)
        {
            List<TankJson> tanks = new List<TankJson>();

            using (StreamReader re = new StreamReader(path))
            {
                JsonTextReader reader = new JsonTextReader(re);
                JsonSerializer se = new JsonSerializer();
                JObject parsedData = (JObject)se.Deserialize(reader);

                JToken tanksData = parsedData["tanks"];

                foreach (JToken jToken in tanksData)
                {
                    JProperty property = (JProperty)jToken;
                    string raw = property.Value.ToString();
                    TankJson29 tank29 = JsonConvert.DeserializeObject<TankJson29>(raw);
                    TankJson tank = TankJsonV2Converter.Convert(tank29);
                    tank.Raw = WotApiHelper.Zip(JsonConvert.SerializeObject(tank));
                    ExtendPropertiesData(tank);
                    tanks.Add(tank);
                }

                JToken tanksDataV2 = parsedData["tanks_v2"];

                foreach (JToken jToken in tanksDataV2)
                {
                    JProperty property = (JProperty)jToken;
                    string raw = property.Value.ToString();
                    TankJson65 tank65 = JsonConvert.DeserializeObject<TankJson65>(raw);
                    TankJson tank = TankJsonV2Converter.Convert(tank65);
                    tank.Raw = WotApiHelper.Zip(JsonConvert.SerializeObject(tank));
                    ExtendPropertiesData(tank);
                    tanks.Add(tank);
                }
            }
            return tanks;
        }

        public void ExtendPropertiesData(TankJson tank)
        {
            tank.Description = Dictionaries.Instance.TanksDictionary[tank.UniqueId()];
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
                            Icon = Dictionaries.Instance.TanksDictionary[uniqueId].Icon,
                            TankUniqueId = uniqueId,
                            Count = Convert.ToInt32(x[2]),
                            Type = Dictionaries.Instance.TanksDictionary[uniqueId].Type,
                            Tier = Dictionaries.Instance.TanksDictionary[uniqueId].Tier,
                            KilledByTankUniqueId = tank.UniqueId(),
                            Tank = x[3]
                        };
                    }).ToList();
        }

        /// <summary>
        /// Loads player stat from server
        /// </summary>
        /// <exception cref="PlayerInfoLoadException"></exception>
        public PlayerStat LoadPlayerStat(AppSettings settings, int playerId)
        {
            return LoadPlayerStat(settings, playerId, true);
        }

        /// <summary>
        /// Loads player stat from server
        /// </summary>
        /// <exception cref="PlayerInfoLoadException"></exception>
        public PlayerStat LoadPlayerStat(AppSettings settings, int playerId, bool loadVehicles)
        {
            if (settings == null || string.IsNullOrEmpty(settings.Server))
            {
                return null;
            }

            try
            {
                var playerStat = Request<PlayerStat>(settings, "account/info/", new Dictionary<string, object>
                {
                    {PARAM_APPID, WotDossierSettings.GetAppId(settings.Server)},
                    {PARAM_ACCOUNT_ID, playerId},
                });
                playerStat.dataField = playerStat.data[playerId];
                playerStat.dataField.ratings = GetPlayerRatings(settings, playerId);
                if (loadVehicles)
                {
                    playerStat.dataField.vehicles = GetPlayerTanks(settings, playerId);
                }
                if (playerStat.dataField.clan != null)
                {
                    playerStat.dataField.clanData = LoadClan(settings, playerStat.dataField.clan.clan_id,
                        new[] {"abbreviation", "name", "clan_id", "description", "emblems"});
                }
                return playerStat;
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
                JObject parsedData = Request<JObject>(settings, "account/tanks/", new Dictionary<string, object>
                {
                    {PARAM_APPID, WotDossierSettings.GetAppId(settings.Server)},
                    {PARAM_ACCOUNT_ID, playerId},
                });

                if (parsedData["data"].Any())
                {
                    List<VehicleStat> tanks = JsonConvert.DeserializeObject<List<VehicleStat>>(parsedData["data"][playerId.ToString()].ToString());
                    foreach (VehicleStat tank in tanks)
                    {
                        if (Dictionaries.Instance.ServerTanksDictionary.ContainsKey(tank.tank_id))
                        {
                            tank.tank = Dictionaries.Instance.ServerTanksDictionary[tank.tank_id];
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

        private Ratings GetPlayerRatings(AppSettings settings, int playerId)
        {
            try
            {
                JObject parsedData = Request<JObject>(settings, "account/ratings/", new Dictionary<string, object>
                {
                    {PARAM_APPID, WotDossierSettings.GetAppId(settings.Server)},
                    {PARAM_ACCOUNT_ID, playerId},
                });

                if (parsedData["data"].Any())
                {
                    return JsonConvert.DeserializeObject<Ratings>(parsedData["data"][playerId.ToString()].ToString());
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
            return LoadClan(settings, clanId, null);
        }

        /// <summary>
        /// Loads player stat from server
        /// </summary>
        /// <exception cref="PlayerInfoLoadException"></exception>
        public ClanData LoadClan(AppSettings settings, int clanId, string[] fields)
        {
            if (settings == null || string.IsNullOrEmpty(settings.Server))
            {
                return null;
            }

            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>
                {
                    {PARAM_APPID, WotDossierSettings.GetAppId(settings.Server)},
                    {PARAM_CLAN_ID, clanId},
                };

                if (fields != null)
                {
                    dictionary.Add(PARAM_FIELDS, string.Join(",", fields));
                }

                JObject parsedData = Request<JObject>(settings, "clan/info/", dictionary);
                return JsonConvert.DeserializeObject<ClanData>(parsedData["data"][clanId.ToString()].ToString());
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
            List<PlayerSearchJson> list = SearchPlayer(settings, playerName, 1);
            if (list != null)
            {
                return list.FirstOrDefault();
            }
            return null;
        }

        /// <summary>
        /// Searches the player.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="playerName">Name of the player.</param>
        /// <param name="limit">The limit.</param>
        /// <returns>
        /// Found players
        /// </returns>
        public List<PlayerSearchJson> SearchPlayer(AppSettings settings, string playerName, int limit)
        {
#if DEBUG
            return new List<PlayerSearchJson> {new PlayerSearchJson {id = 10800699, nickname = "rembel"}};
#else
            try
            {
                JObject parsedData = Request<JObject>(settings, "account/list/", new Dictionary<string, object>
                {
                    {PARAM_APPID, WotDossierSettings.GetAppId(settings.Server)},
                    {PARAM_SEARCH, playerName},
                    {PARAM_LIMIT, limit},
                });
                return JsonConvert.DeserializeObject<List<PlayerSearchJson>>(parsedData["data"].ToString());
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
        /// <param name="settings">The settings.</param>
        /// <param name="clanName">Name of the clan.</param>
        /// <param name="count">The count.</param>
        /// <returns>Found clans</returns>
        public List<ClanSearchJson> SearchClan(AppSettings settings, string clanName, int count)
        {
            try
            {
                JObject parsedData = Request<JObject>(settings, "clan/list/", new Dictionary<string, object>
                {
                    {PARAM_APPID, WotDossierSettings.GetAppId(settings.Server)},
                    {PARAM_SEARCH, clanName},
                    {PARAM_LIMIT, count}
                });

                if (parsedData["status"].ToString() != "error" && parsedData["data"].Any())
                {
                    return JsonConvert.DeserializeObject<List<ClanSearchJson>>(parsedData["data"].ToString());
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
                    return new Replay { datablock_1 = firstBlock, CommandResult = commandResult };
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

        public T Request<T>(AppSettings settings, string method, Dictionary<string, object> parameters)
        {
            try
            {
                string url = string.Format(URL_API, settings.Server, WotDossierSettings.ApiVersion, method);
                WebRequest request = HttpWebRequest.Create(url);
                request.Proxy.Credentials = CredentialCache.DefaultCredentials;
                request.Method = WebRequestMethods.Http.Post;
                request.ContentType = CONTENT_TYPE;
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
