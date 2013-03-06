namespace WotDossier.Domain.Rows
{
    public class TankRowBase
    {
        private int _tier;
        private TankContour _icon;
        private string _tank;

        public int Tier
        {
            get { return _tier; }
            set { _tier = value; }
        }

        public TankContour Icon
        {
            get { return _icon; }
            set { _icon = value; }
        }

        public string Tank
        {
            get { return _tank; }
            set { _tank = value; }
        }
    }
}