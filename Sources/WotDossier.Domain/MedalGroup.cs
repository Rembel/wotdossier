namespace WotDossier.Domain
{
    public class MedalGroup
    {
        public string Name { get; set; }
        public bool Filter { get; set; }

        protected bool Equals(MedalGroup other)
        {
            return string.Equals(Name, other.Name);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            return Equals((MedalGroup) obj);
        }

        public override int GetHashCode()
        {
            return (Name != null ? Name.GetHashCode() : 0);
        }
    }
}