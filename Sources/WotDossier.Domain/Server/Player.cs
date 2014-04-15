using System.Collections.Generic;

namespace WotDossier.Domain.Server
{
    public class Player
    {
        public string status { get; set; }
        public string status_code { get; set; }

        public PlayerData dataField { get; set; }
        public Dictionary<int, PlayerData> data { get; set; }
    }
}
