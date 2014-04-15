using System.Collections.Generic;

namespace WotDossier.Domain.Server
{
    public class PlayerData
    {
        public int account_id { get; set; }
        public string nickname { get; set; }
        public double created_at { get; set; }
        public double updated_at { get; set; }
        public double logout_at { get; set; }
        public double last_battle_time { get; set; }
        public double global_rating { get; set; }
        public Achievements achievements { get; set; }
        public Ratings ratings { get; set; }
        public List<Vehicle> vehicles { get; set; }
        public Clan clan { get; set; }
        public ClanData clanData { get; set; }
        public Statistics statistics { get; set; }
    }
}