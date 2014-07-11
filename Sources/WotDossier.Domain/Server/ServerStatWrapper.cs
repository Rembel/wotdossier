namespace WotDossier.Domain.Server
{
    public class ServerStatWrapper
    {
        private readonly Player _player;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public ServerStatWrapper(Player player)
        {
            _player = player;

            if (player != null && player.data != null)
            {
                Ratings = player.dataField.ratings;
            }
        }

        /// <summary>
        /// Gets or sets the ratings.
        /// </summary>
        /// <value>
        /// The ratings.
        /// </value>
        public Ratings Ratings { get; set; }

        /// <summary>
        /// Gets the player.
        /// </summary>
        /// <value>
        /// The player.
        /// </value>
        public Player Player
        {
            get { return _player; }
        }
    }
}
