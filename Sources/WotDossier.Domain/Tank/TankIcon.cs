using System.Runtime.Serialization;

namespace WotDossier.Domain.Tank
{
    [DataContract]
    public class TankIcon
    {
        public static TankIcon Empty = new TankIcon{Icon = string.Empty};

        public string IconId
        {
            get
            {
                return string.Format("{0}_{1}", CountryCode, Icon);
            }
        }

        [DataMember(Name = "countryid")]
        public int CountryId { get; set; }

        [DataMember(Name = "country_code")]
        public string CountryCode { get; set; }

        [DataMember(Name = "icon")]
        public string Icon { get; set; }

        [DataMember(Name = "icon_orig")]
        public string IconOrig { get; set; }
    }
}
