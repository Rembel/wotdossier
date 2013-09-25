using System.Runtime.Serialization;
using WotDossier.Common;

namespace WotDossier.Domain.Replay
{
    /*
http://wiki.vbaddict.net
    */
    [DataContract]
    public class Replay
    {
        [DataMember]
        public FirstBlock datablock_1 { get; set; }
        [DataMember]
        public CommandResult CommandResult { get; set; }
        [DataMember]
        public BattleResult datablock_battle_result { get; set; }
        [DataMember]
        public ReplayIdentify identify { get; set; }
    }

    public class ReplayIdentify
    {
        [DataMember]
        public long accountDBID { get; set; }
        [DataMember]
        public long arenaCreateTime { get; set; }
        [DataMember]
        public long arenaUniqueID { get; set; }
        [DataMember]
        public int countryid { get; set; }
        [DataMember]
        public string error { get; set; }
        [DataMember]
        public string error_details { get; set; }
        [DataMember]
        public long internaluserID { get; set; }
        [DataMember]
        public string mapName { get; set; }
        [DataMember]
        public int mapid { get; set; }
        [DataMember]
        public string playername { get; set; }
        [DataMember]
        public int tankid { get; set; }

        public int TankUniqueId()
        {
            return Utils.ToUniqueId(countryid, tankid);
        }
    }
}
