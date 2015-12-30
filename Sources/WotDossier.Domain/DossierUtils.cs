namespace WotDossier.Domain
{
    public class DossierUtils
    {
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
    }
}
