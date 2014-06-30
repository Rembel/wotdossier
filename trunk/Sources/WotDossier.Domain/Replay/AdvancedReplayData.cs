using System.Collections.Generic;

namespace WotDossier.Domain.Replay
{
    public class AdvancedReplayData
    {
        public long arenaCreateTime { get; set; }
        public int arenaTypeID { get; set; }
        public long arenaUniqueID { get; set; }
        public int bonusType { get; set; }
        public int gameplayID { get; set; }
        public int guiType { get; set; }
        public BattleInfo more { get; set; }
        public string playername { get; set; }
        public string replay_version { get; set; }
        public StreamData streamData { get; set; }
        public Dictionary<string, AdvancedPlayerInfo> roster { get; set; }
    }

    public class AdvancedPlayerInfo
    {
        public int accountDBID { get; set; }
        public string clanAbbrev { get; set; }
        public int clanID { get; set; }
        public int compDescr { get; set; }
        public int countryID { get; set; }
        public int internaluserID { get; set; }
        public string playerName { get; set; }
        public int prebattleID { get; set; }
        public int tankID { get; set; }
        public int team { get; set; }
        public AdvancedVehicleInfo vehicle { get; set; }
    }

    public class BattleInfo
    {
        public int battleLevel { get; set; }
    }

    public class AdvancedVehicleInfo
    {
        public ModuleIndexer module;

        public AdvancedVehicleInfo()
        {
            module = new ModuleIndexer(this);
        }

        public int chassisID { get; set; }
        public int engineID { get; set; }
        public int fueltankID { get; set; }
        public int gunID { get; set; }
        public int radioID { get; set; }
        public int turretID { get; set; }

        public int module_0 { get; set; }
        public int module_1 { get; set; }
        public int module_2 { get; set; }
    }

    public class ModuleIndexer
    {
        private readonly AdvancedVehicleInfo _advancedVehicleInfo;

        public ModuleIndexer(AdvancedVehicleInfo advancedVehicleInfo)
        {
            _advancedVehicleInfo = advancedVehicleInfo;
        }

        // Indexer to get and set characters in the containing document:
        public int this[int index]
        {
            get
            {
                if (index == 0)
                {
                    return _advancedVehicleInfo.module_0;
                }

                if (index == 1)
                {
                    return _advancedVehicleInfo.module_1;
                }

                if (index == 2)
                {
                    return _advancedVehicleInfo.module_2;
                }

                return -1;
            }
            set
            {
                if (index == 0)
                {
                    _advancedVehicleInfo.module_0 = value;
                }

                if (index == 1)
                {
                    _advancedVehicleInfo.module_1 = value;
                }

                if (index == 2)
                {
                    _advancedVehicleInfo.module_2 = value;
                }
            }
        }
    }
}