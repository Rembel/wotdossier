using System;

namespace WotDossier.Framework.Forms
{    
    [Flags]
    public enum WpfMessageBoxButton : short 
    {
        Yes = 1,
        No = 2,
        Cancel = 4,
        OK = 8,
        Close = 16,
        Custom = 32
    }
}
