using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
namespace WotDossier.Applications.Parser
{
    public class Parser912 : Parser910
    {
        protected override ulong UpdateEvent_Slot
        {
            get { return 0x09; }
        }

        protected override ulong UpdateEvent_Arena
        {
            get { return 0x28; }
        }
    }
}