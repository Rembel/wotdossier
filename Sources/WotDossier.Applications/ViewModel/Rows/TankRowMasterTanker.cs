using WotDossier.Domain.Tank;

namespace WotDossier.Applications.ViewModel.Rows
{
    public class TankRowMasterTanker : TankRowBase, ITankRowMasterTanker
    {
        private bool _isPremium;

        public bool IsPremium
        {
            get { return _isPremium; }
            set { _isPremium = value; }
        }

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
    }
}
