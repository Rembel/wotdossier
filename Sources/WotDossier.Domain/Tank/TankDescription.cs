using System.Runtime.Serialization;
using WotDossier.Common;

namespace WotDossier.Domain.Tank
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class TankDescription
    {
        public static TankDescription Unknown = new TankDescription { Title = "Unknown", Icon = TankIcon.Empty };

        /// <summary>
        /// Gets or sets the tank id.
        /// </summary>
        [DataMember(Name = "tankid")]
        public int TankId { get; set; }

        /// <summary>
        /// Gets or sets the country id.
        /// </summary>
        [DataMember(Name = "countryid")]
        public int CountryId { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        [DataMember(Name = "type")]
        public int Type { get; set; }

        /// <summary>
        /// Gets or sets the tier.
        /// </summary>
        [DataMember(Name = "tier")]
        public int Tier { get; set; }

        /// <summary>
        /// Gets or sets the premium.
        /// </summary>
        [DataMember(Name = "premium")]
        public int Premium { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        [DataMember(Name = "title")]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the comp descr.
        /// </summary>
        [DataMember(Name = "compDescr")]
        public int CompDescr { get; set; }

        public LevelRange LevelRange { get; set; }

        /// <summary>
        /// Gets or sets the icon.
        /// </summary>
        [IgnoreDataMember]
        public TankIcon Icon { get; set; }

        private RatingExpectancy _expectancy;
        /// <summary>
        /// Gets or sets the rating expectancy.
        /// </summary>
        [IgnoreDataMember]
        public RatingExpectancy Expectancy
        {
            get { return _expectancy ?? new RatingExpectancy(); }
            set { _expectancy = value; }
        }

        private int _uniqueId = -1;
        /// <summary>
        /// Uniques the id.
        /// </summary>
        public int UniqueId()
        {
            if (_uniqueId == -1)
            {
                _uniqueId = Utils.ToUniqueId(CountryId, TankId);
            }
            return _uniqueId;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return Title;
        }
    }
}
