namespace WotDossier.Applications.Parser
{
    public class Parser914 : Parser912
    {
        protected override ulong PacketChat
        {
            get { return 0x23; }
        }

        protected override ulong UpdateEvent_Slot
        {
            get { return 0x0a; }
        }

        protected override ulong UpdateEvent_Arena
        {
            get { return 0x29; }
        }
    }
}