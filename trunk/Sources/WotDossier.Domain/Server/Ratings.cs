namespace WotDossier.Domain.Server
{
    public class Ratings
    {
        public Rating battles_count { get; set; }

        public Rating damage_dealt { get; set; }
        public Rating damage_avg { get; set; }

        public Rating frags_count { get; set; }
        public Rating frags_avg { get; set; }

        public Rating hits_ratio { get; set; }

        public Rating spotted_count { get; set; }
        public Rating spotted_avg { get; set; }

        public Rating survived_ratio { get; set; }
        public Rating wins_ratio { get; set; }
       
        public Rating xp_amount { get; set; }
        public Rating xp_avg { get; set; }
        public Rating xp_max { get; set; }

        public Rating capture_points { get; set; }

        public Rating global_rating { get; set; }
    }
}
