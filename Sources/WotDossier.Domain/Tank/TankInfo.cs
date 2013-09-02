using WotDossier.Common;

namespace WotDossier.Domain.Tank
{
    public class TankInfo
    {
        public int tankid;
        public int countryid;
        public string countryCode;
        public int type;
        public int tier;
        public int premium;
        public string title;
        public string icon;
        public string icon_orig;
        public double nominal_damage;

        public int UniqueId()
        {
            return Utils.ToUniqueId(countryid, tankid);
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return title;
        }
    }
}
