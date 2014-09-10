using System;

namespace WotDossier.Applications
{
    public class Packet
    {
        public byte[] Payload { get; set; }
        public ulong PacketType { get; set; }
        public ulong PacketLength { get; set; }
        public long Position { get; set; }
        public ulong SubType { get; set; }
        public ulong SubTypePayloadLength { get; set; }
        public ulong PlayerId { get; set; }
        public TimeSpan Time { get; set; }
    }
}