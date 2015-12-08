using System.Globalization;
using System.Threading;
using WotDossier.Dal;

namespace WotDossier.Applications
{
    public class CultureHelper
    {
        public static void SetUiCulture(string language)
        {
            var culture = new CultureInfo(language);
            ConfigureNumberFormat(culture);
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        private static void ConfigureNumberFormat(CultureInfo culture)
        {
            culture.NumberFormat.NumberGroupSeparator = " ";
            culture.NumberFormat.NumberDecimalSeparator = ",";
        }
    }
}
