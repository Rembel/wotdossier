using System.Runtime.Serialization;
using WotDossier.Common;

namespace WotDossier.Domain.Tank
{
    [DataContract]
    public class TankDescription
    {
        private RatingExpectancy _expectancy;

        [DataMember(Name = "tankid")]
        public int TankId { get; set; }

        [DataMember(Name = "countryid")]
        public int CountryId { get; set; }

        [DataMember(Name = "countryCode")]
        public string CountryCode { get; set; }

        [DataMember(Name = "type")]
        public int Type { get; set; }

        [DataMember(Name = "tier")]
        public int Tier { get; set; }

        [DataMember(Name = "premium")]
        public int Premium { get; set; }

        [DataMember(Name = "title")]
        public string Title { get; set; }

        [IgnoreDataMember]
        public TankIcon Icon { get; set; }

        [IgnoreDataMember]
        public RatingExpectancy Expectancy
        {
            get { return _expectancy ?? new RatingExpectancy(); }
            set { _expectancy = value; }
        }

        public int UniqueId()
        {
            return Utils.ToUniqueId(CountryId, TankId);
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
