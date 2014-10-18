using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Common.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WotDossier.Common;
using WotDossier.Dal;
using WotDossier.Domain.Dossier.AppSpot;
using WotDossier.Domain.Tank;

namespace WotDossier.Applications
{
    public static class CacheFileHelper
    {
        private const char SEPARATOR = ';';
        
        private static readonly ILog _log = LogManager.GetCurrentClassLogger();

        #region Cache

        /// <summary>
        /// Gets the cache file.
        /// </summary>
        /// <param name="playerName">Name of the player.</param>
        /// <param name="server">The server.</param>
        /// <returns>
        /// null if there is no any dossier cache file for specified player
        /// </returns>
        public static FileInfo GetCacheFile(string playerName, string server)
        {
            _log.Trace("GetCacheFile start");

            FileInfo cacheFile = null;

            string[] files = new string[0];
            
            var dossierCacheFolder = Folder.GetDossierCacheFolder();
            if (Directory.Exists(dossierCacheFolder))
            {
                files = Directory.GetFiles(dossierCacheFolder, "*.dat");
            }

            if (!files.Any())
            {
                return null;
            }

            foreach (string file in files)
            {
                FileInfo info = new FileInfo(file);

                string decodedFileName = DecodFileName(info);
                string decodedPlayerName = decodedFileName.Split(SEPARATOR)[1];
                string decodedDerverName = decodedFileName.Split(SEPARATOR)[0];

                if (decodedPlayerName.Equals(playerName, StringComparison.InvariantCultureIgnoreCase) &&
                    decodedDerverName.Contains(Dictionaries.Instance.GameServers[server]))
                {
                    if (cacheFile == null)
                    {
                        cacheFile = info;
                    }
                    else if (cacheFile.LastWriteTime < info.LastWriteTime)
                    {
                        cacheFile = info;
                    }
                }
            }
            if (cacheFile != null)
            {
                cacheFile = cacheFile.CopyTo(Path.Combine(Path.GetTempPath(), cacheFile.Name), true);
            }
            _log.Trace("GetCacheFile end");
            return cacheFile;
        }

        /// <summary>
        /// Binary dossier cache to plain json.
        /// -f - By setting f the JSON will be formatted for better human readability
        /// -r - By setting r the JSON will contain all fields with their values and recognized names
        /// -k - By setting k the JSON will not contain Kills/Frags
        /// -s - By setting s the JSON will not include unix timestamp of creation as it is useless for calculation of 
        /// </summary>
        /// <param name="cacheFile">The cache file.</param>
        public static string BinaryCacheToJson(FileInfo cacheFile)
        {
            _log.Trace("BinaryCacheToJson start");

            string directoryName = Environment.CurrentDirectory;

            string task = directoryName + @"\External\wotdc2j.exe";
            string arguments = string.Format("\"{0}\" -f", cacheFile.FullName);
            var logPath = directoryName + @"\Logs\wotdc2j.log";
            var workingDirectory = directoryName + @"\External";

            ExternalTask.Execute(task, arguments, logPath, workingDirectory);

            _log.Trace("BinaryCacheToJson end");
            return cacheFile.FullName.Replace(".dat", ".json");
        }

        /// <summary>
        /// Reads the tanks from cache.
        /// </summary>
        /// <param name="path">The path to parsed cache file.</param>
        /// <returns></returns>
        public static List<TankJson> ReadTanksCache(string path)
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
        /// Reads the dossier application spot tanks.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static List<TankJson> ReadDossierAppSpotTanks(string data)
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
        /// Extends the properties data with Description and FragsList.
        /// </summary>
        /// <param name="tank">The tank.</param>
        /// <returns></returns>
        public static bool ExtendPropertiesData(TankJson tank)
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


                            TankDescription tankDescription;

                            if (Dictionaries.Instance.Tanks.ContainsKey(uniqueId))
                            {
                                tankDescription = Dictionaries.Instance.Tanks[uniqueId];
                            }
                            else
                            {
                                tankDescription = TankDescription.Unknown;
                            }

                            return new FragsJson
                            {
                                CountryId = countryId,
                                TankId = tankId,
                                Icon = tankDescription.Icon,
                                TankUniqueId = uniqueId,
                                Count = Convert.ToInt32(x[2]),
                                Type = tankDescription.Type,
                                Tier = tankDescription.Tier,
                                KilledByTankUniqueId = tank.UniqueId(),
                                Tank = tankDescription.Title
                            };
                        }).ToList();
                return true;
            }
            _log.WarnFormat("Found unknown tank:\n{0}", JsonConvert.SerializeObject(tank.Common, Formatting.Indented));
            return false;
        }

        #endregion

        /// <summary>
        /// Gets the name of the player from name of dossier cache file.
        /// </summary>
        /// <param name="cacheFile">The cache file in base32 format. Example of decoded filename - login-ct-p1.worldoftanks.com:20015;_Rembel__RU</param>
        /// <returns></returns>
        public static string GetPlayerName(FileInfo cacheFile)
        {
            var decodedFileName = DecodFileName(cacheFile);
            return decodedFileName.Split(SEPARATOR)[1];
        }

        /// <summary>
        /// Decods the name of the file.
        /// </summary>
        /// <param name="cacheFile">The cache file.</param>
        /// <returns></returns>
        public static string DecodFileName(FileInfo cacheFile)
        {
            Base32Encoder encoder = new Base32Encoder();
            string str = cacheFile.Name.Replace(cacheFile.Extension, string.Empty);
            byte[] decodedFileNameBytes = encoder.Decode(str.ToLowerInvariant());
            string decodedFileName = Encoding.UTF8.GetString(decodedFileNameBytes);
            return decodedFileName;
        }
    }
}