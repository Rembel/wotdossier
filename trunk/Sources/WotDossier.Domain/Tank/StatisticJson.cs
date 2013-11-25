namespace WotDossier.Domain.Tank
{
    public class StatisticJson
    {
        public int battlesCount;
        public int capturePoints;
        public int damageDealt;
        public int damageReceived;
        public int droppedCapturePoints;
        public int frags;
        public int frags8p;
        public int hits;
        public int losses;
        public int shots;
        public int spotted;
        public int survivedBattles;
        public int winAndSurvived;
        public int wins;
        public int xp;

        #region [0.8.8]
        public int originalXP;
        public int damageAssistedTrack;
        public int damageAssistedRadio;
        public int shotsReceived;
        public int noDamageShotsReceived;
        public int piercedReceived;
        public int heHitsReceived;
        public int he_hits;
        public int pierced;
        public int xpBefore8_8;
        public int battlesCountBefore8_8;
        #endregion

        public int maxDamage;
        public int maxFrags;
        public int maxXP;
    }
}