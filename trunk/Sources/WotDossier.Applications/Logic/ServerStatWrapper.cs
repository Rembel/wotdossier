using WotDossier.Domain.Server;

namespace WotDossier.Applications.Logic
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

        public Ratings Ratings { get; set; }

        public ClanData Clan { get; set; }
        public string Role { get; set; }
        public long Since { get; set; }
    }
}
