using System.Collections.Generic;

namespace WotDossier.Domain.Server
{
    public class ClanData
    {
        public int clan_id { get; set; }
        public string name { get; set; }
        public string abbreviation { get; set; }
        public string description { get; set; }
        public string motto { get; set; }
        public int members_count { get; set; } 
        
        public string clan_color { get; set; } 
        public double updated_at { get; set; }
        public long created_at { get; set; }

        public int owner_id { get; set; }
        public bool is_clan_disbanded { get; set; }
        public bool request_availability { get; set; }

        public ClanEmblems emblems { get; set; }
        public Dictionary<int, ClanMember> members { get; set; } 
    }
}