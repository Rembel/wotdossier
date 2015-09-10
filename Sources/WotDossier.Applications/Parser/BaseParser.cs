using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Common.Logging;
using WotDossier.Common;
using WotDossier.Common.Extensions;
using WotDossier.Domain;
using WotDossier.Domain.Replay;

namespace WotDossier.Applications.Parser
{
    public class BaseParser
    {
        protected static readonly ILog _log = LogManager.GetCurrentClassLogger();
        private bool _abort = false;

        public void ReadReplayStream(Stream stream, Action<Packet> packetHandler)
        {
            _abort = false;
            bool endOfStream = false;
            _log.Trace("Begin replay stream read");
            while (!endOfStream && !_abort)
            {
                var readPacket = ReadPacket(stream);
                endOfStream = readPacket == null;
                if (!endOfStream)
                {
                    packetHandler(readPacket);
                }
            }
            _log.Trace("End replay stream read");
        }

        public void Abort()
        {
            _abort = true;
        }

        private Packet ReadPacket(Stream stream)
        {
            ulong packetLength = stream.Read(4).ConvertLittleEndian();
            ulong packetType = stream.Read(4).ConvertLittleEndian();
            float time = stream.Read(4).ToSingle();
            
            long position = stream.Position;

            bool endOfStream = packetType == new byte[] { 255, 255, 255, 255 }.ConvertLittleEndian() || stream.Position >= stream.Length;

            byte[] payload = new byte[packetLength];

            Packet packet = null;
            
            if (!endOfStream)
            {
                stream.Read(payload, 0, (int)packetLength);

                packet = new Packet
                {
                    Payload = payload,
                    StreamPacketType = packetType,
                    PacketLength = packetLength,
                    Offset = position,
                    Time = TimeSpan.FromSeconds(time),
                    Clock = time
                };

                //battle level setup 
                if (packet.StreamPacketType == 0x00)
                {
                    _log.Trace("Process packet 0x00");
                    ProcessPacket_0x00(packet);
                }

                //player position
                if (packet.StreamPacketType == 0x0a)
                {
                    _log.Trace("Process packet 0x0a");
                    ProcessPacket_0x0a(packet);
                }

                //minimap click
                if (packet.StreamPacketType == 0x21)
                {
                    _log.Trace("Process packet 0x21");
                    ProcessPacket_0x21(packet);
                }

                //replay version
                if (packet.StreamPacketType == 0x14)
                {
                    _log.Trace("Process packet 0x14");
                    ProcessPacket_0x14(packet);
                }

                //in game updates
                if (packet.StreamPacketType == 0x08)
                {
                    _log.Trace("Process packet 0x08");
                    ProcessPacket_0x08(packet);
                }

                if (packet.StreamPacketType == 0x07)
                {
                    _log.Trace("Process packet 0x07");
                    ProcessPacket_0x07(packet);
                }

                //chat
                if (packet.StreamPacketType == 0x1f)
                {
                    _log.Trace("Process packet 0x1f");
                    ProcessPacket_0x1f(packet);
                }
            }

            return packet;
        }

        private void ProcessPacket_0x07(Packet packet)
        {
            packet.Type = PacketType.Health;

            dynamic data = new ExpandoObject();

            packet.Data = data;

            using (MemoryStream stream = new MemoryStream(packet.Payload))
            {
                //read 0-4 - player_id
                packet.PlayerId = stream.Read(4).ConvertLittleEndian();
                //read 4-8 - subType
                packet.StreamSubType = stream.Read(4).ConvertLittleEndian();
                //read 8-12 - update length
                packet.SubTypePayloadLength = stream.Read(4).ConvertLittleEndian();

                if (packet.StreamSubType == 0x03)
                {
                    int value = BitConverter.ToInt16(stream.Read(2), 0);
                    data.health = value < 0 ? 0 : value;
                }
            }
        }

        private void ProcessPacket_0x21(Packet packet)
        {
            packet.Type = PacketType.MinimapClick;

            dynamic data = new ExpandoObject();

            packet.Data = data;

            using (MemoryStream f = new MemoryStream(packet.Payload))
            {
                int cellId = BitConverter.ToInt16(f.Read(2), 0);
                int cellLeft = (int) Math.Floor(cellId/10.0);
                int cellTop = cellId - (cellLeft*10);

                data.cellId = cellId;
                data.cellLeft = cellLeft;
                data.cellTop = cellTop;
            }
        }

