using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Common.Logging;
using Ionic.Zlib;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WotDossier.Common;
using WotDossier.Common.Extensions;
using WotDossier.Dal;
using WotDossier.Domain;
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
                                stream.Seek(startPosition, SeekOrigin.Current);

                                int replayDataLength = (int)(stream.Length - stream.Position) - startPosition;

                                byte[] decrypt = DecryptReplayData(stream.Read(replayDataLength));
                                byte[] uncompressed = DecompressReplayData(decrypt);
                                
                                uncompressed.Dump(@"c:\\temp");

                                using (var uncompressedReplayStream = new MemoryStream(uncompressed))
                                {
                                    advancedReplayData = ReadReplayStream(uncompressedReplayStream);
                                }
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

        private static AdvancedReplayData ReadReplayStream(Stream stream)
        {
            AdvancedReplayData data = new AdvancedReplayData();

            bool endOfStream = false;
            _log.Trace("Begin replay stream read");
            while (!endOfStream)
            {
                endOfStream = ReadPacket(stream, data);
            }
            _log.Trace("End replay stream read");
            return data;
        }

        private static bool ReadPacket(Stream stream, AdvancedReplayData data)
        {
            ulong packetLength = stream.Read(4).ConvertLittleEndian();
            ulong packetType = stream.Read(4).ConvertLittleEndian();

            //move to payload
            stream.Seek(4, SeekOrigin.Current);

            long position = stream.Position;

            bool endOfStream = packetType == new byte[] { 255, 255, 255, 255 }.ConvertLittleEndian() || stream.Position > stream.Length;

            byte[] payload = new byte[packetLength];

            if (!endOfStream)
            {
                stream.Read(payload, 0, (int) packetLength);

                var packet = new Packet
                {
                    Payload = payload,
                    PacketType = packetType,
                    PacketLength = packetLength,
                    Position = position
                };

                //battle level setup 
                if (packet.PacketType == 0x00)
                {
                    _log.Trace("Process packet 0x00");
                    ProcessPacket_0x00(packet.Payload, data);
                }

                //replay version
                if (packet.PacketType == 0x14)
                {
                    _log.Trace("Process packet 0x14");
                    ProcessPacket_0x14(packet, data);
                }

                //in game updates
                if (packet.PacketType == 0x08)
                {
                    _log.Trace("Process packet 0x08");
                    ProcessPacket_0x08(packet, data);
                }

                //chat
                if (packet.PacketType == 0x1f)
                {
                    _log.Trace("Process packet 0x1f");
                    ProcessPacket_0x1f(packet, data);
                }
            }

            return endOfStream;
        }

        /// <summary>
        /// Process packet 0x00
        /// Contains Battle level setup and Player Name.
        /// </summary>
        /// <param name="payload">The payload.</param>
        /// <param name="data">The data.</param>
        private static void ProcessPacket_0x00(byte[] payload, AdvancedReplayData data)
        {
            using (var f = new MemoryStream(payload))
            {
                f.Seek(10, SeekOrigin.Begin);

                int playernamelength = (int)f.Read(1).ConvertLittleEndian();

                data.playername = f.Read(playernamelength).GetUtf8String();

                f.Seek(playernamelength+25, SeekOrigin.Begin);

                data.more = new BattleInfo();
                int advancedlength = (int)f.Read(1).ConvertLittleEndian();

                if (advancedlength == 255)
                {
                    advancedlength = (int)f.Read(2).ConvertLittleEndian();
                    f.Seek(1, SeekOrigin.Current);
                }

                try
                {
                    byte[] advancedPickles = f.Read(advancedlength);
                    object load = Unpickle.Load(new MemoryStream(advancedPickles));
                    data.more = load.ToObject<BattleInfo>();
                }
                catch (Exception e)
                {
                    _log.Error(
                        "Cannot load advanced pickle. \nPosition: " + f.Position + ", Length: " + advancedlength, e);
                }
            }
        }

        /// <summary>
        /// Process packet 0x08
        /// Contains Various game state updates 
        /// </summary>
        /// <param name="packet">The packet.</param>
        /// <param name="data">The data.</param>
        private static void ProcessPacket_0x08(Packet packet, AdvancedReplayData data)
        {
            using (MemoryStream stream = new MemoryStream(packet.Payload))
            {
                //skip 0-4 - player_id
                stream.Seek(4, SeekOrigin.Current);
                //read 4-8 - subType
                packet.SubType = stream.Read(4).ConvertLittleEndian();
                //read 8-12 - update length
                packet.SubTypePayloadLength = stream.Read(4).ConvertLittleEndian();

                if (packet.SubType == 0x1d) //onArenaUpdate events
                {
                    ProcessPacket_0x08_0x1d(packet, stream, data);
                }

                if (packet.SubType == 0x09) //onSlotUpdate events
                {
                    ProcessPacket_0x08_0x09(packet, stream, data);
                }
            }
        }

        /// <summary>
        /// Process packet 0x08 subType 0x1d
        /// http://wiki.vbaddict.net/pages/Packet_0x08
        /// </summary>
        /// <param name="packet">The packet.</param>
        /// <param name="stream">The stream.</param>
        /// <param name="data">The data.</param>
        private static void ProcessPacket_0x08_0x1d(Packet packet, MemoryStream stream, AdvancedReplayData data)
        {
            ulong updateType = stream.Read(1).ConvertLittleEndian();

            //For update types 0x01 and 0x04: at offset 14, read an uint16 and unpack it to 2 bytes, 
            //if the unpacked value matches 0x80, 0x02 then set your offset to 14. 
            //If the unpacked value does not match 0x80, 0x02 set your offset to 17. 
            if (updateType == 0x01 || updateType == 0x04)
            {
                ulong firstByte = stream.Read(1).ConvertLittleEndian();

                ulong secondByte = stream.Read(1).ConvertLittleEndian();

                if (firstByte != 0x80 || secondByte != 0x02)
                {
                    stream.Seek(2, SeekOrigin.Current);
                    packet.SubTypePayloadLength = packet.SubTypePayloadLength - 5;
                }
                else
                {
                    stream.Seek(-1, SeekOrigin.Current);
                    packet.SubTypePayloadLength = packet.SubTypePayloadLength - 2;
                }
            }
            else
            {
                stream.Seek(1, SeekOrigin.Current);
                packet.SubTypePayloadLength = packet.SubTypePayloadLength - 2;
            }

            //Updates the vehicle list; also known as the roster
            if (updateType == 1 && data.roster == null)
            {
                //Read from your offset to the end of the packet, this will be the "update pickle". 
                byte[] updatePayload = stream.Read((int) (packet.SubTypePayloadLength));

                var rosterdata = new Dictionary<string, AdvancedPlayerInfo>();
                data.roster = rosterdata;

                List<object> rosters;

                using (var updatePayloadStream = new MemoryStream(updatePayload))
                {
                    rosters = (List<object>) Unpickle.Load(updatePayloadStream);
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
                    int optionalDevicesMask = flags & 15;
                    int idx = 2;
                    int pos = 15;

                    while (optionalDevicesMask != 0)
                    {
                        if ((optionalDevicesMask & 1) == 1)
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

                        optionalDevicesMask = optionalDevicesMask >> 1;
                        idx = idx - 1;
                        pos = pos + 2;

                    }
                }
            }

        }
        
        /// <summary>
        /// Process packet 0x08 subType 0x09
        /// Contains slots updates
        /// </summary>
        /// <param name="packet">The packet.</param>
        /// <param name="stream">The stream.</param>
        /// <param name="data">The data.</param>
        private static void ProcessPacket_0x08_0x09(Packet packet, MemoryStream stream, AdvancedReplayData data)
        {
            //buffer = new byte[packet.SubTypePayloadLength];
            ////Read from your offset to the end of the packet, this will be the "update pickle". 
            //stream.Read(buffer, 0, (int) (packet.SubTypePayloadLength));

            ulong value = stream.Read(4).ConvertLittleEndian();
            var item = new SlotItem((SlotType)(value & 15), value >> 4 & 15, value >> 8 & 65535);

            ulong count = stream.Read(2).ConvertLittleEndian();

            ulong rest = stream.Read(3).ConvertLittleEndian();

            var slot = new Slot(item, count, rest);

            if (data.Slots.Count < 6)
            {
                data.Slots.Add(slot);
            }
        }

        /// <summary>
        /// Process packet 0x14
        /// Contains Replay version
        /// </summary>
        /// <param name="packet">The packet.</param>
        /// <param name="data">The data.</param>
        private static void ProcessPacket_0x14(Packet packet, AdvancedReplayData data)
        {
            using (var f = new MemoryStream(packet.Payload))
            {
                int versionlength = (int)f.Read(1).ConvertLittleEndian();

                f.Seek(3, SeekOrigin.Current);

                data.replay_version = f.Read(versionlength).GetUtf8String();
                data.replay_version = data.replay_version.Replace(", ", ".");
                data.replay_version = data.replay_version.Replace(". ", ".");
                data.replay_version = data.replay_version.Replace(' ', '.');
            }
        }

        /// <summary>
        /// Processes the packet 0x1f.
        /// Contains chat messages
        /// </summary>
        /// <param name="packet">The packet.</param>
        /// <param name="data">The data.</param>
        private static void ProcessPacket_0x1f(Packet packet, AdvancedReplayData data)
        {
            string message = Encoding.UTF8.GetString(packet.Payload);
            data.Messages.Add(ParseChatMessage(message.Replace("&nbsp;", " ").Replace(":", "")));
        }

        public static ChatMessage ParseChatMessage(string messageText)
        {
            var reg = new Regex(@"<(?<tag>[\w]+)[^>]*color\s*=\s*['""](?<color>[^'""]+)['""][^>]*>(?<text>.*?)<\/\<tag>", RegexOptions.IgnoreCase);
            MatchCollection match = reg.Matches(messageText);
            return new ChatMessage
            {
                Player = match[0].Groups["text"].Value.Trim(), 
                PlayerColor = match[0].Groups["color"].Value.Trim(), 
                Text = match[1].Groups["text"].Value.Trim(),
                TextColor = match[1].Groups["color"].Value.Trim()

            };
        }

        /// <summary>
        /// Decompresses the specified decrypted replay bytes.
        /// </summary>
        /// <param name="decryptedReplaysBytes">The decrypted replay bytes.</param>
        /// <returns></returns>
        private static byte[] DecompressReplayData(byte[] decryptedReplaysBytes)
        {
            return ZlibStream.UncompressBuffer(decryptedReplaysBytes);
        }

        /// <summary>
        /// Decrypts the specified data.
        /// </summary>
        /// <param name="replayData">The replay data.</param>
        /// <returns></returns>
        private static byte[] DecryptReplayData(byte[] replayData)
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

            for (int i = 0, bi = 0; i < replayData.Length; i++, bi++)
            {
                block[bi] = replayData[i];
                if (bi == 7 || i == replayData.Length - 1)
                {
                    byte[] db = blowFish.Decrypt_ECB(block);

                    if (pb != null)
                    {
                        db = ByteArrayExtensions.Xor(pb, db);
                    }

                    dataStream.Write(db, 0, 8);
                    pb = db;
                    block = new byte[8];
                    bi = -1;
                }
            }

            return dataStream.ToArray();
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
