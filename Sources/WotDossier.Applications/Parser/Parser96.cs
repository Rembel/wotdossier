using System.IO;
using WotDossier.Common.Extensions;

namespace WotDossier.Applications.Parser
{
    public class Parser96 : BaseParser
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

                //NOTE: 0.9.6 format changed 0x1e->0x21
                if (packet.StreamSubType == 0x21) //onArenaUpdate events
                {
                    ProcessPacket_0x08_0x1d(packet, stream);
                }

                if (packet.StreamSubType == 0x09) //onSlotUpdate events
                {
                    ProcessPacket_0x08_0x09(packet, stream);
                }

                if (packet.StreamSubType == 0x01) //onDamageReceived
                {
                    ProcessPacket_0x08_0x01(packet, stream);
                }
            }
        }
    }
}