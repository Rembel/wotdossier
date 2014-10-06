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

        protected bool Equals(SlotItem other)
        {
            return _typeId == other._typeId && _country == other._country && _id == other._id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((SlotItem) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (int) _typeId;
                hashCode = (hashCode*397) ^ _country;
                hashCode = (hashCode*397) ^ _id;
                return hashCode;
            }
        }
    }
}