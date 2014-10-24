using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Common.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WotDossier.Common;
using WotDossier.Domain;
using System.Linq;
using WotDossier.Domain.Tank;

namespace WotDossier.Dal
{
    /// <summary>
    /// Common app dictionaries
    /// </summary>
    public class Dictionaries
    {
        private static readonly ILog _log = LogManager.GetCurrentClassLogger();

        private static readonly object _syncObject = new object();
        private static volatile Dictionaries _instance = new Dictionaries();

        private readonly Dictionary<int, TankDescription> _tanks;
        private readonly Dictionary<string, TankIcon> _icons = new Dictionary<string, TankIcon>();
        private readonly Dictionary<TankIcon, TankDescription> _iconTanks = new Dictionary<TankIcon, TankDescription>();
        private readonly Dictionary<string, Map> _maps = new Dictionary<string, Map>();
        private readonly Dictionary<int, TankServerInfo> _serverTanks;
        private readonly Dictionary<string, RatingExpectancy> _ratingExpectations;

        public static readonly Version VersionAll = new Version("0.0.0.0");
        public static readonly Version VersionRelease = new Version("0.9.4.0");
        public static readonly Version VersionTest = new Version("100.0.0.0");

        private static readonly List<Version> _versions = new List<Version>
        {
                VersionRelease,
                new Version("0.9.3.0"),
                new Version("0.9.2.0"),
                new Version("0.9.1.0"),
                new Version("0.9.0.0"),
                new Version("0.8.11.0"), 
                new Version("0.8.10.0"),
                new Version("0.8.9.0"),
                new Version("0.8.8.0"),
                new Version("0.8.7.0"),
                new Version("0.8.6.0"),
                new Version("0.8.5.0"),
                new Version("0.8.4.0"),
                new Version("0.8.3.0"),
                new Version("0.8.2.0"),
                new Version("0.8.1.0"),
        };

        #region BattleLevels

