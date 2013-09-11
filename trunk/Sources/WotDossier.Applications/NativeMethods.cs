using System.Runtime.InteropServices;
using System.Text;

namespace WotDossier.Applications
{
    internal static class NativeMethods
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA2101:SpecifyMarshalingForPInvokeStringArguments", MessageId = "0"), 
        System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA2101:SpecifyMarshalingForPInvokeStringArguments", MessageId = "1"), 
        System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA2101:SpecifyMarshalingForPInvokeStringArguments", MessageId = "2"), 
        DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool InternetGetCookie(string lpszUrlName, string lpszCookieName, [Out] StringBuilder lpszCookieData, [MarshalAs(UnmanagedType.U4)] out int lpdwSize);
    }
}
