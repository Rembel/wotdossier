using System.Collections.Generic;

namespace WotDossier.Domain.Server
{
    public class BattleJson
    {
        /// <summary>
        /// Battle time
        /// </summary>
        public int time { get; set; }

        /// <summary>
        /// battle type
        /// </summary>
        public ClanBattleType type { get; set; }

        public string province_id { get; set; }

        public string province_name { get; set; }

        /// <summary>
        /// Gets or sets the global map front identifier.
        /// </summary>
        public string front_id { get; set; }
    }
}
