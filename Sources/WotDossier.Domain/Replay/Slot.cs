namespace WotDossier.Domain.Replay
{
    public class Slot
    {
        private readonly SlotItem _item;
        private readonly ulong _count;
        private readonly ulong _rest;

        public SlotItem item
        {
            get { return _item; }
        }

        public ulong count
        {
            get { return _count; }
        }

        public ulong rest
        {
            get { return _rest; }
        }

        public Slot(SlotItem item, ulong count, ulong rest)
        {
            _item = item;
            _count = count;
            _rest = rest;
        }
    }
}