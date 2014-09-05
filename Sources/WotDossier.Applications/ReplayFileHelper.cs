using System;
using System.Collections.Generic;
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
using WotDossier.Domain;
using WotDossier.Domain.Replay;

namespace WotDossier.Applications
{
    public static class ReplayFileHelper
    {
private const string REPLAY_DATABLOCK_2 = "datablock_2";

        private static readonly ILog _log = LogManager.GetCurrentClassLogger();
        private static readonly Version _jsonFormatedReplayMinVersion = new Version("0.8.11.0");
        
        public static Replay ParseReplay(FileInfo phisicalFile, Version clientVersion, bool readAdvancedData)
        {
            if (clientVersion < _jsonFormatedReplayMinVersion)
            {
                return ParseReplay_8_0(phisicalFile, readAdvancedData);
            }
            return ParseReplay_8_11(phisicalFile, readAdvancedData);
        }

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

            ExternalTask.Execute(task, arguments, logPath, workingDirectory);

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
            string jsonFile = ReplayToJson(replayFile, false);

            string content = File.ReadAllText(jsonFile);

            JObject jObject = JsonConvert.DeserializeObject<JObject>(content);

            Replay replay = jObject.ToObject<Replay>();

            if (((IDictionary<string, JToken>)jObject).ContainsKey(REPLAY_DATABLOCK_2))
            {
                replay.datablock_1.vehicles = jObject[REPLAY_DATABLOCK_2][1].ToObject<Dictionary<long, Vehicle>>();
            }

            if (readAdvancedData)
            {
                string path = replayFile.FullName;
                if (File.Exists(path))
                {
                    using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        int blocksCount = 0;
                        byte[] buffer = new byte[4];
                        stream.Read(buffer, 0, 4);

                        if (buffer[0] != 0x21)
                        {
                            blocksCount = (int) stream.Read(4).ConvertLittleEndian();
                            _log.Trace("Found Replay Blocks: " + blocksCount);
                        }

                        for (int i = 0; i < blocksCount; i++)
                        {
                            int blockLength = (int)stream.Read(4).ConvertLittleEndian();
                            stream.Seek(blockLength, SeekOrigin.Current);
                        }

                        ReadAdvancedDataBlock(stream, replay);
                    }
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
            _log.Trace("Parse Replay: " + file);

            string path = file.FullName;
            if (File.Exists(path))
            {
                using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    Replay replay = new Replay();

                    try
                    {
                        int blocksCount = 0;
                        byte[] buffer = new byte[4];
                        stream.Read(buffer, 0, 4);

                        if (buffer[0] != 0x21)
                        {
                            blocksCount = (int) stream.Read(4).ConvertLittleEndian();
                            _log.Trace("Found Replay Blocks: " + blocksCount);
                        }

                        for (int i = 0; i < blocksCount; i++)
                        {
                            //read first block
                            if (i == 0)
                            {
                                ReadFirstBlock(stream, replay);
                            }

                            //read second block
                            if (i == 1)
                            {
                                ReadSecondBlock(stream, replay);
                            }

                            //read third block for replays 0.8.1-0.8.10
                            if (i == 2)
                            {
                                ReadThirdBlock(stream, replay);
                            }
                        }

                        if (replay.datablock_1 != null)
                        {
                            //read advanced data block
                            if (readAdvancedData)
                            {
                                ReadAdvancedDataBlock(stream, replay);
                            }
                            return replay;
                        }
                    }
                    catch (Exception e)
                    {
                        _log.ErrorFormat("Error on replay file read. Incorrect file format({0})", e, file.FullName);
                    }
                }
            }
            return null;
        }

        private static void ReadFirstBlock(FileStream stream, Replay replay)
        {
            int blockLength = (int) stream.Read(4).ConvertLittleEndian();
            _log.Trace("1 block length: " + blockLength);

            string blockData = stream.Read(blockLength).GetAsciiString();
            if (!string.IsNullOrEmpty(blockData))
            {
                replay.datablock_1 = JsonConvert.DeserializeObject<FirstBlock>(blockData);
            }
        }

