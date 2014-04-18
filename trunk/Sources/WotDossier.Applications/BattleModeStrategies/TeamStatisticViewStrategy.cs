using System;
using System.Collections.Generic;
using System.Linq;
using WotDossier.Applications.Logic.Adapter;
using WotDossier.Applications.ViewModel;
using WotDossier.Applications.ViewModel.Rows;
using WotDossier.Dal;
using WotDossier.Domain.Entities;
using WotDossier.Domain.Server;
using WotDossier.Domain.Tank;

namespace WotDossier.Applications.BattleModeStrategies
{
    public class TeamStatisticViewStrategy : StatisticViewStrategyBase
    {
        /// <summary>
        /// Predicate to get tank statistic
        /// </summary>
        public override Func<TankJson, StatisticJson> Predicate
        {
            get { return tank => tank.A7x7; }
        }

        public TeamStatisticViewStrategy(DossierRepository dossierRepository) : base(dossierRepository)
        {
        }

        /// <summary>
        /// To the tank statistic row.
        /// </summary>
        /// <param name="currentStatistic">The current statistic.</param>
        /// <param name="prevStatisticViewModels">The previous statistic view models.</param>
        /// <returns></returns>
        protected override ITankStatisticRow ToTankStatisticRow(TankJson currentStatistic, List<TankJson> prevStatisticViewModels)
        {
            TeamBattlesTankStatisticRowViewModel model = new TeamBattlesTankStatisticRowViewModel(currentStatistic, prevStatisticViewModels.Any() ? prevStatisticViewModels : new List<TankJson> { TankJson.Initial });
            return model;
        }

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="currentStatistic">The current statistic.</param>
        /// <returns></returns>
        protected override PlayerStatisticViewModel ToViewModel(StatisticEntity currentStatistic)
        {
            return new TeamPlayerStatisticViewModel((TeamBattlesStatisticEntity)currentStatistic);
        }

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="currentStatistic">The current statistic.</param>
        /// <param name="oldStatisticEntities">The old statistic entities.</param>
        /// <returns></returns>
        protected override PlayerStatisticViewModel ToViewModel(StatisticEntity currentStatistic, List<PlayerStatisticViewModel> oldStatisticEntities)
        {
            return new TeamPlayerStatisticViewModel((TeamBattlesStatisticEntity)currentStatistic, oldStatisticEntities);
        }

        /// <summary>
        /// Gets the statistic slices.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <returns></returns>
        public override List<StatisticEntity> GetStatisticSlices(PlayerEntity player)
        {
            return DossierRepository.GetStatistic<TeamBattlesStatisticEntity>(player.PlayerId).Cast<StatisticEntity>().ToList();
        }

        public override PlayerEntity UpdateTankStatistic(int playerId, List<TankJson> tanks)
        {
            return DossierRepository.UpdateTankStatistic<TankTeamBattleStatisticEntity>(playerId, tanks, Predicate);
        }

        public override IEnumerable<TankStatisticEntityBase> GetTanksStatistic(int playerId)
        {
            return DossierRepository.GetTanksStatistic<TankTeamBattleStatisticEntity>(playerId);
        }

        public override PlayerEntity UpdateStatistic(List<TankJson> tanks, Ratings ratings, int playerId)
        {
            return DossierRepository.UpdateStatistic(new TeamBattlesStatAdapter(tanks), ratings, playerId);
        }
    }
}