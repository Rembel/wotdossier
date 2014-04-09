using System.Collections.Generic;
using System.Linq;
using WotDossier.Applications.ViewModel;
using WotDossier.Applications.ViewModel.Rows;
using WotDossier.Dal;
using WotDossier.Domain.Entities;
using WotDossier.Domain.Tank;

namespace WotDossier.Applications
{
    public class RandomStatisticViewStrategy : StatisticViewStrategyBase
    {
        public override List<ITankStatisticRow> CreateStatistic(IEnumerable<TankStatisticEntity> entities)
        {
            return CreateStatistic(entities, tank => tank.A15x15);
        }

        protected override ITankStatisticRow GetTankStatisticRow(TankJson currentStatistic, List<TankJson> prevStatisticViewModels)
        {
            TankStatisticRowViewModel model = new TankStatisticRowViewModel(currentStatistic, prevStatisticViewModels.Any() ? prevStatisticViewModels : new List<TankJson> { TankJson.Initial });
            return model;
        }

        protected override PlayerStatisticViewModel GetModel(StatisticEntity currentStatistic)
        {
            return new RandomPlayerStatisticViewModel((PlayerStatisticEntity)currentStatistic);
        }

        protected override PlayerStatisticViewModel GetModel(StatisticEntity currentStatistic, List<PlayerStatisticViewModel> oldStatisticEntities)
        {
            return new RandomPlayerStatisticViewModel((PlayerStatisticEntity)currentStatistic, oldStatisticEntities);
        }

        public override List<StatisticEntity> GetStatistic(DossierRepository repository, PlayerEntity player)
        {
            return repository.GetStatistic<PlayerStatisticEntity>(player.PlayerId).Cast<StatisticEntity>().ToList();
        }
    }
}