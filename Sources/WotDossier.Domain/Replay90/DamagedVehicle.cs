using System;

namespace WotDossier.Domain.Replay90
{
    [Serializable]
    public class DamagedVehicle {

        public int Crits;
        public int CritsCount;
        public object[] CritsCriticalDevicesList;
        public object[] CritsDestroyedDevicesList;
        public string[] CritsDestroyedTankmenList;
        public int DamageAssistedRadio;
        public int DamageAssistedTrack;
        public int DamageDealt;
        public int DeathReason;
        public int DirectHits;
        public int ExplosionHits;
        public int Fire;
        public int Piercings;
        public int Spotted;

    }
}