        private static void ReadSecondBlock(FileStream stream, Replay replay)
        {
            int blockLength = (int) stream.Read(4).ConvertLittleEndian();
            _log.Trace("2 block length: " + blockLength);
            string blockData = stream.Read(blockLength).GetAsciiString();

            var parsedData = JsonConvert.DeserializeObject<JArray>(blockData);
            if (parsedData.Count > 0)
            {
                //0.8.11+
                if (replay.datablock_1.Version >= _jsonFormatedReplayMinVersion)
                {
                    replay.datablock_battle_result = parsedData[0].ToObject<BattleResult>();
                    replay.datablock_1.vehicles = parsedData[1].ToObject<Dictionary<long, Vehicle>>();
                }
            }
        }

        private static void ReadThirdBlock(FileStream stream, Replay replay)
        {
            int blockLength = (int) stream.Read(4).ConvertLittleEndian();
            _log.Trace("3 block length: " + blockLength);
            byte[] bytes = stream.Read(blockLength);
            object blockData = Unpickle.Load(new MemoryStream(bytes));
            replay.datablock_battle_result = blockData.ToObject<BattleResult>();
        }

        private static void ReadAdvancedDataBlock(FileStream stream, Replay replay)
        {
            _log.Trace("Start read advanced data");
            const int startPosition = 8;
            stream.Seek(startPosition, SeekOrigin.Current);

            int replayDataLength = (int)(stream.Length - stream.Position) - startPosition;

            byte[] decrypt = DecryptReplayData(stream.Read(replayDataLength));
            byte[] uncompressed = DecompressReplayData(decrypt);

            //uncompressed.Dump(@"c:\\temp");

            using (var uncompressedReplayStream = new MemoryStream(uncompressed))
            {
                replay.datablock_advanced = ReadReplayStream(uncompressedReplayStream);
            }
            _log.Trace("End read advanced data");
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
                    _log.Error("Cannot load battle info pickle object. \nPosition: " + f.Position + ", Length: " + advancedlength, e);
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

                if (packet.SubType == 0x01) //onDamageReceived
                {
                    ProcessPacket_0x08_0x01(packet, stream, data);
                }
            }
        }

        private static void ProcessPacket_0x08_0x01(Packet packet, MemoryStream stream, AdvancedReplayData data)
        {
            //ulong health = stream.Read(2).ConvertLittleEndian();
            //ulong source = stream.Read(4).ConvertLittleEndian();
            //data.DamageReceived = new DamageReceived {Health = (int) health, Source = (int)source};
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
                ulong firstByte = 0x0;
                ulong secondByte = 0x0;

                ulong ofset = 0;

                //find pickle object start marker
                while (firstByte != 0x80 || secondByte != 0x02)
                {
                    firstByte = stream.Read(1).ConvertLittleEndian();
                    secondByte = stream.Read(1).ConvertLittleEndian();
                    stream.Seek(-1, SeekOrigin.Current);
                    ofset++;
                }

                stream.Seek(-1, SeekOrigin.Current);
                packet.SubTypePayloadLength = packet.SubTypePayloadLength - ofset;
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

                List<object> rosters = new List<object>();

                try
                {
                    using (var updatePayloadStream = new MemoryStream(updatePayload))
                    {
                        rosters = (List<object>) Unpickle.Load(updatePayloadStream);
                    }
                }
                catch (Exception e)
                {
                    _log.Error("Error on roster load", e);
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

        public static Version ResolveVersion(Version version, DateTime playTime)
        {
            if (version == new Version(0, 0))
            {
                if (playTime.Date >= new DateTime(2013, 4, 18))
                {
                    return new Version("0.8.5.0");
                }

                if (playTime.Date >= new DateTime(2013, 2, 28))
                {
                    return new Version("0.8.4.0");
                }

                if (playTime.Date >= new DateTime(2013, 1, 16))
                {
                    return new Version("0.8.3.0");
                }

                if (playTime.Date >= new DateTime(2012, 12, 8))
                {
                    return new Version("0.8.2.0");
                }

                if (playTime.Date >= new DateTime(2012, 10, 25))
                {
                    return new Version("0.8.1.0");
                }
            }
            return version;
        }
    }
    
    internal class DamageReceived
    {
        public int Health { get; set; }
        public int Source { get; set; }
    }
}