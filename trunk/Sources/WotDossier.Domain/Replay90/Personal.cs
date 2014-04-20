using System;
using System.Collections.Generic;

namespace WotDossier.Domain.Replay90
{
    [Serializable]
    public class Personal {

        public int AccountDBID;
        public int AchievementCredits;
        public int AchievementFreeXP;
        public int AchievementXP;
        public object[] Achievements;
        public int AogasFactor10;
        public int[] AutoEquipCost;
        public int[] AutoLoadCost;
        public int AutoRepairCost;
        public int CapturePoints;
        public int Countryid;
        public int Credits;
        public int CreditsContributionIn;
        public int CreditsContributionOut;
        public int CreditsPenalty;
        public int CreditsToDraw;
        public int DailyXPFactor10;
        public int DamageAssistedRadio;
        public int DamageAssistedTrack;
        public int DamageBlockedByArmor;
        public int DamageDealt;
        public int DamageReceived;
        public int Damaged;
        public int DeathReason;
        public Dictionary<long, DamagedVehicle> Details;
        public int DirectHits;
        public int DirectHitsReceived;
        public int DirectTeamHits;
        public object[][] DossierPopUps;
        public int DroppedCapturePoints;
        public int EventCredits;
        public int EventFreeXP;
        public int EventGold;
        public int EventTMenXP;
        public int EventXP;
        public int ExplosionHits;
        public int ExplosionHitsReceived;
        public int FreeXP;
        public int Gold;
        public int Health;
        public int[] HistAmmoCost;
        public int IgrXPFactor10;
        public bool IsPrematureLeave;
        public bool IsPremium;
        public bool IsTeamKiller;
        public int KillerID;
        public int Kills;
        public int LifeTime;
        public int MarkOfMastery;
        public int MarksOnGun;
        public int Mileage;
        public int MovingAvgDamage;
        public int NoDamageDirectHitsReceived;
        public int OriginalCredits;
        public int OriginalFreeXP;
        public int OriginalXP;
        public int Piercings;
        public int PiercingsReceived;
        public int PotentialDamageReceived;
        public int PremiumCreditsFactor10;
        public int PremiumXPFactor10;
        public int Repair;
        public int ServiceProviderID;
        public int Shots;
        public int SniperDamageDealt;
        public int Spotted;
        public int Tankid;
        public int TdamageDealt;
        public int Team;
        public int Tkills;
        public int TmenXP;
        public int TypeCompDescr;
        public int VehTypeLockTime;
        public bool Won;
        public int Xp;
        public int XpPenalty;
    }
}