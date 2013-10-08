namespace WotDossier.Domain.Tank
{
    public class TankServerInfo
    {
        public string name;
        public int level;
        public string nation;
        public bool is_premium;
        public string type;
        public int tank_id;
    
        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return name;
        }
    }
}
