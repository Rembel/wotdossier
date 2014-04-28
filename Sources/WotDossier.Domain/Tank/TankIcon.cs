using System.Runtime.Serialization;

namespace WotDossier.Domain.Tank
{
    [DataContract]
    public class TankIcon
    {
        public static TankIcon Empty = new TankIcon{Icon = string.Empty};

        /// <summary>
        /// Gets the icon id.
        /// </summary>
        public string IconId
        {
            get
            {
                return string.Format("{0}_{1}", CountryCode, Icon);
            }
        }

        /// <summary>
        /// Gets or sets the country id.
        /// </summary>
        [DataMember(Name = "countryid")]
        public int CountryId { get; set; }

        /// <summary>
        /// Gets or sets the country code.
        /// </summary>
        [DataMember(Name = "country_code")]
        public string CountryCode { get; set; }

        /// <summary>
        /// Gets or sets the icon.
        /// </summary>
        [DataMember(Name = "icon")]
        public string Icon { get; set; }

        /// <summary>
        /// Gets or sets the icon orig.
        /// </summary>
        [DataMember(Name = "icon_orig")]
        public string IconOrig { get; set; }
    }
}
