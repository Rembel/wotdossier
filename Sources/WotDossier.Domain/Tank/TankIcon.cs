namespace WotDossier.Domain.Tank
{
    public class TankIcon
    {
        public static TankIcon Empty = new TankIcon{iconid = string.Empty, x = 0, y = 0, height = 1, width = 1};

        public string iconid;
        public int country_id;
        public string country_code;
        public int x;
        public int y;
        public int height;
        public int width;
    }
}
