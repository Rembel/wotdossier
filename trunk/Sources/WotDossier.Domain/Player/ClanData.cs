using System.Collections.Generic;

namespace WotDossier.Domain.Player
{
    public class ClanData
    {
        public int id { get; set; }
        public string name { get; set; }
        public string abbreviation { get; set; }
        public string description { get; set; }
        public string motto { get; set; }
        public int members_count { get; set; } 
        
        public string color { get; set; } 
        public double updated_at { get; set; }
        public long created_at { get; set; }

        public int leader_id { get; set; }

        public ClanEmblems emblems_urls { get; set; }
        public List<ClanMember> members { get; set; } 
    }
}