using System.Collections.Generic;

namespace WotDossier.Domain.Player
{
    public class PlayerStat
    {
        public string status { get; set; }
        public string status_code { get; set; }

        public PlayerStatData dataField { get; set; }
        public Dictionary<int, PlayerStatData> data { get; set; }
    }
}
