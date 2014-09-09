using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WotDossier.Domain.Replay
{
    [DataContract]
    public class VehicleResult : StatBase
    {
        [DataMember]
        public long accountDBID { get; set; }
        [DataMember]
        public List<int> achievements { get; set; }
        [DataMember]
        public int capturePoints { get; set; }
        [DataMember]
        public int credits { get; set; }
        [DataMember]
        public int freeXP { get; set; }
        [DataMember]
        public int gold { get; set; }
        [DataMember]
        public int health { get; set; }
        [DataMember]
        public bool isTeamKiller { get; set; }
        [DataMember]
        public int killerID { get; set; }
        [DataMember]
        public int lifeTime { get; set; }
        [DataMember]
        public int mileage { get; set; }
        [DataMember]
        public int repair { get; set; }
        [DataMember]
        public double tdamageDealt { get; set; }
        [DataMember]
        public int team { get; set; }
        private int _thits;
        [DataMember]
        public int thits
        {
            get
            {
                if (_thits > 0)
                {
                    return _thits;
                }
                return directTeamHits;
            }
            set { _thits = value; }
        }
        [DataMember]
        public int directTeamHits { get; set; }
        [DataMember]
        public int tkills { get; set; }
        [DataMember]
        public int? typeCompDescr { get; set; }
        [DataMember]
        public int xp { get; set; }
    }
}