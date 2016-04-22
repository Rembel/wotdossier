namespace WotDossier.Applications.Parser
{
    public class Parser85 : Parser83
    {
        protected override ulong UpdateEvent_Slot
        {
            get { return 0x0a; }
        }

        protected override ulong UpdateEvent_Arena
        {
            get { return 0x18; }
        }
    }
}