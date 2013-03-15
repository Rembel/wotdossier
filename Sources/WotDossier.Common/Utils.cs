using System;

namespace WotDossier.Common
{
    public class Utils
    {
        public static DateTime UnixDateToDateTime(long value)
        {
            return new DateTime(1970, 1, 1).AddSeconds(value);
        }
    }
}
