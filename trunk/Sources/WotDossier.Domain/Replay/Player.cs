using System.Runtime.Serialization;

namespace WotDossier.Domain.Replay
{
    [DataContract]
    public class Player
    {
        [DataMember]
        public string clanAbbrev { get; set; }
        [DataMember]
        public int clanDBID { get; set; }
        [DataMember]
        public string name { get; set; }
        //NOTE: for compatibility with external parser
        [DataMember]
        public int platoonID { get; set; }
        [DataMember]
        public int prebattleID { get; set; }
        [DataMember]
        public int team { get; set; }
    }
}