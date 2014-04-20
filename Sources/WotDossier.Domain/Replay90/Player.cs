using System;

namespace WotDossier.Domain.Replay90
{
    [Serializable]
    public class Player {
        public string ClanAbbrev;
        public int ClanDBID;
        public int IgrType;
        public string Name;
        public int PlatoonID;
        public int Team;
    }
}