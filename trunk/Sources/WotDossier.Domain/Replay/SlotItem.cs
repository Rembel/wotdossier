namespace WotDossier.Domain.Replay
{
    public class SlotItem
    {
        private readonly SlotType _typeId;
        private readonly ulong _country;
        private readonly ulong _id;

        public SlotType type_id
        {
            get { return _typeId; }
        }

        public ulong country
        {
            get { return _country; }
        }

        public ulong id
        {
            get { return _id; }
        }

        public SlotItem(SlotType typeId, ulong country, ulong id)
        {
            _typeId = typeId;
            _country = country;
            _id = id;
        }
    }
}