using System.IO;
using WotDossier.Common.Extensions;
using WotDossier.Domain.Replay;

namespace WotDossier.Applications.Parser
{
    public class Parser93 : BaseParser
    {
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

                //NOTE: 0.9.3 format changed 0x1d->0x1e
                if (packet.SubType == 0x1e) //onArenaUpdate events
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
    }
}