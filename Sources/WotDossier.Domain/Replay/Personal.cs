using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json.Linq;

namespace WotDossier.Domain.Replay
{
    [DataContract]
    public class Personal
    {
        [DataMember]
        private int _damageAssisted;

        private int _shotsReceived;
        private int _heHitsReceived;
        private int _heHits;
        private int _hits;
        private int _piercedReceived;
        private int _pierced;
        private int _noDamageShotsReceived;

        [DataMember]
        public int accountDBID { get; set; }
        [DataMember]
        public List<object> achievements { get; set; }
        [DataMember]
        public int aogasFactor10 { get; set; }
        [DataMember]
        public List<int> autoEquipCost { get; set; }
        [DataMember]
        public List<int> autoLoadCost { get; set; }
        [DataMember]
        public int? autoRepairCost { get; set; }
        [DataMember]
        public int capturePoints { get; set; }
        [DataMember]
        public int credits { get; set; }
        [DataMember]
        public int creditsContributionIn { get; set; }
        [DataMember]
        public int creditsContributionOut { get; set; }
        [DataMember]
        public int creditsPenalty { get; set; }
        [DataMember]
        public int dailyXPFactor10 { get; set; }
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
        public int damageDealt { get; set; }
        [DataMember]
        public int damageReceived { get; set; }
        [DataMember]
        public int damaged { get; set; }
        [DataMember]
        public int deathReason { get; set; }
        [DataMember]
        public int damageAssistedRadio { get; set; }
        [DataMember]
        public int damageAssistedTrack { get; set; }
        [DataMember]
        public Dictionary<long, DamagedVehicle> details { get; set; }
        [DataMember]
        public List<List<JValue>> dossierPopUps { get; set; }
        [DataMember]
        public int droppedCapturePoints { get; set; }
        [DataMember]
        public int eventCredits { get; set; }
        [DataMember]
        public int eventFreeXP { get; set; }
        [DataMember]
        public int eventGold { get; set; }
        [DataMember]
        public int eventTMenXP { get; set; }
        [DataMember]
        public int eventXP { get; set; }
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
                //Compatibility with older versions
                //Some names changed in WoT 0.9.0
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
                //Compatibility with older versions
                //Some names changed in WoT 0.9.0
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
                //Compatibility with older versions
                //Some names changed in WoT 0.9.0
                return directHits;
            }
            set { _hits = value; }
        }

        [DataMember]
        public int directHits { get; set; }
        [DataMember]
        public bool isPremium { get; set; }
        [DataMember]
        public bool isTeamKiller { get; set; }
        [DataMember]
        public int killerID { get; set; }
        [DataMember]
        public int kills { get; set; }
        [DataMember]
        public int lifeTime { get; set; }
        [DataMember]
        public int markOfMastery { get; set; }
        [DataMember]
        public int mileage { get; set; }
        [DataMember]
        public int noDamageDirectHitsReceived { get; set; }
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
        public int originalCredits { get; set; }
        [DataMember]
        public int originalFreeXP { get; set; }
        [DataMember]
        public int originalXP { get; set; }
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
        public int premiumCreditsFactor10 { get; set; }
        [DataMember]
        public int premiumXPFactor10 { get; set; }
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
                //Compatibility with older versions
                //Some names changed in WoT 0.9.0
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
        public int tkills { get; set; }
        [DataMember]
        public int tmenXP { get; set; }
        [DataMember]
        public int typeCompDescr { get; set; }
        [DataMember]
        public int xp { get; set; }
        [DataMember]
        public int xpPenalty { get; set; }
    }
}