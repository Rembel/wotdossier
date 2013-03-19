using System.Collections.Generic;

namespace WotDossier.Domain.Player
{
    public class PlayerStatData
    {
        public string name { get; set; }
        public double created_at { get; set; }
        public double updated_at { get; set; }
        public Achievements achievements { get; set; }
        public Battles battles { get; set; }
        public Summary summary { get; set; }
        public Experience experience { get; set; }
        public Ratings ratings { get; set; }
        public IList<Vehicle> vehicles { get; set; }
        public ClanInfo clan { get; set; }
        public int id { get; set; }
    }
}