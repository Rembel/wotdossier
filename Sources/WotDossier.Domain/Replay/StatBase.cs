using System.Runtime.Serialization;

namespace WotDossier.Domain.Replay
{
    [DataContract]
    public class StatBase
    {
        private int _damageAssisted;
        private int _heHits;
        private int _heHitsReceived;
        private int _hits;
        private int _noDamageShotsReceived;
        private int _pierced;
        private int _piercedReceived;
        private int _shotsReceived;

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
        public int damageBlockedByArmor { get; set; }

        [DataMember]
        public int damaged { get; set; }

        [DataMember]
        public int damageDealt { get; set; }

        [DataMember]
        public int damageReceived { get; set; }

        [DataMember]
        public int directHits { get; set; }

        [DataMember]
        public int directHitsReceived { get; set; }

        [DataMember]
        public int droppedCapturePoints { get; set; }

        [DataMember]
        public int he_hits
        {
            get
            {
                if (_heHits > 0)
                {
                    return _heHits;
                }
                //Compatibility with older versions
                //Some names changed in WoT 0.9.0
                return explosionHits;
            }
            set { _heHits = value; }
        }

        [DataMember]
        public int heHitsReceived
        {
            get
            {
                if (_heHitsReceived > 0)
                {
                    return _heHitsReceived;
                }
                //Compatibility with older versions
                //Some names changed in WoT 0.9.0
                return explosionHitsReceived;
            }
            set { _heHitsReceived = value; }
        }

        [DataMember]
        public int hits
        {
            get
            {
                if (_hits > 0)
                {
                    return _hits;
                }
                //Compatibility with older versions
                //Some names changed in WoT 0.9.0
                return directHits;
            }
            set { _hits = value; }
        }

        [DataMember]
        public int explosionHits { get; set; }

        [DataMember]
        public int explosionHitsReceived { get; set; }

        [DataMember]
        public int kills { get; set; }

        [DataMember]
        public int movingAvgDamage { get; set; }

        [DataMember]
        public int noDamageShotsReceived
        {
            get
            {
                if (_noDamageShotsReceived > 0)
                {
                    return _noDamageShotsReceived;
                }
                //Compatibility with older versions
                //Some names changed in WoT 0.9.0
                return noDamageDirectHitsReceived;
            }
            set { _noDamageShotsReceived = value; }
        }

        [DataMember]
        public int noDamageDirectHitsReceived { get; set; }

        [DataMember]
        public int piercedReceived
        {
            get
            {
                if (_piercedReceived > 0)
                {
                    return _piercedReceived;
                }
                //Compatibility with older versions
                //Some names changed in WoT 0.9.0
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
                //Compatibility with older versions
                //Some names changed in WoT 0.9.0
                return piercings;
            }
            set { _pierced = value; }
        }

        [DataMember]
        public int piercings { get; set; }

        [DataMember]
        public int potentialDamageReceived { get; set; }

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
                //Compatibility with older versions
                //Some names changed in WoT 0.9.0
                return directHitsReceived;
            }
            set { _shotsReceived = value; }
        }

        [DataMember]
        public int sniperDamageDealt { get; set; }

        [DataMember]
        public int spotted { get; set; }

        private int _deathReason = -1;
        [DataMember]
        public int deathReason
        {
            get { return _deathReason; }
            set { _deathReason = value; }
        }
    }
}