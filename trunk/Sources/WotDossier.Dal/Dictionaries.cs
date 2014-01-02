using System;
using System.Collections.Generic;
using System.IO;
using Common.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
        private static readonly ILog _log = LogManager.GetLogger("Dictionaries");

        private static readonly object _syncObject = new object();
        private static volatile Dictionaries _instance = new Dictionaries();

        private readonly Dictionary<int, TankDescription> _tanks;
        private readonly Dictionary<string, TankIcon> _icons = new Dictionary<string, TankIcon>();
        private readonly Dictionary<string, Map> _maps = new Dictionary<string, Map>();
        private readonly Dictionary<int, TankServerInfo> _serverTanks;
        private readonly Dictionary<string, RatingExpectancy> _ratingExpectations;

        /// <summary>
        /// Tanks dictionary
        /// KEY - tankid, countryid
        /// </summary>
        public Dictionary<int, TankDescription> Tanks
        {
            get { return _tanks; }
        }

        public Dictionary<string, TankIcon> Icons
        {
            get { return _icons; }
        }

        public Dictionary<int, TankServerInfo> ServerTanks
        {
            get { return _serverTanks; }
        }

        public Dictionary<string, Map> Maps
        {
            get { return _maps; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        private Dictionaries()
        {
            _ratingExpectations = ReadRatingExpectationsDictionary();
            _tanks = ReadTanksDictionary();
            _serverTanks = ReadServerTanksDictionary();
            _maps = ReadMaps();
        }

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

        public TankIcon GetTankIcon(string playerVehicle)
        {
            string replace = playerVehicle.Replace(":", "_").Replace("-", "_").Replace(" ", "_").Replace(".", "_").ToLower();
            if (Icons.ContainsKey(replace))
            {
                return Icons[replace];
            }
            return TankIcon.Empty;
        }

        public TankDescription GetTankDecription(string playerVehicle)
        {
            string replace = playerVehicle.Replace(":", "_").Replace("-", "_").Replace(" ", "_").Replace(".", "_").ToLower();
            TankDescription description = Tanks.Values.FirstOrDefault(x => x.Icon.IconId == replace);
            if (description != null)
            {
                return description;
            }
            return null;
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
                    TankDescription tank = jToken.ToObject<TankDescription>();
                    tank.CountryCode = WotApiHelper.GetCountryNameCode(tank.CountryId);

                    TankIcon icon = jToken.ToObject<TankIcon>();
                    icon.CountryCode = tank.CountryCode;
                    _icons.Add(icon.IconId, icon);

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
                    var parsedData = se.Deserialize<JArray>(reader);
                    return parsedData.ToObject<List<RatingExpectancy>>().ToDictionary(x => x.Icon, x => x);
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
            List<Map> maps;
            using (StreamReader re = new StreamReader(@"External\maps.json"))
            {
                JsonTextReader reader = new JsonTextReader(re);
                JsonSerializer se = new JsonSerializer();
                maps = se.Deserialize<List<Map>>(reader);
            }

            List<Map> list = (maps ?? new List<Map>());
            int i = 1;
            list.ForEach(x => x.localizedmapname = Resources.Resources.ResourceManager.GetString("Map_" + x.mapidname) ?? x.mapname);
            list = list.OrderByDescending(x => x.localizedmapname).ToList();
            list.ForEach(x => x.mapid = i++);
            return list.ToDictionary(x => x.mapidname, y => y);
        }

        //public Dictionary<int, TankDescription> ReadTankNominalDamage()
        //{
        //    Dictionary<int, TankDescription> dictionary = new Dictionary<int, TankDescription>();
        //    using (StreamReader streamReader = new StreamReader(@"Data\TankNominalDamage.xml"))
        //    {
        //        XmlDocument document = new XmlDocument();
        //        document.Load(streamReader);

        //        XmlNodeList xmlNodeList = document.SelectNodes("damage/tr");

        //        foreach (XmlNode node in xmlNodeList)
        //        {
        //            XmlNodeList values = node.SelectNodes("td");
        //            if (values != null)
        //            {
        //                TankDescription description = new TankDescription();
        //                description.CountryId = WotApiHelper.GetCountryIdBy2Letters(values[2].InnerText);
        //                description.Tier = int.Parse(values[3].InnerText);
        //                description.Type = (int)Enum.Parse(typeof(TankType), values[4].InnerText);
        //                double nominalDamage = double.Parse(values[5].InnerText, CultureInfo.InvariantCulture);
        //                dictionary.Add(description.UniqueId(), description);
        //            }
        //        }
        //    }

        //    return dictionary;
        //}
    }
}
