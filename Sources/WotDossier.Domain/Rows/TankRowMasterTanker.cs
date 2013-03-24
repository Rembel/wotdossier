using WotDossier.Domain.Tank;

namespace WotDossier.Domain.Rows
{
    public class TankRowMasterTanker : TankRowBase
    {
        private bool _isPremium;

        public TankRowMasterTanker(TankJson tank)
            : base(tank)
        {
            _isPremium = tank.Common.premium == 1;
        }

        public TankRowMasterTanker(TankInfo tank, TankContour contour)
        {
            Tier = tank.tier;
            TankType = tank.type;
            Tank = tank.title;
            Icon = contour;
            CountryId = tank.countryid;
            _isPremium = tank.premium == 1;
        }

        public bool IsPremium
        {
            get { return _isPremium; }
            set { _isPremium = value; }
        }
    }
}
