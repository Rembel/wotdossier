using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using WotDossier.Common;
using WotDossier.Common.Extensions;
using WotDossier.Domain.Replay;

namespace WotDossier.Applications.Parser
{
    public class Parser98 : BaseParser
    {
        private bool RosterProcessed = false;

        protected override ulong UpdateEvent_Slot
        {
            get { return 0x09; }
        }

        protected override ulong UpdateEvent_Arena
        {
            get { return 0x22; }
        }

        /// <summary>
        /// Process packet 0x08 subType 0x1d
        /// http://wiki.vbaddict.net/pages/Packet_0x08
        /// </summary>
        /// <param name="packet">The packet.</param>
        /// <param name="stream">The stream.</param>
        protected override void ProcessPacket_0x08_0x1d(Packet packet, MemoryStream stream)
        {
            packet.Type = PacketType.ArenaUpdate;

            dynamic data = new ExpandoObject();

            packet.Data = data;

            ulong updateType = stream.Read(1).ConvertLittleEndian();

            data.updateType = updateType;

            //First packet of this subtype contains pickled roster. BUT compressed using zlib compression. 
            if (updateType == 0x01 || updateType == 0x04)
            {
                var offset = 17;
                stream.Seek(offset, SeekOrigin.Begin);
            }
            else
            {
                stream.Seek(1, SeekOrigin.Current);
                packet.SubTypePayloadLength = packet.SubTypePayloadLength - 1;
            }

            //Read from your offset to the end of the packet, this will be the "update pickle". 
            byte[] updatePayload = stream.Read((int)(packet.SubTypePayloadLength));

            if (!RosterProcessed)
            {
                updatePayload = DecompressData(updatePayload);
                RosterProcessed = true;
            }

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