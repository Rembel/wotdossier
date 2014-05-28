using WotDossier.Domain.Interfaces;
using WotDossier.Domain.Tank;

namespace WotDossier.Applications.ViewModel.Rows
{
    public class TankRowMasterTanker : ITankRowBase
    {
        /// <summary>
        /// Gets or sets the tier.
        /// </summary>
        /// <value>
        /// The tier.
        /// </value>
        public double Tier { get; set; }

        /// <summary>
        /// Gets or sets the icon.
        /// </summary>
        /// <value>
        /// The icon.
        /// </value>
        public TankIcon Icon { get; set; }

        /// <summary>
        /// Gets or sets the tank.
        /// </summary>
        /// <value>
        /// The tank.
        /// </value>
        public string Tank { get; set; }

        /// <summary>
        /// Gets or sets the type of the tank.
        /// </summary>
        /// <value>
        /// The type of the tank.
        /// </value>
        public int TankType { get; set; }

        /// <summary>
        /// Gets or sets the country id.
        /// </summary>
        /// <value>
        /// The country id.
        /// </value>
        public int CountryId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is premium.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is premium; otherwise, <c>false</c>.
        /// </value>
        public bool IsPremium { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TankRowMasterTanker"/> class.
        /// </summary>
        /// <param name="tank">The tank.</param>
        public TankRowMasterTanker(TankDescription tank)
        {
            Tier = tank.Tier;
            TankType = tank.Type;
            Tank = tank.Title;
            Icon = tank.Icon;
            CountryId = tank.CountryId;
            IsPremium = tank.Premium == 1;
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
