using System.Runtime.Serialization;

namespace WotDossier.Domain.Replay
{
    [DataContract]
    public class Player
    {
        private int _platoonId;

        [DataMember]
        public string clanAbbrev { get; set; }
        [DataMember]
        public int clanDBID { get; set; }
        [DataMember]
        public string name { get; set; }

        //NOTE: for compatibility with external parser
        [DataMember]
        public int platoonID
        {
            get
            {
                if (_platoonId > 0)
                {
                    return _platoonId;
                }
                return prebattleID;
            }
            set { _platoonId = value; }
        }

        [DataMember]
        public int prebattleID { get; set; }
        [DataMember]
        public int team { get; set; }
    }
}