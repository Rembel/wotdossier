using System;

namespace WotDossier.Applications.Parser
{
    public class Packet
    {
        public PacketType Type { get; set; }
        public ulong StreamPacketType { get; set; }
        public ulong StreamSubType { get; set; }
        public ulong PlayerId { get; set; }
        public TimeSpan Time { get; set; }

        public long Offset { get; set; }
        public ulong PacketLength { get; set; }
        public ulong SubTypePayloadLength { get; set; }
        public byte[] Payload { get; set; }
        public float Clock { get; set; }
        public object Data { get; set; }
    }

    public enum PacketType
    {
        Unknown,
        ChatMessage,
        BattleLevel,
        DamageReceived,
        ArenaUpdate,
        SlotUpdate,
        Version,
        PlayerPos,
        MinimapClick,
        Health
    }
}