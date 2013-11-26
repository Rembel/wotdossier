using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WotDossier.Domain.Replay
{
    [DataContract]
    public class Personal
    {
        [DataMember]
        private int _damageAssisted;
        [DataMember]
        public int accountDBID { get; set; }
        [DataMember]
        public List<int> achievements { get; set; }
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
        public List<List<int>> dossierPopUps { get; set; }
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
        public int heHitsReceived { get; set; }
        [DataMember]
        public int he_hits { get; set; }
        [DataMember]
        public int health { get; set; }
        [DataMember]
        public int hits { get; set; }
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
        public int noDamageShotsReceived { get; set; }
        [DataMember]
        public int originalCredits { get; set; }
        [DataMember]
        public int originalFreeXP { get; set; }
        [DataMember]
        public int originalXP { get; set; }
        [DataMember]
        public int piercedReceived { get; set; }
        [DataMember]
        public int potentialDamageReceived { get; set; }
        [DataMember]
        public int pierced { get; set; }
        [DataMember]
        public int premiumCreditsFactor10 { get; set; }
        [DataMember]
        public int premiumXPFactor10 { get; set; }
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