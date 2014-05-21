using System.Globalization;
using System.Threading;

namespace WotDossier.Applications
{
    public class CultureHelper
    {
        public static void SetUiCulture()
        {
            var culture = new CultureInfo(SettingsReader.Get().Language);
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }
    }
}
