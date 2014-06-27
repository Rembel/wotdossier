using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Common.Logging;
using Ionic.Zlib;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WotDossier.Common;
using WotDossier.Common.Extensions;
using WotDossier.Dal;
using WotDossier.Domain.Dossier.AppSpot;
using WotDossier.Domain.Replay;
using WotDossier.Domain.Tank;

namespace WotDossier.Applications
{
    public static class WotFileHelper
    {
        private const char SEPARATOR = ';';
        private const string REPLAY_DATABLOCK_2 = "datablock_2";

        private static readonly ILog _log = LogManager.GetCurrentClassLogger();
        public static Version JsonFormatedReplay_MinVersion = new Version("0.8.11.0");

        #region Cache

        /// <summary>
        /// Gets the cache file.
        /// </summary>
        /// <param name="playerId">The player id.</param>
        /// <param name="server"></param>
        /// <returns>null if there is no any dossier cache file for specified player</returns>
        public static FileInfo GetCacheFile(string playerId, string server)
        {
            _log.Trace("GetCacheFile start");

            FileInfo cacheFile = null;

            string[] files = new string[0];

            try
            {
                files = Directory.GetFiles(Folder.GetDossierCacheFolder(), "*.dat");
            }
            catch (DirectoryNotFoundException ex)
            {
                _log.Error("Cann't find dossier cache files", ex);
            }

            if (!files.Any())
            {
                return null;
            }

            foreach (string file in files)
            {
                FileInfo info = new FileInfo(file);

                string decodFileName = DecodFileName(info);
                string playerName = decodFileName.Split(SEPARATOR)[1];
                string serverName = decodFileName.Split(SEPARATOR)[0];

                if (playerName.Equals(playerId, StringComparison.InvariantCultureIgnoreCase) &&
                    serverName.Contains(Dictionaries.Instance.GameServers[server]))
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

            ExecuteTask(task, arguments, logPath, workingDirectory);

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

        #endregion


        #region Replay

        /// <summary>
        /// Binary dossier cache to plain json.
        /// </summary>
        /// <param name="replayFile">The replay file.</param>
        /// <param name="readAdvancedData">if set to <c>true</c> [read advanced data].</param>
        /// <returns>Path to json file</returns>
        public static string ReplayToJson(FileInfo replayFile, bool readAdvancedData = false)
        {
            string outputJsonFilePath = replayFile.FullName.Replace(replayFile.Extension, ".json");
            
            //if file already created the return it's path
            if (File.Exists(outputJsonFilePath))
            {
                return outputJsonFilePath;
            }

            string directoryName = Environment.CurrentDirectory;

            string task = directoryName + @"\External\wotrp2j.exe";
            string arguments = string.Format("\"{0}\" {1}", replayFile.FullName, readAdvancedData ? "-a" : string.Empty);
            var logPath = directoryName + @"\Logs\wotrp2j.log";
            var workingDirectory = directoryName + @"\External";

            ExecuteTask(task, arguments, logPath, workingDirectory);

            return outputJsonFilePath;
        }

        /// <summary>
        /// Loads the replay.
        /// </summary>
        /// <param name="replayFile">The replay file.</param>
        /// <param name="readAdvancedData">if set to <c>true</c> [read advanced data].</param>
        /// <returns></returns>
        public static Replay ParseReplay_8_0(FileInfo replayFile, bool readAdvancedData = false)
        {
            //convert dossier cache file to json
            string jsonFile = ReplayToJson(replayFile, readAdvancedData);

            Replay replay;

            using (StreamReader re = new StreamReader(jsonFile))
            {
                JsonTextReader reader = new JsonTextReader(re);
                JsonSerializer se = new JsonSerializer();
                replay = se.Deserialize<Replay>(reader);
            }

            using (StreamReader re = new StreamReader(jsonFile))
            {
                JsonTextReader reader = new JsonTextReader(re);
                JsonSerializer se = new JsonSerializer();
                JObject parsedData = (JObject) se.Deserialize(reader);
                if (parsedData != null && ((IDictionary<string, JToken>) parsedData).ContainsKey(REPLAY_DATABLOCK_2))
                {
                    replay.datablock_battle_result_plain = parsedData[REPLAY_DATABLOCK_2][0].ToObject<PlayerResult>();
                    replay.datablock_1.vehicles =
                        parsedData[REPLAY_DATABLOCK_2][1].ToObject<Dictionary<long, Vehicle>>();
                }
            }

            return replay;
        }

        /// <summary>
        /// Reads the replay statistic blocks.
        /// </summary>
        /// <param name="file">The replay file.</param>
        /// <param name="readAdvancedData">if set to <c>true</c> [read advanced data].</param>
        /// <returns></returns>
        public static Replay ParseReplay_8_11(FileInfo file, bool readAdvancedData = false)
        {
            string path = file.FullName;
            string firstBlockJson = string.Empty;
            string battleResultBlockJson = string.Empty;
            if (File.Exists(path))
            {
                using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    int blockLength = 0;
                    int blocksCount = 0;
                    byte[] buffer = new byte[4];
                    stream.Read(buffer, 0, 4);
                    if (buffer[0] != 0x21)
                    {
                        blocksCount = (int) stream.Read(4).ConvertLittleEndian();
                        blockLength = (int)stream.Read(4).ConvertLittleEndian();
                    }
                    firstBlockJson = stream.Read(blockLength).GetAsciiString();
                    if (blockLength > 0 && blocksCount > 1)
                    {
                        blockLength = (int)stream.Read(4).ConvertLittleEndian();
                        battleResultBlockJson = stream.Read(blockLength).GetAsciiString();
                    }

                    FirstBlock firstBlock = null;
                    PlayerResult commandResult = null;
                    BattleResult battleResult = null;
                    AdvancedReplayData advancedReplayData = null;

                    if (firstBlockJson.Length > 0)
                    {
                        firstBlock = JsonConvert.DeserializeObject<FirstBlock>(firstBlockJson);
                    }

                    if (firstBlock != null)
                    {

                        try
                        {
                            if (blocksCount > 1)
                            {
                                var parsedData = JsonConvert.DeserializeObject<JArray>(battleResultBlockJson);
                                if (parsedData.Count > 0)
                                {
                                    if (firstBlock.Version < JsonFormatedReplay_MinVersion)
                                    {
                                        commandResult = parsedData[0].ToObject<PlayerResult>();
                                    }
                                    else
                                    {
                                        battleResult = parsedData[0].ToObject<BattleResult>();
                                        firstBlock.vehicles = parsedData[1].ToObject<Dictionary<long, Vehicle>>();
                                    }
                                }
                            }

                            if (readAdvancedData)
                            {
                                const int startPosition = 8;
                                int length = (int) (stream.Length - stream.Position) - startPosition;
                                buffer = new byte[length];
                                stream.Seek(startPosition, SeekOrigin.Current);
                                stream.Read(buffer, 0, length);
                                
                                byte[] decrypt = Decrypt(buffer);
                                byte[] uncompressed = Decompress(decrypt);
                                
                                advancedReplayData = ExtractAdvanced(uncompressed);
                            }
                        }
                        catch (Exception e)
                        {
                            _log.ErrorFormat("Error on replay file read. Incorrect file format({0})", e, file.FullName);
                        }

                        return new Replay
                        {
                            datablock_1 = firstBlock,
                            datablock_battle_result_plain = commandResult,
                            datablock_battle_result = battleResult,
                            datablock_advanced = advancedReplayData
                        };
                    }
                }
            }
            return null;
        }

        private static AdvancedReplayData ExtractAdvanced(byte[] uncompressed)
        {
            AdvancedReplayData advanced = new AdvancedReplayData();

            using (var f = new MemoryStream(uncompressed))
            {
                f.Seek(12, SeekOrigin.Begin);
                int versionlength = (int) f.Read(1).ConvertLittleEndian();

                /*                
		if not is_supported_replay(f):
			advanced['valid'] = 0
			printmessage('Unsupported replay: Versionlength: ' + str(versionlength))
			return advanced
                */

                f.Seek(3, SeekOrigin.Current);

                advanced.replay_version = f.Read(versionlength).GetUtf8String();
                advanced.replay_version = advanced.replay_version.Replace(", ", ".");
                advanced.replay_version = advanced.replay_version.Replace(". ", ".");
                advanced.replay_version = advanced.replay_version.Replace(' ', '.');

                f.Seek(51 + versionlength, SeekOrigin.Begin);

                int playernamelength = (int) f.Read(1).ConvertLittleEndian();

                advanced.playername = f.Read(playernamelength).GetUtf8String();
                advanced.arenaUniqueID = (long) f.Read(8).ConvertLittleEndian();
                advanced.arenaCreateTime = advanced.arenaUniqueID & 4294967295L;

                advanced.arenaTypeID = (int) f.Read(4).ConvertLittleEndian();
                advanced.gameplayID = advanced.arenaTypeID >> 16;
                advanced.arenaTypeID = advanced.arenaTypeID & 32767;

                advanced.bonusType = (int) f.Read(1).ConvertLittleEndian();
                advanced.guiType = (int) f.Read(1).ConvertLittleEndian();

                advanced.more = new BattleInfo();
                int advancedlength = (int) f.Read(1).ConvertLittleEndian();

                if (advancedlength == 255)
                {
                    advancedlength = (int) f.Read(2).ConvertLittleEndian();
                    f.Seek(1, SeekOrigin.Current);
                }

                try
                {
                    byte[] advanced_pickles = f.Read(advancedlength);
                    object load = Unpickle.Load(new MemoryStream(advanced_pickles));
                    advanced.more = load.ToObject<BattleInfo>();
                }
                catch (Exception e)
                {
                    _log.Error(
                        "Cannot load advanced pickle. \nPosition: " + f.Position + ", Length: " + advancedlength, e);
                }

                f.Seek(29, SeekOrigin.Current);

                advancedlength = (int) f.Read(1).ConvertLittleEndian();

                if (advancedlength == 255)
                {
                    advancedlength = (int) f.Read(2).ConvertLittleEndian();
                    f.Seek(1, SeekOrigin.Current);
                }

                var rosters = new List<object>();
                var rosterdata = new Dictionary<string, AdvancedPlayerInfo>();
                advanced.roster = rosterdata;

                try
                {
                    byte[] advanced_pickles = f.Read(advancedlength);
                    rosters = (List<object>) Unpickle.Load(new MemoryStream(advanced_pickles));
                }
                catch (Exception e)
                {
                    _log.Error("Cannot load roster pickle. Position: " + f.Position + ", Length: " + advancedlength, e);
                }

                foreach (object[] roster in rosters)
                {
                    string key = (string) roster[2];
                    rosterdata[key] = new AdvancedPlayerInfo();
                    rosterdata[key].internaluserID = (int) roster[0];
                    rosterdata[key].playerName = key;
                    rosterdata[key].team = (int) roster[3];
                    rosterdata[key].accountDBID = (int) roster[7];
                    rosterdata[key].clanAbbrev = (string) roster[8];
                    rosterdata[key].clanID = (int) roster[9];
                    rosterdata[key].prebattleID = (int) roster[10];

                    var bindataBytes = Encoding.GetEncoding(1252).GetBytes((string) roster[1]);
                    List<int> bindata = bindataBytes.Unpack("BBHHHHHHB");

                    rosterdata[key].countryID = bindata[0] >> 4 & 15;
                    rosterdata[key].tankID = bindata[1];
                    int compDescr = (bindata[1] << 8) + bindata[0];
                    rosterdata[key].compDescr = compDescr;

                    //Does not make sense, will check later
                    rosterdata[key].vehicle = new AdvancedVehicleInfo();
                    rosterdata[key].vehicle.chassisID = bindata[2];
                    rosterdata[key].vehicle.engineID = bindata[3];
                    rosterdata[key].vehicle.fueltankID = bindata[4];
                    rosterdata[key].vehicle.radioID = bindata[5];
                    rosterdata[key].vehicle.turretID = bindata[6];
                    rosterdata[key].vehicle.gunID = bindata[7];

                    int flags = bindata[8];
                    int optional_devices_mask = flags & 15;
                    int idx = 2;
                    int pos = 15;

                    while (optional_devices_mask != 0)
                    {
                        if ((optional_devices_mask & 1) == 1)
                        {
                            try
                            {
                                int m = (int) bindataBytes.Skip(pos).Take(2).ToArray().ConvertLittleEndian();
                                rosterdata[key].vehicle.module[idx] = m;
                            }
                            catch (Exception e)
                            {
                                _log.Error("error on processing player [" + key + "]: ", e);
                            }
                        }

                        optional_devices_mask = optional_devices_mask >> 1;
                        idx = idx - 1;
                        pos = pos + 2;

                    }
                }
            }
            return advanced;
        }

        private static byte[] Decompress(byte[] decrypt)
        {
            return ZlibStream.UncompressBuffer(decrypt);
        }

        private static byte[] Decrypt(byte[] data)
        {
            byte[] key =
            {
                0xDE, 0x72, 0xBE, 0xA0,
                0xDE, 0x04, 0xBE, 0xB1,
                0xDE, 0xFE, 0xBE, 0xEF,
                0xDE, 0xAD, 0xBE, 0xEF
            };

            BlowFish blowFish = new BlowFish(key);

            byte[] block = new byte[8];

            MemoryStream dataStream = new MemoryStream();

            byte[] pb = null;

            for (int i = 0, bi = 0; i < data.Length; i++, bi++)
            {
                block[bi] = data[i];
                if (bi == 7 || i == data.Length - 1)
                {
                    byte[] db = blowFish.Decrypt_ECB(block);

                    if (pb != null)
                    {
                        db = BitwiseXOR(pb, db);
                    }

                    dataStream.Write(db, 0, 8);
                    pb = db;
                    block = new byte[8];
                    bi = -1;
                }
            }

            return dataStream.ToArray();
        }

        private static byte[] BitwiseXOR(byte[] result, byte[] matchValue)
        {
            if (result.Length == 0)
            {
                return matchValue;
            }

            byte[] newResult = new byte[matchValue.Length > result.Length ? matchValue.Length : result.Length];

            for (int i = 1; i < newResult.Length + 1; i++)
            {
                //Use XOR on the LSBs until we run out
                if (i > result.Length)
                {
                    newResult[newResult.Length - i] = matchValue[matchValue.Length - i];
                }
                else if (i > matchValue.Length)
                {
                    newResult[newResult.Length - i] = result[result.Length - i];
                }
                else
                {
                    newResult[newResult.Length - i] =
                        (byte) (matchValue[matchValue.Length - i] ^ result[result.Length - i]);
                }
            }
            return newResult;
        }

        #endregion


        private static void ExecuteTask(string task, string arguments, string logPath, string workingDirectory = null)
        {
            using(Process proc = new Process())
            {
                proc.StartInfo.CreateNoWindow = true;
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.FileName = task;
                proc.StartInfo.Arguments = arguments;

                if (!string.IsNullOrEmpty(workingDirectory))
                {
                    proc.StartInfo.WorkingDirectory = workingDirectory;
                }

                proc.Start();

                //write log
                using (StreamWriter streamWriter = new StreamWriter(logPath, false))
                {
                    streamWriter.WriteLine(proc.StandardOutput.ReadToEnd());   
                }

                proc.WaitForExit();
            }
        }

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

        public static Replay ParseReplay(FileInfo phisicalFile, Version clientVersion, bool readAdvancedData)
        {
            if (clientVersion < JsonFormatedReplay_MinVersion)
            {
                return ParseReplay_8_0(phisicalFile, readAdvancedData);
            }
            return ParseReplay_8_11(phisicalFile, readAdvancedData);
        }
    }
}
