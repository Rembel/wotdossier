using WotDossier.Domain.Player;

namespace WotDossier.Applications.Logic
{
    public class ServerStatWrapper
    {
        private readonly PlayerStat _playerStat;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public ServerStatWrapper(PlayerStat playerStat)
        {
            _playerStat = playerStat;

            if (playerStat != null && playerStat.data != null)
            {
                Clan = playerStat.data.clan;
                Ratings = playerStat.data.ratings;
            }
        }

        public Ratings Ratings { get; set; }

        public Clan Clan { get; set; }
    }
}
