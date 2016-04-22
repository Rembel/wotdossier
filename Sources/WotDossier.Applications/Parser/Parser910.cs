namespace WotDossier.Applications.Parser
{
    public class Parser910 : Parser99
    {
        protected override ulong UpdateEvent_Slot
        {
            get { return 0x0b; }
        }

        protected override ulong UpdateEvent_Arena
        {
            get { return 0x26; }
        }
    }
}