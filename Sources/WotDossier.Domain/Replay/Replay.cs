using System.Runtime.Serialization;

namespace WotDossier.Domain.Replay
{
    /// <summary>
    /// http://wiki.vbaddict.net
    /// </summary>
    [DataContract]
    public class Replay
    {
        [DataMember]
        public FirstBlock datablock_1 { get; set; }

        //for versions <0.8.11
        [DataMember]
        public PlayerResult datablock_battle_result_plain { get; set; }

        [DataMember]
        public BattleResult datablock_battle_result { get; set; }

        [DataMember]
        public ReplayIdentify identify { get; set; }
    }
}
