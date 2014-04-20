using System;

namespace WotDossier.Domain.Replay90
{
    [Serializable]
    public class VehicleResult {

        public int AccountDBID;
        public int AchievementCredits;
        public int AchievementFreeXP;
        public int AchievementXP;
        public object[] Achievements;
        public int CapturePoints;
        public int CountryID;
        public int Credits;
        public int DamageAssistedRadio;
        public int DamageAssistedTrack;
        public int DamageBlockedByArmor;
        public int DamageDealt;
        public int DamageReceived;
        public int Damaged;
        public int DeathReason;
        public int DirectHits;
        public int DirectHitsReceived;
        public int DirectTeamHits;
        public int DroppedCapturePoints;
        public int ExplosionHits;
        public int ExplosionHitsReceived;
        public int Gold;
        public int Health;
        public bool IsPrematureLeave;
        public bool IsTeamKiller;
        public int KillerID;
        public int Kills;
        public int LifeTime;
        public int Mileage;
        public int NoDamageDirectHitsReceived;
        public int Piercings;
        public int PiercingsReceived;
        public int PotentialDamageReceived;
        public int Shots;
        public int SniperDamageDealt;
        public int Spotted;
        public int TankID;
        public int TdamageDealt;
        public int Team;
        public int Tkills;
        public int TypeCompDescr;
        public int Xp;
    }
}