        private void ProcessPacket_0x0a(Packet packet)
        {
            packet.Type = PacketType.PlayerPos;

            dynamic data = new ExpandoObject();

            packet.Data = data;

            using (MemoryStream f = new MemoryStream(packet.Payload))
            {
                data.PlayerId = (long)f.Read(4).ConvertLittleEndian();

                f.Seek(12, SeekOrigin.Begin);

                var pos1 = f.Read(4).ToSingle();
                var pos2 = f.Read(4).ToSingle();
                var pos3 = f.Read(4).ToSingle();
                data.position = new[] { pos1, pos2, pos3 };

                f.Seek(36, SeekOrigin.Begin);

                var hull1 = f.Read(4).ToSingle();
                var hull2 = f.Read(4).ToSingle();
                var hull3 = f.Read(4).ToSingle();
                data.hull_orientation = new[] { hull1, hull2, hull3 };
            }
        }

        /// <summary>
        /// Process packet 0x00
        /// Contains Battle level setup and Player Name.
        /// </summary>
        /// <param name="packet">The packet.</param>
        public virtual void ProcessPacket_0x00(Packet packet)
        {
            packet.Type = PacketType.BattleLevel;

            dynamic data = new ExpandoObject();

            packet.Data = data;

            using (var f = new MemoryStream(packet.Payload))
            {
                f.Seek(10, SeekOrigin.Begin);

                int playernamelength = (int)f.Read(1).ConvertLittleEndian();

                data.playername = f.Read(playernamelength).GetUtf8String();

                f.Seek(playernamelength + 25, SeekOrigin.Begin);

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
        protected virtual void ProcessPacket_0x08(Packet packet)
        {
            using (MemoryStream stream = new MemoryStream(packet.Payload))
            {
                //read 0-4 - player_id
                packet.PlayerId = stream.Read(4).ConvertLittleEndian();
                //read 4-8 - subType
                packet.StreamSubType = stream.Read(4).ConvertLittleEndian();
                //read 8-12 - update length
                packet.SubTypePayloadLength = stream.Read(4).ConvertLittleEndian();

                if (packet.StreamSubType == 0x1d) //onArenaUpdate events
                {
                    ProcessPacket_0x08_0x1d(packet, stream);
                }

                if (packet.StreamSubType == 0x09) //onSlotUpdate events
                {
                    ProcessPacket_0x08_0x09(packet, stream);
                }

                if (packet.StreamSubType == 0x01) //onDamageReceived
                {
                    //ProcessPacket_0x08_0x01(packet, stream, data);
                }
            }
        }

        protected static void ProcessPacket_0x08_0x01(Packet packet, MemoryStream stream)
        {
            packet.Type = PacketType.DamageReceived;

            ulong health = stream.Read(2).ConvertLittleEndian();
            ulong source = stream.Read(4).ConvertLittleEndian();

            dynamic data = new ExpandoObject();

            packet.Data = data;

            //packet.damageReceived = (int)health;
            //packet.damageReceived = new DamageReceived { Health = (int)health, Source = (int)source };
        }

        /// <summary>
        /// Process packet 0x08 subType 0x1d
        /// http://wiki.vbaddict.net/pages/Packet_0x08
        /// </summary>
        /// <param name="packet">The packet.</param>
        /// <param name="stream">The stream.</param>
        protected static void ProcessPacket_0x08_0x1d(Packet packet, MemoryStream stream)
        {
            packet.Type = PacketType.ArenaUpdate;

            dynamic data = new ExpandoObject();

            packet.Data = data;

            ulong updateType = stream.Read(1).ConvertLittleEndian();

            data.updateType = updateType;

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

            //Read from your offset to the end of the packet, this will be the "update pickle". 
            byte[] updatePayload = stream.Read((int)(packet.SubTypePayloadLength));

            //Updates the vehicle list; also known as the roster
            if (updateType == 0x01)
            {
                var rosterdata = new Dictionary<string, AdvancedPlayerInfo>();
                data.roster = rosterdata;

                List<object> rosters = new List<object>();

                try
                {
                    using (var updatePayloadStream = new MemoryStream(updatePayload))
                    {
                        rosters = (List<object>)Unpickle.Load(updatePayloadStream);
                    }
                }
                catch (Exception e)
                {
                    _log.Error("Error on roster load", e);
                }

                foreach (object[] roster in rosters)
                {
                    string key = (string)roster[2];
                    rosterdata[key] = new AdvancedPlayerInfo();
                    rosterdata[key].internaluserID = (int)roster[0];
                    rosterdata[key].playerName = key;
                    rosterdata[key].team = (int)roster[3];
                    rosterdata[key].accountDBID = (int)roster[7];
                    rosterdata[key].clanAbbrev = (string)roster[8];
                    rosterdata[key].clanID = (int)roster[9];
                    rosterdata[key].prebattleID = (int)roster[10];

                    var bindataBytes = Encoding.GetEncoding(1252).GetBytes((string)roster[1]);
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
                                int m = (int)bindataBytes.Skip(pos).Take(2).ToArray().ConvertLittleEndian();
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

            if (updateType == 0x08)
            {
                try
                {
                    using (var updatePayloadStream = new MemoryStream(updatePayload))
                    {
                        object [] update = (object[]) Unpickle.Load(updatePayloadStream);
                        data.team = (int)update[0];
                        data.baseID = (int)update[1];
                        data.points = (int)update[2];
                        data.capturingStopped = (int)update[3];
                    }
                }
                catch (Exception e)
                {
                    _log.Error("Error on update load", e);
                }
            }

            if (updateType == 0x03)
            {
                try
                {
                    using (var updatePayloadStream = new MemoryStream(updatePayload))
                    {
                        object [] update = (object[]) Unpickle.Load(updatePayloadStream);
                        data.period = update[0];
                        data.period_end = update[1];
                        data.period_length = Convert.ToInt32(update[2]);
                        data.activities = update[3];
                    }
                }
                catch (Exception e)
                {
                    _log.Error("Error on update load", e);
                }
            }

            if (updateType == 0x06)
            {
                try
                {
                    using (var updatePayloadStream = new MemoryStream(updatePayload))
                    {
                        object[] update = (object[])Unpickle.Load(updatePayloadStream);
                        data.destroyed = update[0];
                        data.destroyer = update[1];
                        data.reason = update[2];
                    }
                }
                catch (Exception e)
                {
                    _log.Error("Error on update load", e);
                }
            }
        }

        /// <summary>
        /// Process packet 0x08 subType 0x09
        /// Contains slots updates
        /// </summary>
        /// <param name="packet">The packet.</param>
        /// <param name="stream">The stream.</param>
        protected static void ProcessPacket_0x08_0x09(Packet packet, MemoryStream stream)
        {
            packet.Type = PacketType.SlotUpdate;
            //buffer = new byte[packet.SubTypePayloadLength];
            ////Read from your offset to the end of the packet, this will be the "update pickle". 
            //stream.Read(buffer, 0, (int) (packet.SubTypePayloadLength));

            dynamic data = new ExpandoObject();

            packet.Data = data;

            ulong value = stream.Read(4).ConvertLittleEndian();
            var item = new SlotItem((SlotType)(value & 15), (int) (value >> 4 & 15), (int) (value >> 8 & 65535));

            ulong count = stream.Read(2).ConvertLittleEndian();

            ulong rest = stream.Read(3).ConvertLittleEndian();

            data.Slot = new Slot(item, (int) count, (int) rest);
        }

        /// <summary>
        /// Process packet 0x14
        /// Contains Replay version
        /// </summary>
        /// <param name="packet">The packet.</param>
        private static void ProcessPacket_0x14(Packet packet)
        {
            packet.Type = PacketType.Version;

            dynamic data = new ExpandoObject();

            packet.Data = data;

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
        private static void ProcessPacket_0x1f(Packet packet)
        {
            packet.Type = PacketType.ChatMessage;

            dynamic data = new ExpandoObject();

            packet.Data = data;

            string message = Encoding.UTF8.GetString(packet.Payload);
            data.Message = ParseChatMessage(message.Replace("&nbsp;", " ").Replace(":", ""), packet.Time);
        }

        public static ChatMessage ParseChatMessage(string messageText, TimeSpan time)
        {
            var reg = new Regex(@"<(?<tag>[\w]+)[^>]*color\s*=\s*['""](?<color>[^'""]+)['""][^>]*>(?<text>.*?)<\/\<tag>", RegexOptions.IgnoreCase);
            MatchCollection match = reg.Matches(messageText);
            if (match.Count == 2)
            {
                return new ChatMessage
                {
                    Player = match[0].Groups["text"].Value.Trim(),
                    PlayerColor = match[0].Groups["color"].Value.Trim(),
                    Text = match[1].Groups["text"].Value.Trim(),
                    TextColor = match[1].Groups["color"].Value.Trim(),
                    Time = time
                };
            }
            return null;
        }
    }
}
