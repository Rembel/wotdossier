using WotDossier.Applications.ViewModel.Rows;
using WotDossier.Domain.Tank;

namespace WotDossier.Applications.ViewModel
{
    internal class ExportTankFragModel : IExportTankFragModel
    {
        private readonly TankStatisticRowViewModel _tank;
        private readonly FragsJson _frag;

        public ExportTankFragModel(TankStatisticRowViewModel tank, FragsJson frag)
        {
            _tank = tank;
            _frag = frag;
        }

        public string Tank
        {
            get { return _tank.Tank; }
            set { _tank.Tank = value; }
        }

        public double Tier
        {
            get { return _frag.Tier; }
            set { _frag.Tier = value; }
        }

        public int CountryId
        {
            get { return _tank.CountryId; }
            set { _tank.CountryId = value; }
        }

        public int Type
        {
            get { return _tank.Type; }
            set { _tank.Type = value; }
        }

        public int TankId
        {
            get { return _tank.TankId; }
            set { _tank.TankId = value; }
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