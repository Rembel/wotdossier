﻿using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WotDossier.Domain;
using System.Linq;
using WotDossier.Domain.Rows;

namespace WotDossier.Applications
{

    public class Read
    {
        private static readonly object _syncObject = new object();
        private static volatile Read _instance = new Read();

        private static readonly Dictionary<KeyValuePair<int, int>, TankInfo> tankDictionary;
        private static readonly Dictionary<string, TankContour> _contoursDictionary;

        public static Dictionary<KeyValuePair<int, int>, TankInfo> TankDictionary
        {
            get { return tankDictionary; }
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
            tankDictionary = ReadTanksLibrary();
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
                    tank.Info = tankDictionary[new KeyValuePair<int, int>(tank.Common.tankid, tank.Common.countryid)];
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
    }
}