﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WotDossier.Domain.Replay
{
    /*
http://wiki.vbaddict.net
    */
    [DataContract]
    public class Replay
    {
        [DataMember]
        public FirstBlock datablock_1 { get; set; }
        [DataMember]
        public CommandResult CommandResult { get; set; }
        [DataMember]
        public BattleResult datablock_battle_result { get; set; }
    }

    [DataContract]
    public class FirstBlock
    {
        [DataMember]
        public string dateTime { get; set; }
        [DataMember]
        public string gameplayID { get; set; }
        [DataMember]
        public string mapDisplayName { get; set; }
        [DataMember]
        public string mapName { get; set; }
        [DataMember]
        public long playerID { get; set; }
        [DataMember]
        public string playerName { get; set; }
        [DataMember]
        public string playerVehicle { get; set; }
        [DataMember]
        public Dictionary<long, Vehicle> vehicles { get; set; }
    }

    [DataContract]
    public class CommandResult
    {
        [DataMember]
        public Damaged Damage { get; set; }
        [DataMember]
        public Dictionary<long, Vehicle> Vehicles { get; set; }
        [DataMember]
        public Dictionary<long, FragsCount> Frags { get; set; }
    }

    [DataContract]
    public class Damaged
    {
        [DataMember]
        public List<int> achieveIndices { get; set; }
        [DataMember]
        public int arenaCreateTime { get; set; }
        [DataMember]
        public int arenaTypeID { get; set; }
        [DataMember]
        public int capturePoints { get; set; }
        [DataMember]
        public int credits { get; set; }
        [DataMember]
        public int damageDealt { get; set; }
        [DataMember]
        public int damageReceived { get; set; }
        [DataMember]
        public List<int> damaged { get; set; }

        [DataMember]
        public int droppedCapturePoints { get; set; }
        [DataMember]
        public Factors factors { get; set; }
        [DataMember]
        public List<int> heroVehicleIDs { get; set; }
        [DataMember]
        public int hits { get; set; }
        [DataMember]
        public int isWinner { get; set; }
        [DataMember]
        public List<int> killed { get; set; }

        [DataMember]
        public int killerID { get; set; }
        [DataMember]
        public int repair { get; set; }
        [DataMember]
        public int shots { get; set; }
        [DataMember]
        public int shotsReceived { get; set; }
        [DataMember]
        public List<int> spotted { get; set; }
        [DataMember]
        public int xp { get; set; }
    }

    [DataContract]
    public class Factors
    {
        [DataMember]
        public int aogasFactor10 { get; set; }
        [DataMember]
        public int dailyXPFactor10 { get; set; }
    }

    [DataContract]
    public class FragsCount
    {
        [DataMember]
        public int frags { get; set; }
    }

    [DataContract]
    public class BattleResult
    {
        [DataMember]
        public long arenaUniqueID { get; set; }
        [DataMember]
        public Common common { get; set; }
        [DataMember]
        public Personal personal { get; set; }
        [DataMember]
        public Dictionary<long, Player> players { get; set; }
        [DataMember]
        public Dictionary<long, VehicleResult> vehicles { get; set; }
    }

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

    [DataContract]
    public class Common
    {
        [DataMember]
        public long arenaCreateTime { get; set; }
        [DataMember]
        public int arenaTypeID { get; set; }
        [DataMember]
        public int bonusType { get; set; }
        [DataMember]
        public double duration { get; set; }
        [DataMember]
        public int finishReason { get; set; }
        [DataMember]
        public int vehLockMode { get; set; }
        [DataMember]
        public int winnerTeam { get; set; }
    }

    [DataContract]
    public class Vehicle
    {
        [DataMember]
        public string clanAbbrev { get; set; }
        //        "events": {}, 
        [DataMember]
        public bool isAlive { get; set; }
        [DataMember]
        public bool isTeamKiller { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public int team { get; set; }
        [DataMember]
        public string vehicleType { get; set; }
    }

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

    [DataContract]
    public class Player
    {
        [DataMember]
        public string clanAbbrev { get; set; }
        [DataMember]
        public int clanDBID { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public int prebattleID { get; set; }
        [DataMember]
        public int team { get; set; }
    }

    [DataContract]
    public class DamagedVehicle
    {
        [DataMember]
        private int _damageAssisted;

        private int _deathReason = -1;
        private int _killed;
        private List<Device> _tankDamageCrits;
        private List<CrewMember> _crewCrits;
        private List<Device> _tankCrits;

        [DataMember]
        public int crits { get; set; }

        public List<Device> tankDamageCrits
        {
            get
            {
                if (_tankDamageCrits == null)
                {
                    _tankDamageCrits = new List<Device>();
                    int critsFlags = crits >> 12 & 4095;
                    Array array = Enum.GetValues(typeof(Device));
                    foreach (Device device in array)
                    {
                        if ((critsFlags & (short) device) == (short)device)
                        {
                            _tankDamageCrits.Add(device);
                        }
                    }
                }
                return _tankDamageCrits;
            }
        }



        public List<Device> tankCrits
        {
            get
            {
                if (_tankCrits == null)
                {
                    _tankCrits = new List<Device>();
                    int critsFlags = crits & 4095;
                    Array array = Enum.GetValues(typeof(Device));
                    foreach (Device device in array)
                    {
                        if ((critsFlags & (short) device) == (short)device)
                        {
                            _tankCrits.Add(device);
                        }
                    }
                }
                return _tankCrits;
            }
        }

        public List<CrewMember> crewCrits
        {
            get
            {
                if (_crewCrits == null)
                {
                    _crewCrits = new List<CrewMember>();
                    int critsFlags = crits >> 24 & 255;
                    Array array = Enum.GetValues(typeof(CrewMember));
                    foreach (CrewMember member in array)
                    {
                        if ((critsFlags & (short)member) == (short)member)
                        {
                            _crewCrits.Add(member);
                        }
                    }
                }
                return _crewCrits;
            }
        }

        public int critsCount
        {
            get 
            {
                if (crits > 1000)
                {
                    return tankDamageCrits.Count + crewCrits.Count + tankCrits.Count;
                }
                return crits;
            }
        }

        [DataMember]
        public int deathReason
        {
            get { return _deathReason; }
            set { _deathReason = value; }
        }

        [DataMember]
        public int damageAssistedRadio { get; set; }
        [DataMember]
        public int damageAssistedTrack { get; set; }
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
        public int fire { get; set; }
        [DataMember]
        public int he_hits { get; set; }
        [DataMember]
        public int hits { get; set; }
        [DataMember]
        [Obsolete("0.8.6")]
        public int killed
        {
            get
            {
                if (deathReason >= 0)
                {
                    return 1;
                }
                return _killed;
            }
            set { _killed = value; }
        }

        [DataMember]
        public int pierced { get; set; }
        [DataMember]
        public int spotted { get; set; }
    }

    [Flags]
    public enum Device : short
    {
        engine = 1,
        ammoBay = 2,
        fuelTank = 4,
        radio = 8,
        track = 16,
        gun = 32,
        turretRotator = 64,
        surveyingDevice = 128
    }

    [Flags]
    public enum CrewMember : short
    {
        commander = 1,
        driver = 2,
        radioman = 4,
        gunner = 8,
        loader = 16
    }
}