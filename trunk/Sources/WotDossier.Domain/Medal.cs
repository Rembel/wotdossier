namespace WotDossier.Domain
{
    public class Medal
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }

        protected bool Equals(Medal other)
        {
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Medal) obj);
        }

        public override int GetHashCode()
        {
            return Id;
        }
    }
}