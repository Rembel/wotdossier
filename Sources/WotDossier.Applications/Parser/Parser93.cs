namespace WotDossier.Applications.Parser
{
    public class Parser93 : BaseParser
    {
        protected override ulong UpdateEvent_Slot
        {
            get { return 0x09; }
        }

        protected override ulong UpdateEvent_Arena
        {
            get { return 0x1e; }
        }
    }
}