        /// <summary>
        /// http://forum.worldoftanks.ru/index.php?/topic/41221-
        /// </summary>
        private readonly Dictionary<int, Dictionary<TankType, LevelRange>> _tankLevelsMap = new Dictionary<int, Dictionary<TankType, LevelRange>>
        {
            {
                1, new Dictionary<TankType, LevelRange>
                {
                    {TankType.LT, new LevelRange {Min = 1, Max = 2}},
                    {TankType.MT, new LevelRange {Min = 1, Max = 2}},
                }
            },
            {
                2, new Dictionary<TankType, LevelRange>
                {
                    {TankType.LT, new LevelRange{Min = 2, Max = 3}},
                    {TankType.MT, new LevelRange{Min = 2, Max = 3}},
                    {TankType.SPG, new LevelRange{Min = 2, Max = 3}},
                    {TankType.TD, new LevelRange{Min = 2, Max = 3}},
                }
            },
            {
                3, new Dictionary<TankType, LevelRange>
                {
                    {TankType.LT, new LevelRange{Min = 3, Max = 5}},
                    {TankType.MT, new LevelRange{Min = 3, Max = 5}},
                    {TankType.SPG, new LevelRange{Min = 3, Max = 5}},
                    {TankType.TD, new LevelRange{Min = 3, Max = 5}},
                }
            },
            {
                4, new Dictionary<TankType, LevelRange>
                {
                    {TankType.LT, new LevelRange{Min = 4, Max = 8}},
                    {TankType.MT, new LevelRange{Min = 4, Max = 6}},
                    {TankType.HT, new LevelRange{Min = 4, Max = 5}},
                    {TankType.SPG, new LevelRange{Min = 4, Max = 6}},
                    {TankType.TD, new LevelRange{Min = 4, Max = 6}},
                }
            },
            {
                5, new Dictionary<TankType, LevelRange>
                {
                    {TankType.LT, new LevelRange{Min = 6, Max = 9}},
                    {TankType.MT, new LevelRange{Min = 5, Max = 7}},
                    {TankType.HT, new LevelRange{Min = 5, Max = 7}},
                    {TankType.SPG, new LevelRange{Min = 5, Max = 7}},
                    {TankType.TD, new LevelRange{Min = 5, Max = 7}},
                }
            },
            {
                6, new Dictionary<TankType, LevelRange>
                {
                    {TankType.LT, new LevelRange{Min = 7, Max = 10}},
                    {TankType.MT, new LevelRange{Min = 6, Max = 8}},
                    {TankType.HT, new LevelRange{Min = 6, Max = 8}},
                    {TankType.SPG, new LevelRange{Min = 6, Max = 8}},
                    {TankType.TD, new LevelRange{Min = 6, Max = 8}},
                }
            },
            {
                7, new Dictionary<TankType, LevelRange>
                {
                    {TankType.LT, new LevelRange{Min = 8, Max = 11}},
                    {TankType.MT, new LevelRange{Min = 7, Max = 9}},
                    {TankType.HT, new LevelRange{Min = 7, Max = 9}},
                    {TankType.SPG, new LevelRange{Min = 7, Max = 9}},
                    {TankType.TD, new LevelRange{Min = 7, Max = 9}},
                }
            },
            {
                8, new Dictionary<TankType, LevelRange>
                {
                    {TankType.LT, new LevelRange{Min = 9, Max = 11}},
                    {TankType.MT, new LevelRange{Min = 8, Max = 10}},
                    {TankType.HT, new LevelRange{Min = 8, Max = 10}},
                    {TankType.SPG, new LevelRange{Min = 8, Max = 10}},
                    {TankType.TD, new LevelRange{Min = 8, Max = 10}},
                }
            },
            {
                9, new Dictionary<TankType, LevelRange>
                {
                    {TankType.MT, new LevelRange{Min = 9, Max = 11}},
                    {TankType.HT, new LevelRange{Min = 9, Max = 11}},
                    {TankType.SPG, new LevelRange{Min = 9, Max = 11}},
                    {TankType.TD, new LevelRange{Min = 9, Max = 11}},
                }
            },
            {
                10, new Dictionary<TankType, LevelRange>
                {
                    {TankType.MT, new LevelRange{Min = 10, Max = 11}},
                    {TankType.HT, new LevelRange{Min = 10, Max = 11}},
                    {TankType.SPG, new LevelRange{Min = 10, Max = 11}},
                    {TankType.TD, new LevelRange{Min = 10, Max = 11}},
                }
            },
        };

        #endregion

        public List<Version> Versions
        {
            get { return _versions; }
        }

        /// <summary>
        /// Tanks dictionary
        /// KEY - tankid, countryid
        /// </summary>
        public Dictionary<int, TankDescription> Tanks
        {
            get { return _tanks; }
        }

        /// <summary>
        /// Gets the tanks icons.
        /// </summary>
        public Dictionary<string, TankIcon> Icons
        {
            get { return _icons; }
        }

        /// <summary>
        /// Gets the tanks icons.
        /// </summary>
        public Dictionary<TankIcon, TankDescription> IconTanks
        {
            get { return _iconTanks; }
        }

        /// <summary>
        /// Gets the server tanks info.
        /// </summary>
        public Dictionary<int, TankServerInfo> ServerTanks
        {
            get { return _serverTanks; }
        }

        /// <summary>
        /// Gets the maps.
        /// </summary>
        public Dictionary<string, Map> Maps
        {
            get { return _maps; }
        }

        private readonly List<int> _notExistsedTanksList = new List<int>
                {
                    226,//t62a_sport
                    10234,//Karl
                    30251,//T-34-1 training
                    255,//Spectator
                    10226,//pziii_training
                    10227,//pzvib_tiger_ii_training
                    10228,//pzv_training
                    10254,//env
                    220,//t_34_85_training
                    20212,//m4a3e8_sherman_training
                    5,//KV
                    20009,//T23
                    30002,//Type 59 G
                    20211,//sexton_i
                    20255,//M24 Chaffee GT
                    30003,//WZ-111
                };

