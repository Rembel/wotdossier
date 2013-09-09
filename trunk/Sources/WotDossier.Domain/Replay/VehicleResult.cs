using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WotDossier.Domain.Replay
{
    [DataContract]
    public class VehicleResult
    {
        [DataMember]
        private int _damageAssisted;
        [DataMember]
        public long accountDBID { get; set; }
        [DataMember]
        public List<int> achievements { get; set; }
        [DataMember]
        public int capturePoints { get; set; }
        [DataMember]
        public int credits { get; set; }
        [DataMember]
        public int damageAssisted
        {
            get
            {
                int result = damageAssistedRadio + damageAssistedTrack;
                return result > 0 ? result : _damageAssisted;
            }
            set { _damageAssisted = value; }
        }
        [DataMember]
        public int damageAssistedRadio { get; set; }
        [DataMember]
        public int damageAssistedTrack { get; set; }
        [DataMember]
        public int damageDealt { get; set; }
        [DataMember]
        public int damageReceived { get; set; }
        [DataMember]
        public int damaged { get; set; }
        [DataMember]
        public int deathReason { get; set; }
        [DataMember]
        public int droppedCapturePoints { get; set; }
        [DataMember]
        public int freeXP { get; set; }
        [DataMember]
        public int gold { get; set; }
        [DataMember]
        public int heHitsReceived { get; set; }
        [DataMember]
        public int he_hits { get; set; }
        [DataMember]
        public int health { get; set; }
        [DataMember]
        public int hits { get; set; }
        [DataMember]
        public bool isTeamKiller { get; set; }
        [DataMember]
        public int killerID { get; set; }
        [DataMember]
        public int kills { get; set; }
        [DataMember]
        public int lifeTime { get; set; }
        [DataMember]
        public int mileage { get; set; }
        [DataMember]
        public int noDamageShotsReceived { get; set; }
        [DataMember]
        public int piercedReceived { get; set; }
        [DataMember]
        public int pierced { get; set; }
        [DataMember]
        public int potentialDamageReceived { get; set; }
        [DataMember]
        public int repair { get; set; }
        [DataMember]
        public int shots { get; set; }
        [DataMember]
        public int shotsReceived { get; set; }
        [DataMember]
        public int spotted { get; set; }
        [DataMember]
        public double tdamageDealt { get; set; }
        [DataMember]
        public int team { get; set; }
        [DataMember]
        public int thits { get; set; }
        [DataMember]
        public int tkills { get; set; }
        [DataMember]
        public int typeCompDescr { get; set; }
        [DataMember]
        public int xp { get; set; }
    }
}