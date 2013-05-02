using System;

namespace WotDossier.Common
{
    public class Utils
    {
        public static DateTime UnixDateToDateTime(long value)
        {
            return new DateTime(1970, 1, 1).AddSeconds(value);
        }

        public static int ToUniqueId(int countryId, int tankId)
        {
            return countryId * 10000 + tankId;
        }
    }
}
