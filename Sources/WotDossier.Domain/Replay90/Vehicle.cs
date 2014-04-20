using System;

namespace WotDossier.Domain.Replay90
{
    [Serializable]
    public class Vehicle {

        public string ClanAbbrev;
        public int IgrType;
        public bool IsAlive;
        public bool IsTeamKiller;
        public string Name;
        public int Team;
        public string VehicleType;
    }
}