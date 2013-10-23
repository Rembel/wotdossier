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

        public TankRowMasterTanker(TankDescription tank)
        {
            Tier = tank.Tier;
            TankType = tank.Type;
            Tank = tank.Title;
            Icon = tank.Icon;
            CountryId = tank.CountryId;
            _isPremium = tank.Premium == 1;
        }
    }
}
