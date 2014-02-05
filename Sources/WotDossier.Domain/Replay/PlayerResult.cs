using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WotDossier.Domain.Replay
{
    [DataContract]
    public class PlayerResult
    {
        [DataMember]
        public List<int> achieveIndices { get; set; }
        [DataMember]
        public int arenaCreateTime { get; set; }
        [DataMember]
        public int arenaTypeID { get; set; }
        [DataMember]
        public int capturePoints { get; set; }
        [DataMember]
        public int credits { get; set; }
        [DataMember]
        public int damageDealt { get; set; }
        [DataMember]
        public int damageReceived { get; set; }
        [DataMember]
        public List<int> damaged { get; set; }

        [DataMember]
        public int droppedCapturePoints { get; set; }
        [DataMember]
        public Factors factors { get; set; }
        [DataMember]
        public List<int> heroVehicleIDs { get; set; }
        [DataMember]
        public int hits { get; set; }
        [DataMember]
        public int isWinner { get; set; }
        [DataMember]
        public List<int> killed { get; set; }

        [DataMember]
        public int killerID { get; set; }
        [DataMember]
        public int repair { get; set; }
        [DataMember]
        public int shots { get; set; }
        [DataMember]
        public int shotsReceived { get; set; }
        [DataMember]
        public List<int> spotted { get; set; }
        [DataMember]
        public int xp { get; set; }
    }
}