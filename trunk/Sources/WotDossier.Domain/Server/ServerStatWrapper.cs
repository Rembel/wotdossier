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
                Clan = player.dataField.clanData;
                if (player.dataField.clan != null)
                {
                    Role = player.dataField.clan.role;
                    Since = player.dataField.clan.since;
                }
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
        /// Gets or sets the clan.
        /// </summary>
        /// <value>
        /// The clan.
        /// </value>
        public ClanData Clan { get; set; }

        /// <summary>
        /// Gets or sets the role.
        /// </summary>
        /// <value>
        /// The role.
        /// </value>
        public string Role { get; set; }

        /// <summary>
        /// Gets or sets the since.
        /// </summary>
        /// <value>
        /// The since.
        /// </value>
        public long Since { get; set; }

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
