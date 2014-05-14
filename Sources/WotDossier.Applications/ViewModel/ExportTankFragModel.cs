using WotDossier.Domain.Tank;

namespace WotDossier.Applications.ViewModel
{
    internal class ExportTankFragModel : IExportTankFragModel
    {
        private readonly TankJson _tank;
        private readonly FragsJson _frag;

        public ExportTankFragModel(TankJson tank, FragsJson frag)
        {
            _tank = tank;
            _frag = frag;
        }

        public string Tank
        {
            get { return _tank.Common.tanktitle; }
            set { _tank.Common.tanktitle = value; }
        }

        public double Tier
        {
            get { return _frag.Tier; }
            set { _frag.Tier = value; }
        }

        public int CountryId
        {
            get { return _tank.Common.countryid; }
            set { _tank.Common.countryid = value; }
        }

        public int Type
        {
            get { return _tank.Common.type; }
            set { _tank.Common.type = value; }
        }

        public int TankId
        {
            get { return _tank.Common.tankid; }
            set { _tank.Common.tankid = value; }
        }

        public double FragTier
        {
            get { return _frag.Tier; }
            set { _frag.Tier = value; }
        }

        public int FragCountryId
        {
            get { return _frag.CountryId; }
            set { _frag.CountryId = value; }
        }

        public int FragType
        {
            get { return _frag.Type; }
            set { _frag.Type = value; }
        }

        public int FragTankId
        {
            get { return _frag.TankId; }
            set { _frag.TankId = value; }
        }

        public string FragTank
        {
            get { return _frag.Tank; }
            set { _frag.Tank = value; }
        }

        public int Count
        {
            get { return _frag.Count; }
            set { _frag.Count = value; }
        }
    }
}