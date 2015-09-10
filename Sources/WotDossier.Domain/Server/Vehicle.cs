using WotDossier.Domain.Tank;

namespace WotDossier.Domain.Server
{
    public class Vehicle
    {
        public int battles;
        public int max_frags;
        public int max_xp;
        public bool? in_garage;
        public int wins;
        public int tank_id;
        public int mark_of_mastery;
        public StatisticPart clan;
        public StatisticPart all;
        public StatisticPart company;
        public MedalAchievements achievements;

        public TankDescription description { get; set; }
    }
}
