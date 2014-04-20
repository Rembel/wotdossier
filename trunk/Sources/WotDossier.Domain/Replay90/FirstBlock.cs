using System;
using System.Collections.Generic;

namespace WotDossier.Domain.Replay90
{
    [Serializable]
    public class FirstBlock 
    {

        public int BattleType;
        public string ClientVersionFromExe;
        public string ClientVersionFromXml;
        public string DateTime;
        public string GameplayID;
        public string MapDisplayName;
        public string MapName;
        public int PlayerID;
        public string PlayerName;
        public string PlayerVehicle;
        public string RegionCode;
        public object[] RoamingSettings;
        public string ServerName;
        public Dictionary<long, Vehicle> Vehicles;
    }
}