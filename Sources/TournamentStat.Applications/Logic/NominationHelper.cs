using System.Collections.Generic;
using System.Linq;
using TournamentStat.Applications.ViewModel;
using WotDossier.Applications.ViewModel.Rows;
using WotDossier.Applications.ViewModel.Statistic;

namespace TournamentStat.Applications.Logic
{
    public class NominationHelper
    {
        public static List<ITankStatisticRow> GetNominationResults(TournamentNomination nomination, List<ITankStatisticRow> results)
        {
            var nominationTanks = nomination.TournamentTanks.Select(x => x.TankUniqueId).ToList();
            var tankStatisticRows = results.Where(x => nominationTanks.Contains(x.TankUniqueId));
            if (nomination.Criterion == TournamentCriterion.Damage)
            {
                return
                    tankStatisticRows.OrderByDescending(
                        x => ((StatisticViewModelBase)x).AvgDamageDealtForPeriod).ToList();
            }
            if (nomination.Criterion == TournamentCriterion.DamageWithAssist)
            {
                return
                    tankStatisticRows.OrderByDescending(
                        x =>
                            ((TankStatisticRowViewModelBase)x).AvgDamageAssistedForPeriod +
                            ((TankStatisticRowViewModelBase)x).AvgDamageDealtForPeriod).ToList();
            }
            if (nomination.Criterion == TournamentCriterion.WinPercent)
            {
                return
                    tankStatisticRows.OrderByDescending(
                        x => ((TankStatisticRowViewModelBase)x).WinsPercentForPeriod).ToList();
            }
            if (nomination.Criterion == TournamentCriterion.Frags)
            {
                return
                    tankStatisticRows.OrderByDescending(
                        x => ((TankStatisticRowViewModelBase)x).AvgFragsForPeriod).ToList();
            }
            if (nomination.Criterion == TournamentCriterion.DamageWithArmor)
            {
                return
                    tankStatisticRows.OrderByDescending(
                        x =>
                            ((TankStatisticRowViewModelBase)x).AvgDamageDealtForPeriod +
                            ((TankStatisticRowViewModelBase)x).AvgPotentialDamageReceivedForPeriod).ToList();
            }

            return results;
        } 
    }
}
