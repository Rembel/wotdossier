using System.Collections.Generic;

namespace WotDossier.Domain.Player
{
    public class PlayerStatData
    {
        public string name;
        public double created_at;
        public double updated_at;
        public Achievements achievements { get; set; }
        public Battles battles { get; set; }
        public Summary summary { get; set; }
        public Experience experience { get; set; }
        public Ratings ratings { get; set; }
        public IList<Vehicle> vehicles;
    }
}