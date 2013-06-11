using System;
using System.Collections.Generic;

namespace WotDossier.Domain.Replay
{
    /*
    'warrior',  # 1
'invader',  # 2
'sniper',  # 3
'defender',  # 4
'steelwall', # 5
'supporter', # 6
'scout',  # 7

    'medalWittmann',  # 0
'medalOrlik',   # 1
'medalOskin',   # 2
'medalHalonen',  # 3
'medalBurda',   # 4
'medalBillotte',  # 5
'medalKolobanov',  # 6
'medalFadin',   # 7
    */
    public class Replay
    {
        public FirstBlock datablock_1 { get; set; }
        public CommandResult CommandResult { get; set; }
        public BattleResult datablock_battle_result { get; set; }
    }

    public class FirstBlock
    {
        public string dateTime { get; set; }
        public string gameplayID { get; set; }
        public string mapDisplayName { get; set; }
        public string mapName { get; set; }
        public long playerID { get; set; }
        public string playerName { get; set; }
        public string playerVehicle { get; set; }

        public Dictionary<long, Vehicle> vehicles { get; set; }
    }

    public class CommandResult
    {
        public Damaged Damage { get; set; }
        public Dictionary<int, Vehicle> Vehicles { get; set; }
        public Dictionary<int, FragsCount> Frags { get; set; }
    }

    public class Damaged
    {
        public List<int> achieveIndices { get; set; }
        public int arenaCreateTime { get; set; }
        public int arenaTypeID { get; set; }
        public int capturePoints { get; set; }
        public int credits { get; set; }
        public int damageDealt { get; set; }
        public int damageReceived { get; set; }
        public List<int> damaged { get; set; }
        public int droppedCapturePoints { get; set; }
        public Factors factors { get; set; }
        public List<int> heroVehicleIDs { get; set; }
        public int hits { get; set; }
        public int isWinner { get; set; }
        public List<int> killed { get; set; }
        public int killerID { get; set; }
        public int repair { get; set; }
        public int shots { get; set; }
        public int shotsReceived { get; set; }
        public List<int> spotted { get; set; }
        public int xp { get; set; }
    }

    public class Factors
    {
        public int aogasFactor10 { get; set; }
        public int dailyXPFactor10 { get; set; }
    }

    public class FragsCount
    {
        public int frags { get; set; }
    }

    public class BattleResult
    {
        public long arenaUniqueID { get; set; }
        public Common common { get; set; }
        public Personal personal { get; set; }
        public Dictionary<long, Player> players { get; set; }
        public Dictionary<long, VehicleResult> vehicles { get; set; }
    }

    public class Personal
    {
        public int accountDBID { get; set; }
        public List<int> achievements { get; set; }
        public int aogasFactor10 { get; set; }
        public List<int> autoEquipCost { get; set; }
        public List<int> autoLoadCost { get; set; }
        public int autoRepairCost { get; set; }
        public int capturePoints { get; set; }
        public int credits { get; set; }
        public int creditsContributionIn { get; set; }
        public int creditsContributionOut { get; set; }
        public int creditsPenalty { get; set; }
        public int dailyXPFactor10 { get; set; }
        public int damageAssisted { get; set; }
        public int damageDealt { get; set; }
        public int damageReceived { get; set; }
        public int damaged { get; set; }

        public Dictionary<long, DamagedVehicle> details { get; set; }

        //"dossierPopUps": [
        //    [
        //        36, 
        //        7
        //    ]
        //], 
        public int droppedCapturePoints { get; set; }
        public int eventCredits { get; set; }
        public int eventFreeXP { get; set; }
        public int eventGold { get; set; }
        public int eventTMenXP { get; set; }
        public int eventXP { get; set; }
        public int freeXP { get; set; }
        public int gold { get; set; }
        public int he_hits { get; set; }
        public int health { get; set; }
        public int hits { get; set; }
        public bool isPremium { get; set; }
        public bool isTeamKiller { get; set; }
        public int killerID { get; set; }
        public int kills { get; set; }
        public int lifeTime { get; set; }
        public int markOfMastery { get; set; }
        public int mileage { get; set; }
        public int pierced { get; set; }
        public int premiumCreditsFactor10 { get; set; }
        public int premiumXPFactor10 { get; set; }
        public int repair { get; set; }
        public int shots { get; set; }
        public int shotsReceived { get; set; }
        public int spotted { get; set; }
        public int tdamageDealt { get; set; }
        public int team { get; set; }
        public int tkills { get; set; }
        public int tmenXP { get; set; }
        public int typeCompDescr { get; set; }
        public int xp { get; set; }
        public int xpPenalty { get; set; }
    }

    public class Common
    {
        public long arenaCreateTime { get; set; }
        public int arenaTypeID { get; set; }
        public int bonusType { get; set; }
        public double duration { get; set; }
        public int finishReason { get; set; }
        public int vehLockMode { get; set; }
        public int winnerTeam { get; set; }
    }

    public class Vehicle
    {
        public string clanAbbrev { get; set; }
        //        "events": {}, 
        public bool isAlive { get; set; }
        public bool isTeamKiller { get; set; }
        public string name { get; set; }
        public int team { get; set; }
        public string vehicleType { get; set; }
    }

    public class VehicleResult
    {
        public long accountDBID { get; set; }
        public List<int> achievements { get; set; }
        public int capturePoints { get; set; }
        public int credits { get; set; }
        public int damageAssisted { get; set; }
        public int damageDealt { get; set; }
        public int damageReceived { get; set; }
        public int damaged { get; set; }
        public int droppedCapturePoints { get; set; }
        public int freeXP { get; set; }
        public int gold { get; set; }
        public int he_hits { get; set; }
        public int health { get; set; }
        public int hits { get; set; }
        public bool isTeamKiller { get; set; }
        public int killerID { get; set; }
        public int kills { get; set; }
        public int lifeTime { get; set; }
        public int mileage { get; set; }
        public int pierced { get; set; }
        public int potentialDamageReceived { get; set; }
        public int repair { get; set; }
        public int shots { get; set; }
        public int shotsReceived { get; set; }
        public int spotted { get; set; }
        public int tdamageDealt { get; set; }
        public int team { get; set; }
        public int thits { get; set; }
        public int tkills { get; set; }
        public int typeCompDescr { get; set; }
        public int xp { get; set; }
    }

    public class Player
    {
        public string clanAbbrev { get; set; }
        public int clanDBID { get; set; }
        public string name { get; set; }
        public int prebattleID { get; set; }
        public int team { get; set; }
    }

    public class DamagedVehicle
    {
        public int crits { get; set; }
        public int damageAssisted { get; set; }
        public int damageDealt { get; set; }
        public int fire { get; set; }
        public int he_hits { get; set; }
        public int hits { get; set; }
        public int killed { get; set; }
        public int pierced { get; set; }
        public int spotted { get; set; }
    }
}
