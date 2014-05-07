using System;
using System.Globalization;
using WotDossier.Domain;

namespace WotDossier.Dal
{
    public class CountryHelper
    {
        /// <summary>
        /// Gets the country identifier by code.
        /// </summary>
        /// <param name="countryCode">The country code.</param>
        /// <returns></returns>
        public static Country GetCountryIdByCode(string countryCode)
        {
            string capitalizedFirstLetter = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(countryCode.ToLower());
            if (Enum.IsDefined(typeof (Country), capitalizedFirstLetter))
            {
                return (Country) Enum.Parse(typeof(Country), capitalizedFirstLetter);
            }
            return Country.Unknown;
        }
    }
}