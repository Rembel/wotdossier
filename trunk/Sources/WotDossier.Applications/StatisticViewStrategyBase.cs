using System;
using System.Collections.Generic;
using System.Linq;
using WotDossier.Applications.Logic;
using WotDossier.Applications.Model;
using WotDossier.Applications.ViewModel;
using WotDossier.Applications.ViewModel.Rows;
using WotDossier.Dal;
using WotDossier.Domain.Entities;
using WotDossier.Domain.Tank;

namespace WotDossier.Applications
{
    public abstract class StatisticViewStrategyBase
    {
        public abstract List<ITankStatisticRow> CreateStatistic(IEnumerable<TankStatisticEntity> entities);

        public PlayerStatisticViewModel Create<T>(List<T> statisticEntities, List<TankJson> tanks, PlayerEntity player, ServerStatWrapper playerData = null)
            where T : StatisticEntity
        {
            T currentStatistic = statisticEntities.OrderByDescending(x => x.BattlesCount).First();
            List<PlayerStatisticViewModel> oldStatisticEntities = statisticEntities.Where(x => x.Id != currentStatistic.Id)
                .Select(GetModel).ToList();

            PlayerStatisticViewModel currentStatisticViewModel = GetModel(currentStatistic, oldStatisticEntities);
            currentStatisticViewModel.Name = player.Name;
            currentStatisticViewModel.Created = player.Creaded;
            currentStatisticViewModel.AccountId = player.PlayerId;
            currentStatisticViewModel.BattlesPerDay = currentStatisticViewModel.BattlesCount / (DateTime.Now - player.Creaded).Days;
            currentStatisticViewModel.PlayTime = new TimeSpan(0, 0, 0, tanks.Sum(x => x.Common.battleLifeTime));

            if (playerData != null && playerData.Clan != null)
            {
                currentStatisticViewModel.Clan = new ClanModel(playerData.Clan, playerData.Role, playerData.Since);
            }
            return currentStatisticViewModel;
        }

        protected abstract PlayerStatisticViewModel GetModel(StatisticEntity currentStatistic);

        protected abstract PlayerStatisticViewModel GetModel(StatisticEntity currentStatistic,
            List<PlayerStatisticViewModel> oldStatisticEntities);

        public abstract List<StatisticEntity> GetStatistic(DossierRepository repository, PlayerEntity player);
    }

    public class RandomStatisticViewStrategy : StatisticViewStrategyBase
    {
        public override List<ITankStatisticRow> CreateStatistic(IEnumerable<TankStatisticEntity> entities)
        {
            return StatisticViewModelFactory.CreateStatistic(entities, tank => tank.A15x15);
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

    public class TeamStatisticViewStrategy : StatisticViewStrategyBase
    {
        public override List<ITankStatisticRow> CreateStatistic(IEnumerable<TankStatisticEntity> entities)
        {
            return StatisticViewModelFactory.CreateStatistic(entities, tank => tank.A7x7);
        }

        protected override PlayerStatisticViewModel GetModel(StatisticEntity currentStatistic)
        {
            return new TeamPlayerStatisticViewModel((TeamBattlesStatisticEntity)currentStatistic);
        }

        protected override PlayerStatisticViewModel GetModel(StatisticEntity currentStatistic, List<PlayerStatisticViewModel> oldStatisticEntities)
        {
            return new TeamPlayerStatisticViewModel((TeamBattlesStatisticEntity)currentStatistic, oldStatisticEntities);
        }

        public override List<StatisticEntity> GetStatistic(DossierRepository repository, PlayerEntity player)
        {
            return repository.GetStatistic<TeamBattlesStatisticEntity>(player.PlayerId).Cast<StatisticEntity>().ToList();
        }
    }

    public class HistoricalStatisticViewStrategy : StatisticViewStrategyBase
    {
        public override List<ITankStatisticRow> CreateStatistic(IEnumerable<TankStatisticEntity> entities)
        {
            return StatisticViewModelFactory.CreateStatistic(entities, tank => tank.Historical);
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


    public class StatisticViewStrategyManager
    {
        public static StatisticViewStrategyBase Get(BattleMode randomCompany)
        {
            if (randomCompany == BattleMode.RandomCompany)
            {
                return new RandomStatisticViewStrategy();
            }
            if (randomCompany == BattleMode.HistoricalBattle)
            {
                return new HistoricalStatisticViewStrategy();
            }
            if (randomCompany == BattleMode.TeamBattle)
            {
                return new TeamStatisticViewStrategy();
            }
            return null;
        }
    }
}
