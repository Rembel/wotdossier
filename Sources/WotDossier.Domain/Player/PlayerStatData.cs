using System.Collections.Generic;

namespace WotDossier.Domain.Player
{
    public class PlayerStatData
    {
        public int account_id { get; set; }
        public string nickname { get; set; }
        public double created_at { get; set; }
        public double updated_at { get; set; }
        public Achievements achievements { get; set; }
        public Ratings ratings { get; set; }
        public List<VehicleStat> vehicles { get; set; }
        public Clan clan { get; set; }
        public ClanData clanData { get; set; }
        public Statistics statistics { get; set; }
    }
}