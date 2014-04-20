using System;

namespace WotDossier.Domain.Replay90
{
    [Serializable]
    public class ReplayIdentify {

        public int AccountDBID;
        public int ArenaCreateTime;
        public long ArenaUniqueID;
        public int Countryid;
        public string Error;
        public string Error_details;
        public string InternaluserID;
        public string MapName;
        public int Mapid;
        public string Playername;
        public string Replay_version;
        public int Tankid;
    }
}