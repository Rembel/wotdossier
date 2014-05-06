using System;
using System.Globalization;
using WotDossier.Domain;

namespace WotDossier.Dal
{
    public class CountryHelper
    {
        /// <summary>
        /// Gets the country code.
        /// </summary>
        /// <param name="countryid">The countryid.</param>
        /// <returns></returns>
        public static string GetCountryNameCode(int countryid)
        {
            if (Enum.IsDefined(typeof(Country), countryid))
            {
                return ((Country)countryid).ToString().ToLower();
            }
            return string.Empty;
        }

        /// <summary>
        /// Gets the country identifier by code.
        /// </summary>
        /// <param name="countryCode">The country code.</param>
        /// <returns></returns>
        public static int GetCountryIdByCode(string countryCode)
        {
            string capitalizedFirstLetter = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(countryCode.ToLower());
            if (Enum.IsDefined(typeof (Country), capitalizedFirstLetter))
            {
                return (int)Enum.Parse(typeof(Country), capitalizedFirstLetter);
            }
            return -1;
        }
    }
}