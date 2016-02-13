using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using WotDossier.Common;
using WotDossier.Common.Extensions;
using WotDossier.Domain.Replay;

namespace WotDossier.Applications.Parser
{
    public class Parser914 : BaseParser
    {
        /// <summary>
        /// Process packet 0x08
        /// Contains Various game state updates
        /// </summary>
        /// <param name="packet">The packet.</param>
        protected override void ProcessPacket_0x08(Packet packet)
        {
            using (MemoryStream stream = new MemoryStream(packet.Payload))
            {
                //read 0-4 - player_id
                packet.PlayerId = stream.Read(4).ConvertLittleEndian();
                //read 4-8 - subType
                packet.StreamSubType = stream.Read(4).ConvertLittleEndian();
                //read 8-12 - update length
                packet.SubTypePayloadLength = stream.Read(4).ConvertLittleEndian();

                //NOTE: 0.9.14 format changed 0x28->0x29
                if (packet.StreamSubType == 0x29) //onArenaUpdate events
                {
                    ProcessPacket_0x08_0x1d_(packet, stream);
                }

                if (packet.StreamSubType == 0x0c) //onSlotUpdate events
                {
                    ProcessPacket_0x08_0x09(packet, stream);
                }

                if (packet.StreamSubType == 0x01) //onDamageReceived
                {
                    ProcessPacket_0x08_0x01(packet, stream);
                }
            }
        }

        /// <summary>
        /// Process packet 0x08 subType 0x1d
        /// http://wiki.vbaddict.net/pages/Packet_0x08
        /// </summary>
        /// <param name="packet">The packet.</param>
        /// <param name="stream">The stream.</param>
        protected static void ProcessPacket_0x08_0x1d_(Packet packet, MemoryStream stream)
        {
            packet.Type = PacketType.ArenaUpdate;

            dynamic data = new ExpandoObject();

            packet.Data = data;

            ulong updateType = stream.Read(1).ConvertLittleEndian();

            data.updateType = updateType;

            //For update types 0x01 and 0x04: at offset 14, read an uint16 and unpack it to 2 bytes, 
            //if the unpacked value matches 0x80, 0x02 then set your offset to 14. 
            //If the unpacked value does not match 0x80, 0x02 set your offset to 17. 
            //if (updateType == 0x01 || updateType == 0x04)
            //{
            //    ulong firstByte = 0x0;
            //    ulong secondByte = 0x0;

            //    ulong ofset = 0;

            //    //find pickle object start marker
            //    while (firstByte != 0x82 || secondByte != 0x06)
            //    {
            //        firstByte = stream.Read(1).ConvertLittleEndian();
            //        secondByte = stream.Read(1).ConvertLittleEndian();
            //        stream.Seek(-1, SeekOrigin.Current);
            //        ofset++;
            //    }

            //    stream.Seek(-1, SeekOrigin.Current);
            //    packet.SubTypePayloadLength = packet.SubTypePayloadLength - ofset;
            //}
            //else
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

            //    List<object> rosters = new List<object>();

            //    try
            //    {
            //        using (var updatePayloadStream = new MemoryStream(updatePayload))
            //        {
            //            rosters = (List<object>)Unpickle.Load(updatePayloadStream);
            //        }
            //    }
            //    catch (Exception e)
            //    {
            //        _log.Error("Error on roster load", e);
            //    }

            //    foreach (object[] roster in rosters)
            //    {
            //        string key = (string)roster[2];
            //        rosterdata[key] = new AdvancedPlayerInfo();
            //        rosterdata[key].internaluserID = (int)roster[0];
            //        rosterdata[key].playerName = key;
            //        rosterdata[key].team = (int)roster[3];
            //        rosterdata[key].accountDBID = (int)roster[7];
            //        rosterdata[key].clanAbbrev = (string)roster[8];
            //        rosterdata[key].clanID = (int)roster[9];
            //        rosterdata[key].prebattleID = (int)roster[10];

            //        var bindataBytes = Encoding.GetEncoding(1252).GetBytes((string)roster[1]);
            //        List<int> bindata = bindataBytes.Unpack("BBHHHHHHB");

            //        rosterdata[key].countryID = bindata[0] >> 4 & 15;
            //        rosterdata[key].tankID = bindata[1];
            //        int compDescr = (bindata[1] << 8) + bindata[0];
            //        rosterdata[key].compDescr = compDescr;

            //        //Does not make sense, will check later
            //        rosterdata[key].vehicle = new AdvancedVehicleInfo();
            //        rosterdata[key].vehicle.chassisID = bindata[2];
            //        rosterdata[key].vehicle.engineID = bindata[3];
            //        rosterdata[key].vehicle.fueltankID = bindata[4];
            //        rosterdata[key].vehicle.radioID = bindata[5];
            //        rosterdata[key].vehicle.turretID = bindata[6];
            //        rosterdata[key].vehicle.gunID = bindata[7];

            //        int flags = bindata[8];
            //        int optionalDevicesMask = flags & 15;
            //        int idx = 2;
            //        int pos = 15;

            //        while (optionalDevicesMask != 0)
            //        {
            //            if ((optionalDevicesMask & 1) == 1)
            //            {
            //                try
            //                {
            //                    int m = (int)bindataBytes.Skip(pos).Take(2).ToArray().ConvertLittleEndian();
            //                    rosterdata[key].vehicle.module[idx] = m;
            //                }
            //                catch (Exception e)
            //                {
            //                    _log.Error("error on processing player [" + key + "]: ", e);
            //                }
            //            }

            //            optionalDevicesMask = optionalDevicesMask >> 1;
            //            idx = idx - 1;
            //            pos = pos + 2;

            //        }
            //    }
            }

            if (updateType == 0x08)
            {
                try
                {
                    using (var updatePayloadStream = new MemoryStream(updatePayload))
                    {
                        object[] update = (object[])Unpickle.Load(updatePayloadStream);
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
                        object[] update = (object[])Unpickle.Load(updatePayloadStream);
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
    }
}