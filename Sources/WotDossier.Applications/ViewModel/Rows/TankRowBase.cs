using WotDossier.Domain.Tank;

namespace WotDossier.Applications.ViewModel.Rows
{
    public class TankRowBase : ITankRowBase
    {
        public int Tier { get; set; }

        public TankIcon Icon { get; set; }

        public string Tank { get; set; }

        public int TankType { get; set; }

        public int CountryId { get; set; }

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
            Icon = tank.Description.Icon;
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