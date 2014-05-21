namespace WotDossier.Domain.Tank
{
    public class LevelRange
    {
        public static LevelRange All = new LevelRange{Min = 1, Max = 11};
        public int Min { get; set; }
        public int Max { get; set; }

        public override string ToString()
        {
            return string.Format("{0}-{1}", Min, Max);
        }
    }
}