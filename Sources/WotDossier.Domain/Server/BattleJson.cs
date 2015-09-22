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

    public class StrongholdBattleJson
    {
        public string attacker_clan_tag { get; set; }
        public string attacker_clan_name { get; set; }
        public int battle_planned_date { get; set; }
        public string defender_clan_name { get; set; }
        public int attacker_clan_id { get; set; }
        public string defender_clan_tag { get; set; }
        public StrongholBattleType battle_type { get; set; }
        public int defender_clan_id { get; set; }
        public int battle_creation_date { get; set; }
        public string attack_direction { get; set; }
        public string defense_direction { get; set; }
    }
}
