using System;
using System.Collections.Generic;
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
        private static readonly ILog _log = LogManager.GetCurrentClassLogger();

        public AdvancedReplayData ReadReplayStream(Stream stream)
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

        private bool ReadPacket(Stream stream, AdvancedReplayData data)
        {
            ulong packetLength = stream.Read(4).ConvertLittleEndian();
            ulong packetType = stream.Read(4).ConvertLittleEndian();
            float time = BitConverter.ToSingle(stream.Read(4), 0);
            
            long position = stream.Position;

            bool endOfStream = packetType == new byte[] { 255, 255, 255, 255 }.ConvertLittleEndian() || stream.Position >= stream.Length;

            byte[] payload = new byte[packetLength];

            if (!endOfStream)
            {
                stream.Read(payload, 0, (int)packetLength);

                var packet = new Packet
                {
                    Payload = payload,
                    PacketType = packetType,
                    PacketLength = packetLength,
                    Position = position,
                    Time = TimeSpan.FromSeconds(time)
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
        /// <param name="data">The data.</param>
        protected virtual void ProcessPacket_0x08(Packet packet, AdvancedReplayData data)
        {
            using (MemoryStream stream = new MemoryStream(packet.Payload))
            {
                //read 0-4 - player_id
                packet.PlayerId = stream.Read(4).ConvertLittleEndian();
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
                    //ProcessPacket_0x08_0x01(packet, stream, data);
                }
            }
        }

        protected static void ProcessPacket_0x08_0x01(Packet packet, MemoryStream stream, AdvancedReplayData data)
        {
            ulong health = stream.Read(2).ConvertLittleEndian();
            ulong source = stream.Read(4).ConvertLittleEndian();
            var damageReceived = new DamageReceived {Health = (int) health, Source = (int)source};
        }

        /// <summary>
        /// Process packet 0x08 subType 0x1d
        /// http://wiki.vbaddict.net/pages/Packet_0x08
        /// </summary>
        /// <param name="packet">The packet.</param>
        /// <param name="stream">The stream.</param>
        /// <param name="data">The data.</param>
        protected static void ProcessPacket_0x08_0x1d(Packet packet, MemoryStream stream, AdvancedReplayData data)
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
                byte[] updatePayload = stream.Read((int)(packet.SubTypePayloadLength));

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

        }

        /// <summary>
        /// Process packet 0x08 subType 0x09
        /// Contains slots updates
        /// </summary>
        /// <param name="packet">The packet.</param>
        /// <param name="stream">The stream.</param>
        /// <param name="data">The data.</param>
        protected static void ProcessPacket_0x08_0x09(Packet packet, MemoryStream stream, AdvancedReplayData data)
        {
            //buffer = new byte[packet.SubTypePayloadLength];
            ////Read from your offset to the end of the packet, this will be the "update pickle". 
            //stream.Read(buffer, 0, (int) (packet.SubTypePayloadLength));

            ulong value = stream.Read(4).ConvertLittleEndian();
            var item = new SlotItem((SlotType)(value & 15), (int) (value >> 4 & 15), (int) (value >> 8 & 65535));

            ulong count = stream.Read(2).ConvertLittleEndian();

            ulong rest = stream.Read(3).ConvertLittleEndian();

            var slot = new Slot(item, (int) count, (int) rest);

            var foundItem = data.Slots.FirstOrDefault(x => x.Item.Equals(item));

            if (foundItem == null)
            {
                data.Slots.Add(slot);
            }
            else
            {
                foundItem.EndCount = slot.Count + slot.Rest;
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
            var chatMessage = ParseChatMessage(message.Replace("&nbsp;", " ").Replace(":", ""), packet.Time);
            if (chatMessage != null)
            {
                data.Messages.Add(chatMessage);
            }
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
