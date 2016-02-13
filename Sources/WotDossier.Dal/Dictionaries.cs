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
        private static readonly ILog _log = LogManager.GetLogger<Dictionaries>();

        private static readonly object _syncObject = new object();
        private static volatile Dictionaries _instance = new Dictionaries();

        private Dictionary<int, TankDescription> _tanks;
        private readonly Dictionary<string, TankIcon> _icons = new Dictionary<string, TankIcon>();
        private readonly Dictionary<TankIcon, TankDescription> _iconTanks = new Dictionary<TankIcon, TankDescription>();
        private Dictionary<string, Map> _maps = new Dictionary<string, Map>();
        private Dictionary<int, RatingExpectancy> _ratingExpectations;

        public static readonly Version VersionAll = new Version("100.0.0.0");
        public static readonly Version VersionRelease = new Version("0.9.14.0");
        public static readonly Version VersionTest = new Version("0.9.15.0");

        private static readonly List<Version> _versions = new List<Version>
        {
                VersionRelease,
                new Version("0.9.13.0"),
                new Version("0.9.12.0"),
                new Version("0.9.10.0"),
                new Version("0.9.9.0"),
                new Version("0.9.8.0"),
                new Version("0.9.7.0"),
                new Version("0.9.6.0"),
                new Version("0.9.5.0"),
                new Version("0.9.4.0"),
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
        /// Gets the maps.
        /// </summary>
        public Dictionary<string, Map> Maps
        {
            get { return _maps; }
        }

        private List<int> _notExistsedTanksList;

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
            Init();
        }

        public void Init()
        {
            _ratingExpectations = ReadRatingExpectationsDictionary();
            _tanks = ReadTanksDictionary();
            _maps = ReadMaps();
            Medals = ReadMedals();

            DeviceDescriptions = ReadDeviceDescriptions();
            ConsumableDescriptions = ReadConsumableDescriptions();
            Shells = ReadShellsDescriptions();
        }

        private static void UpdateMapsGeometry(Dictionary<string, Map> maps)
        {
            using (StreamReader re = new StreamReader(@"External\maps_description.json"))
            {
                JsonTextReader reader = new JsonTextReader(re);
                JsonSerializer se = new JsonSerializer();
                JArray array = se.Deserialize<JArray>(reader);

                foreach (var map in array)
                {
                    var key = map["name"].Value<string>().Replace("#arenas:", string.Empty).Replace("/name", string.Empty);

                    if (maps.ContainsKey(key))
                    {
                        var target = maps[key];

                        target.Config = JsonConvert.DeserializeObject<MapConfig>(map.ToString(), new MapConfigConverter());
                    }
                }
            }
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
            //var i = iconId.IndexOf("_");
            //string hdIcon = i != -1 ? iconId.Substring(i + 1, iconId.Length-i) : iconId;

            TankDescription tankDescription = null;

            tankDescription = TankDescriptionByIconId(clientVersion, iconId);// ?? TankDescriptionByIconId(clientVersion, hdIcon);
            
            if (iconId == "ussr_t_34")
            {
                //replay tank name t_34 changed to r04_t_34
                return _tanks[0];
            }
            else if (iconId == "ussr_object_704")
            {
                //replay tank name object_704 changed to r53_object_704
                return _tanks[32];
            }
            else if (iconId == "ussr_is_4")
            {
                //0.9.7 replay tank name is_4 changed to r90_is_4m
                return _tanks[24];
            }
            else if (iconId == "usa_pershing")
            {
                //0.9.7 replay tank name pershing changed to a35_pershing
                return _tanks[20023];
            }
            else if (iconId == "usa_t26_e4_superpershing")
            {
                //0.9.7 replay tank name t26_e4_superpershing changed to a80_t26_e4_superpershing
                return _tanks[20052];
            }
            else if (iconId == "usa_t37")
            {
                //0.9.7 replay tank name changed to a94_t37
                return _tanks[20065];
            }

            else if (iconId == "germany_hummel")
            {
                //0.9.8 replay tank name changed to g02_hummel
                return _tanks[10001];
            }
            else if (iconId == "germany_leopard1")
            {
                //0.9.8 replay tank name changed to g89_leopard1
                return _tanks[10057];
            }
            else if (iconId == "france_amx_50_120")
            {
                //0.9.8 replay tank name changed to f09_amx_50_120
                return _tanks[40015];
            }

            else if (iconId == "ussr_su_85")
            {
                //0.9.8 replay tank name changed to r02_su_85
                return _tanks[1];
            }

            else if (iconId == "ussr_is_3")
            {
                //0.9.8 replay tank name changed to r19_is_3
                return _tanks[21];
            }
            else if (iconId == "usa_t32")
            {
                //0.9.8 replay tank name changed to a12_t32
                return _tanks[20017];
            }
            else if (iconId == "usa_a30_m10_wolverine")
            {
                //0.9.8 replay tank name changed to a12_t32
                return _tanks[20017];
            }

            else if (iconId == "usa_sherman_jumbo")
            {
                //0.9.8 replay tank name changed to a36_sherman_jumbo
                return _tanks[20039];
            }
            else if (iconId == "usa_m48a1")
            {
                //0.9.8 replay tank name changed to a84_m48a1
                return _tanks[20055];
            }
            else if (iconId == "france_amx_50_100")
            {
                //0.9.9 replay tank name changed to f08_amx_50_100
                return _tanks[40012];
            }
            else if (iconId == "france_fcm_50t")
            {
                //0.9.9 replay tank name changed to f65_fcm_50t
                return _tanks[40250];
            }
            else if (iconId == "france_amx_50_100_igr")
            {
                //0.9.9 replay tank name changed to f08_amx_50_100_igr
                return _tanks[40153];
            }
            else if (iconId == "germany_vk3601h")
            {
                //0.9.9 replay tank name changed to g15_vk3601h
                return _tanks[10009];
            }
            else if (iconId == "germany_panther_m10")
            {
                //0.9.9 replay tank name changed to g78_panther_m10
                return _tanks[10225];
            }
            else if (iconId == "usa_m46_patton")
            {
                //0.9.9 replay tank name changed to a63_m46_patton
                return _tanks[20035];
            }
            else if (iconId == "usa_t23e3")
            {
                //0.9.9 replay tank name changed to a86_t23e3
                return _tanks[20046];
            }
            else if (iconId == "usa_t110e4")
            {
                //0.9.9 replay tank name changed to a83_t110e4
                return _tanks[20051];
            }
            else if (iconId == "usa_t110e3")
            {
                //0.9.9 replay tank name changed to a85_t110e3
                return _tanks[20054];
            }
            else if (iconId == "usa_m6a2e1")
            {
                //0.9.9 replay tank name changed to a45_m6a2e1
                return _tanks[20205];
            }
            else if (iconId == "ussr_t62a")
            {
                //0.9.9 replay tank name changed to r87_t62a
                return _tanks[54];
            }
            else if (iconId == "france_arl_44")
            {
                //0.9.10 replay tank name changed to f06_arl_44
                return _tanks[40010];
            }
            else if (iconId == "france_amx_m4_1945")
            {
                //0.9.10 replay tank name changed to f07_amx_m4_1945
                return _tanks[40027];
            }
            else if (iconId == "germany_pz35t")
            {
                //0.9.10 replay tank name changed to g07_pz35t
                return _tanks[10003];
            }
            else if (iconId == "germany_jagdpziv")
            {
                //0.9.10 replay tank name changed to g17_jagdpziv
                return _tanks[10006];
            }
            else if (iconId == "germany_panzerjager_i")
            {
                //0.9.10 replay tank name changed to g21_panzerjager_i
                return _tanks[10014];
            }
            else if (iconId == "germany_sturmpanzer_ii")
            {
                //0.9.10 replay tank name changed to g22_sturmpanzer_ii
                return _tanks[10018];
            }
            else if (iconId == "germany_vk1602")
            {
                //0.9.10 replay tank name changed to g26_vk1602
                return _tanks[10021];
            }
            else if (iconId == "germany_jagdtiger")
            {
                //0.9.10 replay tank name changed to g44_jagdtiger
                return _tanks[10031];
            }
            else if (iconId == "germany_pro_ag_a")
            {
                //0.9.10 replay tank name changed to g91_pro_ag_a
                return _tanks[10058];
            }
            else if (iconId == "germany_pzii_j")
            {
                //0.9.10 replay tank name changed to g36_pzii_j
                return _tanks[10202];
            }
            else if (iconId == "usa_m2_lt")
            {
                //0.9.10 replay tank name changed to a02_m2_lt
                return _tanks[20007];
            }
            else if (iconId == "usa_m41")
            {
                //0.9.10 replay tank name changed to a18_m41
                return _tanks[20016];
            }
            else if (iconId == "usa_t110")
            {
                //0.9.10 replay tank name changed to a69_t110e5
                return _tanks[20042];
            }
            else if (iconId == "usa_t71")
            {
                //0.9.10 replay tank name changed to a103_t71e1
                return _tanks[20061];
            }
            else if (iconId == "usa_t71_igr")
            {
                //0.9.10 replay tank name changed to a103_t71e1_igr
                return _tanks[20151];
            }
            else if (iconId == "ussr_t_28")
            {
                //0.9.10 replay tank name changed to r06_t_28
                return _tanks[6];
            }
            else if (iconId == "ussr_su_76")
            {
                //0.9.10 replay tank name changed to r24_su_76
                return _tanks[25];
            }
            else if (iconId == "ussr_su122a")
            {
                //0.9.10 replay tank name changed to r100_su122a
                return _tanks[64];
            }
            else if (iconId == "france_amx_12t")
            {
                //0.9.12 replay tank name changed to f15_amx_12t
                return _tanks[40025];
            }
            else if (iconId == "france_amx_ac_mle1946")
            {
                //0.9.12 replay tank name changed to f35_amx_ac_mle1946
                return _tanks[40042];
            }
            else if (iconId == "germany_pzii")
            {
                //0.9.12 replay tank name changed to g06_pzii
                return _tanks[10008];
            }
            else if (iconId == "germany_bison_i")
            {
                //0.9.12 replay tank name changed to g11_bison_i
                return _tanks[10011];
            }
            else if (iconId == "germany_pzvib_tiger_ii")
            {
                //0.9.12 replay tank name changed to g16_pzvib_tiger_ii
                return _tanks[10020];
            }
            else if (iconId == "germany_grille")
            {
                //0.9.12 replay tank name changed to g23_grille
                return _tanks[10022];
            }
            else if (iconId == "germany_wespe")
            {
                //0.9.12 replay tank name changed to g19_wespe
                return _tanks[10023];
            }
            else if (iconId == "germany_pz38_na")
            {
                //0.9.12 replay tank name changed to g52_pz38_na
                return _tanks[10032];
            }
            else if (iconId == "germany_marder_iii")
            {
                //0.9.12 replay tank name changed to g39_marder_iii
                return _tanks[10044];
            }
            else if (iconId == "germany_jagdpantherii")
            {
                //0.9.12 replay tank name changed to g71_jagdpantherii
                return _tanks[10045];
            }
            else if (iconId == "germany_pz_ii_ausfg")
            {
                //0.9.12 replay tank name changed to g82_pz_ii_ausfg
                return _tanks[10051];
            }
            else if (iconId == "germany_vk2001db")
            {
                //0.9.12 replay tank name changed to g86_vk2001db
                return _tanks[10053];
            }
            else if (iconId == "germany_gw_mk_vie")
            {
                //0.9.12 replay tank name changed to g93_gw_mk_vie
                return _tanks[10059];
            }
            else if (iconId == "germany_s35_captured")
            {
                //0.9.12 replay tank name changed to g34_s35_captured
                return _tanks[10203];
            }
            else if (iconId == "germany_jagdtiger_sdkfz_185")
            {
                //0.9.12 replay tank name changed to g65_jagdtiger_sdkfz_185
                return _tanks[10216];
            }
            else if (iconId == "germany_pzvib_tiger_ii_training")
            {
                //0.9.12 replay tank name changed to g16_pzvib_tiger_ii_training
                return _tanks[10227];
            }
            else if (iconId == "germany_jagdtiger_sdkfz_185_igr")
            {
                //0.9.12 replay tank name changed to g65_jagdtiger_sdkfz_185_igr
                return _tanks[10151];
            }
            else if (iconId == "germany_pzvib_tiger_ii_igr")
            {
                //0.9.12 replay tank name changed to g16_pzvib_tiger_ii_igr
                return _tanks[10153];
            }
            else if (iconId == "uk_gb25_loyd_carrier")
            {
                //0.9.12 replay tank name changed to gb25_loyd_gun_carriage
                return _tanks[50041];
            }
            else if (iconId == "uk_gb70_fv4202_105")
            {
                //0.9.12 replay tank name changed to gb86_centurion_action_x
                return _tanks[50028];
            }
            else if (iconId == "usa_m3_stuart")
            {
                //0.9.12 replay tank name changed to a03_m3_stuart
                return _tanks[20001];
            }
            else if (iconId == "usa_t34_hvy")
            {
                //0.9.12 replay tank name changed to a13_t34_hvy
                return _tanks[20011];
            }
            else if (iconId == "usa_m2_med")
            {
                //0.9.12 replay tank name changed to a25_m2_med
                return _tanks[20019];
            }
            else if (iconId == "usa_m7_med")
            {
                //0.9.12 replay tank name changed to a23_m7_med
                return _tanks[20021];
            }
            else if (iconId == "usa_t2_med")
            {
                //0.9.12 replay tank name changed to a24_t2_med
                return _tanks[20022];
            }
            else if (iconId == "usa_t82")
            {
                //0.9.12 replay tank name changed to a109_t56_gmc
                return _tanks[20025];
            }
            else if (iconId == "usa_t18")
            {
                //0.9.12 replay tank name changed to a46_t3
                return _tanks[20024];
            }
            else if (iconId == "usa_t57")
            {
                //0.9.12 replay tank name changed to a107_t1_hmc
                return _tanks[20008];
            }
            else if (iconId == "usa_t34_hvy_igr")
            {
                //0.9.12 replay tank name changed to a13_t34_hvy_igr
                return _tanks[20153];
            }
            else if (iconId == "ussr_object_212")
            {
                //0.9.12 replay tank name changed to r51_object_212
                return _tanks[33];
            }
            else if (iconId == "ussr_is8")
            {
                //0.9.12 replay tank name changed to r81_is8
                return _tanks[45];
            }
            else if (iconId == "ussr_kv1")
            {
                //0.9.12 replay tank name changed to r80_kv1
                return _tanks[46];
            }
            else if (iconId == "ussr_kv_220")
            {
                //0.9.12 replay tank name changed to r38_kv_220
                return _tanks[200];
            }
            else if (iconId == "ussr_kv_220_action")
            {
                //0.9.12 replay tank name changed to r38_kv_220_action
                return _tanks[211];
            }
            else if (iconId == "france_amx_ac_mle1948")
            {
                //0.9.13 replay tank name changed to f36_amx_ac_mle1948
                return _tanks[40047];
            }
            else if (iconId == "france_amx_ac_mle1948_igr")
            {
                //0.9.13 replay tank name changed to f36_amx_ac_mle1948_igr
                return _tanks[40151];
            }
            else if (iconId == "germany_pzii_luchs")
            {
                //0.9.13 replay tank name changed to g25_pzii_luchs
                return _tanks[10024];
            }
            else if (iconId == "japan_ha_go")
            {
                //0.9.13 replay tank name changed to j03_ha_go
                return _tanks[60003];
            }
            else if (iconId == "japan_ke_ni")
            {
                //0.9.13 replay tank name changed to j04_ke_ni
                return _tanks[60009];
            }
            else if (iconId == "usa_t30")
            {
                //0.9.13 replay tank name changed to a14_t30
                return _tanks[20010];
            }
            else if (iconId == "usa_m7_priest")
            {
                //0.9.13 replay tank name changed to a16_m7_priest
                return _tanks[20014];
            }
            else if (iconId == "usa_t29")
            {
                //0.9.13 replay tank name changed to a11_t29
                return _tanks[20015];
            }
            else if (iconId == "usa_t54e1")
            {
                //0.9.13 replay tank name changed to a89_t54e1
                return _tanks[20060];
            }
            else if (iconId == "usa_t95_e6")
            {
                //0.9.13 replay tank name changed to a95_t95_e6
                return _tanks[20218];
            }
            else if (iconId == "usa_t29_igr")
            {
                //0.9.13 replay tank name changed to a11_t29_igr
                return _tanks[20157];
            }
            else if (iconId == "ussr_su_152")
            {
                //0.9.13 replay tank name changed to r18_su_152
                return _tanks[9];
            }
            else if (iconId == "ussr_su_18")
            {
                //0.9.13 replay tank name changed to r16_su_18
                return _tanks[15];
            }
            else if (iconId == "ussr_kv_3")
            {
                //0.9.13 replay tank name changed to r39_kv_3
                return _tanks[23];
            }
            else if (iconId == "ussr_kv_5")
            {
                //0.9.13 replay tank name changed to r54_kv_5
                return _tanks[208];
            }
            else if (iconId == "ussr_ltp")
            {
                //0.9.13 replay tank name changed to r86_ltp
                return _tanks[221];
            }
            else if (iconId == "ussr_su_152_igr")
            {
                //0.9.13 replay tank name changed to r18_su_152_igr
                return _tanks[156];
            }
            else if (iconId == "ussr_kv_5_igr")
            {
                //0.9.13 replay tank name changed to r54_kv_5_igr
                return _tanks[157];
            }
            else if (iconId == "france__105_lefh18b2")
            {
                //0.9.14 replay tank name changed to f28_105_lefh18b2
                return _tanks[40008];
            }
            else if (iconId == "france_amx_50fosh_155")
            {
                //0.9.14 replay tank name changed to f64_amx_50fosh_155
                return _tanks[40054];
            }
            else if (iconId == "france__105_lefh18b2_igr")
            {
                //0.9.14 replay tank name changed to f28_105_lefh18b2_igr
                return _tanks[40154];
            }
            else if (iconId == "germany_vk3001h")
            {
                //0.9.14 replay tank name changed to g13_vk3001h
                return _tanks[10010];
            }
            else if (iconId == "germany_g_tiger")
            {
                //0.9.14 replay tank name changed to g45_g_tiger
                return _tanks[10034];
            }
            else if (iconId == "germany_nashorn")
            {
                //0.9.14 replay tank name changed to g40_nashorn
                return _tanks[10046];
            }
            else if (iconId == "germany_t_25")
            {
                //0.9.14 replay tank name changed to g46_t_25
                return _tanks[10213];
            }
            else if (iconId == "germany_e_25")
            {
                //0.9.14 replay tank name changed to g48_e_25
                return _tanks[10217];
            }
            else if (iconId == "germany_vk7201")
            {
                //0.9.14 replay tank name changed to g92_vk7201
                return _tanks[10229];
            }
            else if (iconId == "germany_e_25_igr")
            {
                //0.9.14 replay tank name changed to g48_e_25_igr
                return _tanks[10156];
            }
            else if (iconId == "japan_chi_ni")
            {
                //0.9.14 replay tank name changed to j15_chi_ni
                return _tanks[60001];
            }
            else if (iconId == "japan_chi_nu")
            {
                //0.9.14 replay tank name changed to j08_chi_nu
                return _tanks[60005];
            }
            else if (iconId == "japan_chi_ha")
            {
                //0.9.14 replay tank name changed to j07_chi_ha
                return _tanks[60008];
            }
            else if (iconId == "japan_ke_ho")
            {
                //0.9.14 replay tank name changed to j06_ke_ho
                return _tanks[60011];
            }
            else if (iconId == "usa_m4_sherman")
            {
                //0.9.14 replay tank name changed to a05_m4_sherman
                return _tanks[20004];
            }
            else if (iconId == "usa_t20")
            {
                //0.9.14 replay tank name changed to a07_t20
                return _tanks[20006];
            }
            else if (iconId == "usa_t1_hvy")
            {
                //0.9.14 replay tank name changed to a09_t1_hvy
                return _tanks[20013];
            }
            else if (iconId == "usa_t40")
            {
                //0.9.14 replay tank name changed to a29_t40
                return _tanks[20030];
            }
            else if (iconId == "usa_t28_prototype")
            {
                //0.9.14 replay tank name changed to a68_t28_prototype
                return _tanks[20044];
            }
            else if (iconId == "usa_m53_55")
            {
                //0.9.14 replay tank name changed to a88_m53_55
                return _tanks[20063];
            }
            else if (iconId == "usa_m44")
            {
                //0.9.14 replay tank name changed to a87_m44
                return _tanks[20064];
            }
            else if (iconId == "usa_m4_sherman_igr")
            {
                //0.9.14 replay tank name changed to a05_m4_sherman_igr
                return _tanks[20158];
            }
            else if (iconId == "ussr_is")
            {
                //0.9.14 replay tank name changed to r01_is
                return _tanks[2];
            }
            else if (iconId == "ussr_a_20")
            {
                //0.9.14 replay tank name changed to r12_a_20
                return _tanks[8];
            }
            else if (iconId == "ussr_su_100")
            {
                //0.9.14 replay tank name changed to r17_su_100
                return _tanks[14];
            }
            else if (iconId == "ussr_ms_1")
            {
                //0.9.14 replay tank name changed to r11_ms_1
                return _tanks[13];
            }
            else if (iconId == "ussr_object252")
            {
                //0.9.14 replay tank name changed to r61_object252
                return _tanks[36];
            }
            else if (iconId == "ussr_ms_1_bot")
            {
                //0.9.14 replay tank name changed to r11_ms_1_bot
                return _tanks[160];
            }
            return tankDescription ?? TankDescription.Unknown(playerVehicle);
        }

        private TankDescription TankDescriptionByIconId(Version clientVersion, string iconId)
        {
            TankDescription tankDescription = null;
            if (Icons.ContainsKey(iconId))
            {
                TankIcon tankIcon = Icons[iconId];

                if (IconTanks.ContainsKey(tankIcon))
                {
                    tankDescription = IconTanks[tankIcon];

                    //t49 renamed to t67 in 9.3
                    if (tankDescription.UniqueId() == 20071 && clientVersion < new Version("0.9.3.0"))
                    {
                        tankDescription = _tanks[20041];
                    }
                    //kv-1s renamed to kv-85 in 9.3
                    if (tankDescription.UniqueId() == 73 && clientVersion < new Version("0.9.3.0"))
                    {
                        tankDescription = _tanks[11];
                    }
                }
            }
            return tankDescription;
        }

        public TankDescription GetTankDescription(int? typeCompDescr)
        {
            if (typeCompDescr == null)
            {
                return TankDescription.Unknown();
            }

            var uniqueId = Utils.ToUniqueId(typeCompDescr.Value);

            if (!Tanks.ContainsKey(uniqueId))
            {
                return TankDescription.Unknown(typeCompDescr.Value);
            }

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
                    if (_ratingExpectations.ContainsKey(tank.CompDescr))
                    {
                        tank.Expectancy = _ratingExpectations[tank.CompDescr];
                    }
                    else
                    {
                        tank.Expectancy = GetNearestExpectationsByTypeAndLevel(tank);
                    }

                    tank.Title = Resources.Tanks.ResourceManager.GetString(tank.Icon.Icon) ?? tank.Title;

                    tanks.Add(tank);
                }
            }

            _notExistsedTanksList = tanks.Where(x => !x.Active).Select(x => x.UniqueId()).ToList();

            return tanks.ToDictionary(x => x.UniqueId());
        }

        private RatingExpectancy GetNearestExpectationsByTypeAndLevel(TankDescription tank)
        {
            return _ratingExpectations.Values.FirstOrDefault(x => x.TankLevel == tank.Tier && (int) x.TankType == tank.Type);
        }

        private Dictionary<int, RatingExpectancy> ReadRatingExpectationsDictionary()
        {
            try
            {
                using (StreamReader re = new StreamReader(@"External\tanks_expectations.json"))
                {
                    JsonTextReader reader = new JsonTextReader(re);
                    JsonSerializer se = new JsonSerializer();
                    JArray parsedData = se.Deserialize<JArray>(reader);
                    return parsedData.ToObject<List<RatingExpectancy>>().ToDictionary(x => x.CompDescr, x => x);
                }
            }
            catch (Exception e)
            {
                _log.Error(e);
            }
            return new Dictionary<int, RatingExpectancy>();
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
            var dictionary = list.ToDictionary(x => x.MapNameId, y => y);
            UpdateMapsGeometry(dictionary);
            return dictionary;
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
                var attribute = node.Attributes["name"];
                medal.Name = Resources.Resources.ResourceManager.GetString(attribute.Value) ?? attribute.Value;
                medal.NameResourceId = attribute.Value;
                medal.Icon = node.Attributes["icon"].Value;
                medal.Type = int.Parse(node.Attributes["type"].Value);
                var xmlAttribute = node.Attributes["showribbon"];
                if (xmlAttribute != null)
                {
                    medal.ShowRibbon = bool.Parse(xmlAttribute.Value);
                }
                medal.Group = new MedalGroup();

                attribute = node.ParentNode.Attributes["filter"];
                medal.Group.Filter = attribute != null && bool.Parse(attribute.Value);
                attribute = node.ParentNode.Attributes["name"];
                if (attribute != null)
                {
                    medal.Group.Name = Resources.Resources.ResourceManager.GetString(attribute.Value) ?? attribute.Value;
                }
                else
                {
                    medal.Group.Name = node.ParentNode.Name;
                }

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
