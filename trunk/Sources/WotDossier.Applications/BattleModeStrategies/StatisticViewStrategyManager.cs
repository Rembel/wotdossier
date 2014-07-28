using WotDossier.Dal;
using WotDossier.Domain;

namespace WotDossier.Applications.BattleModeStrategies
{
    public class StatisticViewStrategyManager
    {
        public static StatisticViewStrategyBase Get(BattleMode randomCompany, DossierRepository dossierRepository)
        {
            if (randomCompany == BattleMode.RandomCompany)
            {
                return new RandomStatisticViewStrategy(dossierRepository);
            }
            if (randomCompany == BattleMode.HistoricalBattle)
            {
                return new HistoricalStatisticViewStrategy(dossierRepository);
            }
            if (randomCompany == BattleMode.TeamBattle)
            {
                return new TeamStatisticViewStrategy(dossierRepository);
            }
            if (randomCompany == BattleMode.Clan)
            {
                return new ClanStatisticViewStrategy(dossierRepository);
            }
            return null;
        }
    }
}