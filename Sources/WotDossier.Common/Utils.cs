using System;

namespace WotDossier.Common
{
    public class Utils
    {
        public static DateTime UnixDateToDateTime(long value)
        {
            return new DateTime(1970, 1, 1).AddSeconds(value);
        }

        public static DateTime? UnixDateToDateTime(long value, bool nullable, bool toLocal)
        {
            if (value == 0 && nullable)
            {
                return null;    
            }

            DateTime time = new DateTime(1970, 1, 1).AddSeconds(value);

            if (toLocal)
            {
                time = time.ToLocalTime();
            }

            return time;
        }

        public static int ToUniqueId(int typeCompDescr)
        {
            int tankId = ToTankId(typeCompDescr);
            int countryId = ToCountryId(typeCompDescr);

            return ToUniqueId(countryId, tankId);
        }

        public static int ToCountryId(int typeCompDescr)
        {
            return typeCompDescr >> 4 & 15;
        }

        public static int ToTankId(int typeCompDescr)
        {
            return typeCompDescr >> 8 & 65535;
        }

        public static int ToUniqueId(int countryId, int tankId)
        {
            return countryId * 10000 + tankId;
        }
    }
}
