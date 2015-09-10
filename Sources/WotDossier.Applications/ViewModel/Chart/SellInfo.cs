namespace WotDossier.Applications.ViewModel.Chart
{
    public sealed class SellInfo : GenericPoint<int, double>
    {
        /// <summary>
        /// Gets or sets the win percent.
        /// </summary>
        public double WinPercent { get; set; }

        /// <summary>
        /// Gets or sets the name of the tank.
        /// </summary>
        public string TankName { get; set; }

        /// <summary>
        /// Gets or sets the battles.
        /// </summary>
        public double Battles { get; set; }

        public SellInfo(int x, double y, string tank) : base(x, y)
        {
            Battles = x;
            WinPercent = y;
            TankName = tank;
        }

        public override string ToString()
        {
            return string.Format("{0}: battles: {1}, {2:0.0}%", TankName, Battles, WinPercent);
        }
    }
}