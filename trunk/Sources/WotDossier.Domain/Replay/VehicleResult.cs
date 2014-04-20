using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WotDossier.Domain.Replay
{
    [DataContract]
    public class VehicleResult
    {
        [DataMember]
        private int _damageAssisted;

        private int _shotsReceived;
        private int _pierced;
        private int _piercedReceived;
        private int _heHits;
        private int _heHitsReceived;
        private int _hits;
        private int _thits;

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
        public int heHitsReceived
        {
            get
            {
                if (_heHitsReceived > 0)
                {
                    return _heHitsReceived;
                }
                return explosionHitsReceived;
            }
            set { _heHitsReceived = value; }
        }

        [DataMember]
        public int explosionHitsReceived { get; set; }

        [DataMember]
        public int he_hits
        {
            get
            {
                if (_heHits > 0)
                {
                    return _heHits;
                }
                return explosionHits;
            }
            set { _heHits = value; }
        }

        [DataMember]
        public int explosionHits { get; set; }
        [DataMember]
        public int health { get; set; }

        [DataMember]
        public int hits
        {
            get
            {
                if (_hits > 0)
                {
                    return _hits;
                }
                return directHits;
            }
            set { _hits = value; }
        }

        [DataMember]
        public int directHits { get; set; }
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
        public int piercedReceived
        {
            get
            {
                if (_piercedReceived > 0)
                {
                    return _piercedReceived;
                }
                return piercingsReceived;
            }
            set { _piercedReceived = value; }
        }

        [DataMember]
        public int piercingsReceived { get; set; }

        [DataMember]
        public int pierced
        {
            get
            {
                if (_pierced > 0)
                {
                    return _pierced;
                }
                return piercings;
            }
            set { _pierced = value; }
        }

        [DataMember]
        public int piercings { get; set; }
        [DataMember]
        public int potentialDamageReceived { get; set; }
        [DataMember]
        public int repair { get; set; }
        [DataMember]
        public int shots { get; set; }

        [DataMember]
        public int shotsReceived
        {
            get
            {
                if (_shotsReceived > 0)
                {
                    return _shotsReceived;
                }
                return directHitsReceived;
            }
            set { _shotsReceived = value; }
        }

        [DataMember]
        public int directHitsReceived { get; set; }
        [DataMember]
        public int spotted { get; set; }
        [DataMember]
        public double tdamageDealt { get; set; }
        [DataMember]
        public int team { get; set; }

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
        public int typeCompDescr { get; set; }
        [DataMember]
        public int xp { get; set; }
    }
}