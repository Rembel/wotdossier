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
                Clan = playerStat.dataField.clanData;
                if (playerStat.dataField.clan != null)
                {
                    Role = playerStat.dataField.clan.role;
                    Since = playerStat.dataField.clan.since;
                }
                Ratings = playerStat.dataField.ratings;
            }
        }

        public Ratings Ratings { get; set; }

        public ClanData Clan { get; set; }
        public string Role { get; set; }
        public long Since { get; set; }
    }
}
