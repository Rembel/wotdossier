using System;
using System.Collections.Generic;

namespace WotDossier.Domain.Replay90
{
    [Serializable]
    public class BattleResult {

        public long ArenaUniqueID;
        public Common Common;
        public Personal Personal;
        public Dictionary<long, Player> Players;
        public Dictionary<long, VehicleResult> Vehicles;
    }
}