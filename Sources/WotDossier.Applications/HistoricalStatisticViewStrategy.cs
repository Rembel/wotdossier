using System.Collections.Generic;
using System.Linq;
using WotDossier.Applications.ViewModel;
using WotDossier.Applications.ViewModel.Rows;
using WotDossier.Dal;
using WotDossier.Domain.Entities;
using WotDossier.Domain.Tank;

namespace WotDossier.Applications
{
    public class HistoricalStatisticViewStrategy : StatisticViewStrategyBase
    {
        public override List<ITankStatisticRow> CreateStatistic(IEnumerable<TankStatisticEntity> entities)
        {
            return CreateStatistic(entities, tank => tank.Historical);
        }

        protected override ITankStatisticRow GetTankStatisticRow(TankJson currentStatistic, List<TankJson> prevStatisticViewModels)
        {
            HistoricalBattlesTankStatisticRowViewModel model = new HistoricalBattlesTankStatisticRowViewModel(currentStatistic, prevStatisticViewModels.Any() ? prevStatisticViewModels : new List<TankJson> { TankJson.Initial });
            return model;
        }

        protected override PlayerStatisticViewModel GetModel(StatisticEntity currentStatistic)
        {
            return new HistoricalPlayerStatisticViewModel((HistoricalBattlesStatisticEntity)currentStatistic);
        }

        protected override PlayerStatisticViewModel GetModel(StatisticEntity currentStatistic, List<PlayerStatisticViewModel> oldStatisticEntities)
        {
            return new HistoricalPlayerStatisticViewModel((HistoricalBattlesStatisticEntity)currentStatistic, oldStatisticEntities);
        }

        public override List<StatisticEntity> GetStatistic(DossierRepository repository, PlayerEntity player)
        {
            return repository.GetStatistic<HistoricalBattlesStatisticEntity>(player.PlayerId).Cast<StatisticEntity>().ToList();
        }
    }
}