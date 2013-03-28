using WotDossier.Domain.Tank;

namespace WotDossier.Applications.ViewModel.Rows
{
    public class TankRowBase : ITankRowBase
    {
        private int _tier;
        private TankContour _icon;
        private string _tank;
        private int _tankType;
        private int _countryId;

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

        public int TankType
        {
            get { return _tankType; }
            set { _tankType = value; }
        }

        public int CountryId
        {
            get { return _countryId; }
            set { _countryId = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        protected TankRowBase()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public TankRowBase(TankJson tank)
        {
            Tier = tank.Common.tier;
            TankType = tank.Common.type;
            Tank = tank.Common.tanktitle;
            Icon = tank.TankContour;
            CountryId = tank.Common.countryid;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return Tank;
        }
    }
}