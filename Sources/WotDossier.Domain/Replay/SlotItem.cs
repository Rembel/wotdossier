namespace WotDossier.Domain.Replay
{
    public class SlotItem
    {
        private readonly string _typeId;
        private readonly ulong _country;
        private readonly ulong _id;

        public string type_id
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

        public SlotItem(string typeId, ulong country, ulong id)
        {
            _typeId = typeId;
            _country = country;
            _id = id;
        }
    }
}