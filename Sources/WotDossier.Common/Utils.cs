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

        public static int ToTypeId(int typeCompDescr)
        {
            return typeCompDescr & 15;
        }

        public static int TypeCompDesc(int countryId, int tankId, int type = 1)
        {
            return (type & 15) | (countryId << 4 & 255) | (tankId << 8 & 65535);
        }

        public static int ToUniqueId(int countryId, int tankId)
        {
            return countryId * 10000 + tankId;
        }

        public static string ToRoman(int number)
        {
            if ((number < 0) || (number > 3999)) throw new ArgumentOutOfRangeException("insert value betwheen 1 and 3999");
            if (number < 1) return string.Empty;
            if (number >= 1000) return "M" + ToRoman(number - 1000);
            if (number >= 900) return "CM" + ToRoman(number - 900); //EDIT: i've typed 400 instead 900
            if (number >= 500) return "D" + ToRoman(number - 500);
            if (number >= 400) return "CD" + ToRoman(number - 400);
            if (number >= 100) return "C" + ToRoman(number - 100);
            if (number >= 90) return "XC" + ToRoman(number - 90);
            if (number >= 50) return "L" + ToRoman(number - 50);
            if (number >= 40) return "XL" + ToRoman(number - 40);
            if (number >= 10) return "X" + ToRoman(number - 10);
            if (number >= 9) return "IX" + ToRoman(number - 9);
            if (number >= 5) return "V" + ToRoman(number - 5);
            if (number >= 4) return "IV" + ToRoman(number - 4);
            if (number >= 1) return "I" + ToRoman(number - 1);
            throw new ArgumentOutOfRangeException("something bad happened");
        }
    }
}
