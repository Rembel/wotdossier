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
            switch (countryid)
            {
                case 0:
                    return "ussr";
                case 1:
                    return "germany";
                case 2:
                    return "usa";
                case 3:
                    return "china";
                case 4:
                    return "france";
                case 5:
                    return "uk";
                case 6:
                    return "japan";
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
            switch (countryCode.ToLower())
            {
                case "ussr":
                    return 0;
                case "germany":
                    return 1;
                case "usa":
                    return 2;
                case "china":
                    return 3;
                case "france":
                    return 4;
                case "uk":
                    return 5;
                case "jp":
                    return 6;
            }
            return -1;
        }

        /// <summary>
        /// Gets the country identifier by 2 letters code.
        /// </summary>
        /// <param name="countryCode">2 letters country code.</param>
        /// <returns></returns>
        public static int GetCountryIdBy2LettersCode(string countryCode)
        {
            switch (countryCode.ToLower())
            {
                case "ru":
                    return 0;
                case "de":
                    return 1;
                case "us":
                    return 2;
                case "ch":
                    return 3;
                case "fr":
                    return 4;
                case "uk":
                    return 5;
                case "jp":
                    return 6;
            }
            return -1;
        }
    }
}