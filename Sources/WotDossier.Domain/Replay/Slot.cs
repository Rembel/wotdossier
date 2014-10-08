namespace WotDossier.Domain.Replay
{
    public class Slot
    {
        private readonly SlotItem _item;
        private readonly int _count;
        private readonly int _rest;

        public SlotItem Item
        {
            get { return _item; }
        }

        public int Count
        {
            get { return _count; }
        }

        public int EndCount { get; set; }

        public int Rest
        {
            get { return _rest; }
        }

        public string FormatedCount
        {
            get { return string.Format("{0}/{1}", EndCount, Count); }
        }

        public object Description { get; set; }

        public Slot(SlotItem item, int count, int rest)
        {
            _item = item;
            EndCount = _count = count;
            _rest = rest;
        }
    }
}