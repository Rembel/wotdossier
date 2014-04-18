using WotDossier.Applications.ViewModel;
using WotDossier.Dal;

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
            return null;
        }
    }
}