namespace WotDossier.Domain.Replay
{
    public class SlotItem
    {
        private readonly SlotType _typeId;
        private readonly int _country;
        private readonly int _id;

        public SlotType TypeId
        {
            get { return _typeId; }
        }

        public int Country
        {
            get { return _country; }
        }

        public int Id
        {
            get { return _id; }
        }

        public SlotItem(SlotType typeId, int country, int id)
        {
            _typeId = typeId;
            _country = country;
            _id = id;
        }
    }
}