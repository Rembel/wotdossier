namespace WotDossier.Applications.Parser
{
    public class Parser83 : Parser81
    {
        protected override ulong UpdateEvent_Slot
        {
            get { return 0x0b; }
        }

        protected override ulong UpdateEvent_Arena
        {
            get { return 0x19; }
        }
    }
}