        /// <summary>
        /// Gets the not existsed tanks list.
        /// </summary>
        /// <value>
        /// The not existsed tanks list.
        /// </value>
        public List<int> NotExistsedTanksList
        {
            get { return _notExistsedTanksList; }
        }

        /// <summary>
        /// The game servers
        /// </summary>
        public readonly Dictionary<string, string> GameServers = new Dictionary<string, string>
        {
            {"ru", "worldoftanks.net"},
            {"eu", "worldoftanks.eu"},
            {"us", "worldoftanks.com"},
            {"kr", "worldoftanks.kr"},
            {"asia", "worldoftanks.asia"},
        };

        

        public Dictionary<int, DeviceDescription> DeviceDescriptions { get; set; }
        public Dictionary<int, ConsumableDescription> ConsumableDescriptions { get; set; }

        /// <summary>
        /// Gets or sets the medals dictionary.
        /// </summary>
        /// <value>
        /// The medals.
        /// </value>
        public Dictionary<int, Medal> Medals { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        private Dictionaries()
        {
            _ratingExpectations = ReadRatingExpectationsDictionary();
            _tanks = ReadTanksDictionary();
            _serverTanks = ReadServerTanksDictionary();
            _maps = ReadMaps();
            Medals = ReadMedals();

            DeviceDescriptions = ReadDeviceDescriptions();
            ConsumableDescriptions = ReadConsumableDescriptions();
            Shells = ReadShellsDescriptions();
        }

        private Dictionary<Country, Dictionary<int, ShellDescription>> ReadShellsDescriptions()
        {
            Dictionary<Country, Dictionary<int, ShellDescription>> result = new Dictionary<Country, Dictionary<int, ShellDescription>>();
            foreach (Country country in Enum.GetValues(typeof(Country)))
            {
                string file = string.Format(@"External\shells\{0}_shells.json", country);
                if (File.Exists(file))
                {
                    using (StreamReader re = new StreamReader(file))
                    {
                        JsonTextReader reader = new JsonTextReader(re);
                        JsonSerializer se = new JsonSerializer();
                        JObject parsedData = se.Deserialize<JObject>(reader);
                        var readDeviceDescriptions = parsedData.ToObject<Dictionary<string, ShellDescription>>();
                        result.Add(country, readDeviceDescriptions.Values.Where(x => x.id != null).ToDictionary(x => x.id.Value, y => y));
                    }
                }
            }
            return result;
        }

        public Dictionary<Country, Dictionary<int, ShellDescription>> Shells { get; set; }

        private Dictionary<int, DeviceDescription> ReadDeviceDescriptions()
        {
            using (StreamReader re = new StreamReader(@"External\optional_devices.json"))
            {
                JsonTextReader reader = new JsonTextReader(re);
                JsonSerializer se = new JsonSerializer();
                JObject parsedData = se.Deserialize<JObject>(reader);
                var readDeviceDescriptions = parsedData.ToObject<Dictionary<string, DeviceDescription>>();
                return readDeviceDescriptions.Values.Where(x => x.id != null).ToDictionary(x => x.id.Value, y => y);
            }
        }

        private Dictionary<int, ConsumableDescription> ReadConsumableDescriptions()
        {
            using (StreamReader re = new StreamReader(@"External\consumables.json"))
            {
                JsonTextReader reader = new JsonTextReader(re);
                JsonSerializer se = new JsonSerializer();
                JObject parsedData = se.Deserialize<JObject>(reader);
                var readDeviceDescriptions = parsedData.ToObject<Dictionary<string, ConsumableDescription>>();
                return readDeviceDescriptions.Values.Where(x => x.id != null).ToDictionary(x => x.id.Value, y => y);
            }
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        public static Dictionaries Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_syncObject)
                    {
                        if (_instance == null)
                        {
                            _instance = new Dictionaries();
                        }
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Gets the tank icon.
        /// </summary>
        /// <param name="playerVehicle">The player vehicle.</param>
        /// <param name="clientVersion"></param>
        /// <returns></returns>
        public TankDescription GetReplayTankDescription(string playerVehicle, Version clientVersion)
        {
            string iconId = playerVehicle.Replace(":", "_").Replace("-", "_").Replace(" ", "_").Replace(".", "_").ToLower();

            if (Icons.ContainsKey(iconId))
            {
                TankIcon tankIcon = Icons[iconId];

                if (IconTanks.ContainsKey(tankIcon))
                {
                    var tankDescription = IconTanks[tankIcon];

                    //t49 renamed to t67 in 9.3
                    if (tankDescription.UniqueId() == 20071 && clientVersion < new Version("0.9.3.0"))
                    {
                        return _tanks[20041];
                    }
                    //kv-1s renamed to kv-85 in 9.3
                    if (tankDescription.UniqueId() == 73 && clientVersion < new Version("0.9.3.0"))
                    {
                        return _tanks[11];
                    }

                    return tankDescription;
                }
            }

            return null;
        }

        public TankDescription GetTankDescription(int? typeCompDescr)
        {
            if (typeCompDescr == null)
            {
                return new TankDescription{Title = "Unknown", Icon = TankIcon.Empty};
            }

            int tankId = typeCompDescr.Value >> 8 & 65535;
            int countryId = typeCompDescr.Value >> 4 & 15;

            var uniqueId = Utils.ToUniqueId(countryId, tankId);

            return Tanks[uniqueId];
        }

        private Dictionary<int, TankDescription> ReadTanksDictionary()
        {
            List<TankDescription> tanks = new List<TankDescription>();
            using (StreamReader re = new StreamReader(@"External\tanks.json"))
            {
                JsonTextReader reader = new JsonTextReader(re);
                JsonSerializer se = new JsonSerializer();
                JArray parsedData = se.Deserialize<JArray>(reader);
                foreach (JToken jToken in parsedData)
                {
                    TankDescription tank = jToken.ToObject<TankDescription>();

                    TankIcon icon = jToken.ToObject<TankIcon>();

                    _icons.Add(icon.IconId.ToLower(), icon);

                    _iconTanks.Add(icon, tank);

                    tank.Icon = icon;

                    if (_tankLevelsMap.ContainsKey(tank.Tier) && _tankLevelsMap[tank.Tier].ContainsKey((TankType)tank.Type))
                    {
                        tank.LevelRange = _tankLevelsMap[tank.Tier][(TankType) tank.Type];
                    }
                    else
                    {
                        tank.LevelRange = LevelRange.All;
                    }

                    var key = tank.Icon.IconOrig.ToLower();
                    if (_ratingExpectations.ContainsKey(key))
                    {
                        tank.Expectancy = _ratingExpectations[key];
                    }
                    else
                    {
                        tank.Expectancy = GetNearestExpectationsByTypeAndLevel(tank);
                    }

                    tanks.Add(tank);
                }
            }

            return tanks.ToDictionary(x => x.UniqueId());
        }

        private RatingExpectancy GetNearestExpectationsByTypeAndLevel(TankDescription tank)
        {
            return _ratingExpectations.Values.FirstOrDefault(x => x.TankLevel == tank.Tier && (int) x.TankType == tank.Type);
        }

        private Dictionary<int, TankServerInfo> ReadServerTanksDictionary()
        {
            using (StreamReader re = new StreamReader(@"External\server_tanks.json"))
            {
                JsonTextReader reader = new JsonTextReader(re);
                JsonSerializer se = new JsonSerializer();
                JObject parsedData = se.Deserialize<JObject>(reader);
                return parsedData["data"].ToObject<Dictionary<int, TankServerInfo>>();
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
                    JArray parsedData = se.Deserialize<JArray>(reader);
                    return parsedData.ToObject<List<RatingExpectancy>>().ToDictionary(x => x.Icon.ToLower(), x => x);
                }
            }
            catch (Exception e)
            {
                _log.Error(e);
            }
            return new Dictionary<string, RatingExpectancy>();
        }

        /// <summary>
        /// Reads the maps.
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, Map> ReadMaps()
        {
            List<Map> maps;
            using (StreamReader re = new StreamReader(@"External\maps.json"))
            {
                JsonTextReader reader = new JsonTextReader(re);
                JsonSerializer se = new JsonSerializer();
                maps = se.Deserialize<List<Map>>(reader);
            }

            List<Map> list = (maps ?? new List<Map>()).Where(x => x.MapNameId != "00_tank_tutorial").ToList();
            int i = 1;
            list.ForEach(x => x.LocalizedMapName = Resources.Resources.ResourceManager.GetString("Map_" + x.MapNameId) ?? x.MapName);
            list = list.OrderByDescending(x => x.LocalizedMapName).ToList();
            list.ForEach(x => x.MapId = i++);
            return list.ToDictionary(x => x.MapNameId, y => y);
        }

        public int GetBattleLevel(List<LevelRange> members)
        {
            int max = members.Max(x => x.Max);

            int level = -1;

            for (int i = max; i > 0; i--)
            {
                if (members.All(x => x.Min <= i && x.Max >= i))
                {
                    level = i;
                    break;
                }
            }

            return level;
        }

        /*
         218-max damage
         2-max xp
         */

        /// <summary>
        /// Gets the medals by identifiers.
        /// </summary>
        /// <param name="achievements">The achievements.</param>
        /// <returns></returns>
        public List<Medal> GetMedals(List<int> achievements)
        {
            List<Medal> list = new List<Medal>();

            foreach (int achievement in achievements)
            {
                if (Medals.ContainsKey(achievement))
                {
                    list.Add(Medals[achievement]);
                }
            }
            return list;
        }

        /// <summary>
        /// Reads the medals.
        /// </summary>
        /// <returns></returns>
        public static Dictionary<int, Medal> ReadMedals()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(File.OpenRead(Path.Combine(Environment.CurrentDirectory, @"Data\Medals.xml")));

            XmlNodeList nodes = doc.SelectNodes("Medals/node()/medal");

            Dictionary<int, Medal> medals = new Dictionary<int, Medal>();

            foreach (XmlNode node in nodes)
            {
                Medal medal = new Medal();
                medal.Id = Convert.ToInt32(node.Attributes["id"].Value);
                medal.Name = node.Attributes["name"].Value;
                medal.Icon = node.Attributes["icon"].Value;
                medal.Type = int.Parse(node.Attributes["type"].Value);
                medals.Add(medal.Id, medal);
            }

            return medals;
        }

        /// <summary>
        /// Gets the achiev medals.
        /// </summary>
        /// <param name="dossierPopUps">The dossier pop ups.</param>
        /// <returns></returns>
        public List<Medal> GetAchievMedals(List<List<JValue>> dossierPopUps)
        {
            List<Medal> list = new List<Medal>();

            foreach (List<JValue> achievement in dossierPopUps)
            {
                int id = achievement[0].Value<int>();
                int value = 0;
                if (achievement[1].Type == JTokenType.Integer)
                {
                    value = achievement[1].Value<int>();
                }
                else
                {
                    value = -1;
                }

                int exId = id * 100 + value;

                if (Medals.ContainsKey(id))
                {
                    list.Add(Medals[id]);
                }
                else if (Medals.ContainsKey(exId))
                {
                    list.Add(Medals[exId]);
                }
            }
            return list;
        }
    }
}
