namespace WotDossier.Domain.Rows
{
    public class TankRowMasterTanker : TankRowBase
    {
        private bool _isPremium;

        public TankRowMasterTanker(Tank tank)
        {
            Tier = tank.Common.tier;
            Tank = tank.Name;
            Icon = tank.TankContour;
            _isPremium = tank.Common.premium == 1;
        }

        public TankRowMasterTanker(TankInfo tank, TankContour contour)
        {
            Tier = tank.tier;
            Tank = tank.title;
            Icon = contour;
            _isPremium = tank.premium == 1;
        }

        public bool IsPremium
        {
            get { return _isPremium; }
            set { _isPremium = value; }
        }
    }
}
