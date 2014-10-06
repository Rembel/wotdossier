using System;
using System.IO;
using Common.Logging;
using WotDossier.Common;
using WotDossier.Common.Extensions;
using WotDossier.Domain.Replay;

namespace WotDossier.Applications.Parser
{
    public class Parser81 : BaseParser
    {
        private static readonly ILog _log = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Process packet 0x08
        /// Contains Various game state updates 
        /// </summary>
        /// <param name="packet">The packet.</param>
        /// <param name="data">The data.</param>
        protected override void ProcessPacket_0x08(Packet packet, AdvancedReplayData data)
        {
            using (MemoryStream stream = new MemoryStream(packet.Payload))
            {
                //read 0-4 - player_id
                packet.PlayerId = stream.Read(4).ConvertLittleEndian();
                //read 4-8 - subType
                packet.SubType = stream.Read(4).ConvertLittleEndian();
                //read 8-12 - update length
                packet.SubTypePayloadLength = stream.Read(4).ConvertLittleEndian();

                if (packet.SubType == 0x17) //onArenaUpdate events
                {
                    ProcessPacket_0x08_0x1d(packet, stream, data);
                }

                if (packet.SubType == 0x0b) //onSlotUpdate events
                {
                    ProcessPacket_0x08_0x09(packet, stream, data);
                }

                if (packet.SubType == 0x01) //onDamageReceived
                {
                    ProcessPacket_0x08_0x01(packet, stream, data);
                }
            }
        }

        /// <summary>
        /// Process packet 0x00
        /// Contains Battle level setup and Player Name.
        /// </summary>
        /// <param name="payload">The payload.</param>
        /// <param name="data">The data.</param>
        public override void ProcessPacket_0x00(byte[] payload, AdvancedReplayData data)
        {
            using (var f = new MemoryStream(payload))
            {
                f.Seek(10, SeekOrigin.Begin);

                int playernamelength = (int)f.Read(1).ConvertLittleEndian();

                data.playername = f.Read(playernamelength).GetUtf8String();

                f.Seek(playernamelength + 24, SeekOrigin.Begin);

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
    }
}