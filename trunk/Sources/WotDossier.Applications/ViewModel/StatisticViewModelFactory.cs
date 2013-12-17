using System;
using System.Collections.Generic;
using System.Linq;
using WotDossier.Applications.Logic;
using WotDossier.Applications.Model;
using WotDossier.Applications.ViewModel.Rows;
using WotDossier.Common;
using WotDossier.Dal;
using WotDossier.Domain.Entities;
using WotDossier.Domain.Tank;

namespace WotDossier.Applications.ViewModel
{
    public class StatisticViewModelFactory
    {
        public static PlayerStatisticViewModel Create(List<PlayerStatisticEntity> statisticEntities, List<TankJson> tanks, PlayerEntity player, ServerStatWrapper playerData)
        {
            PlayerStatisticEntity currentStatistic = statisticEntities.OrderByDescending(x => x.BattlesCount).First();
            List<PlayerStatisticViewModel> oldStatisticEntities = statisticEntities.Where(x => x.Id != currentStatistic.Id)
                .Select(Create).ToList();

            PlayerStatisticViewModel currentStatisticViewModel = new PlayerStatisticViewModel(currentStatistic, oldStatisticEntities);
            currentStatisticViewModel.Name = player.Name;
            currentStatisticViewModel.Created = player.Creaded;
            currentStatisticViewModel.AccountId = player.PlayerId;
            currentStatisticViewModel.DamageTaken = tanks.Sum(x => x.A15x15.damageReceived);
            currentStatisticViewModel.BattlesPerDay = currentStatisticViewModel.BattlesCount / (DateTime.Now - player.Creaded).Days;
            currentStatisticViewModel.PerformanceRating = GetPerformanceRating(currentStatisticViewModel, tanks);
            currentStatisticViewModel.WN8Rating = GetWN8Rating(currentStatisticViewModel, tanks);
            currentStatisticViewModel.RBR = GetRBR(currentStatisticViewModel, tanks);
            currentStatisticViewModel.PlayTime = new TimeSpan(0, 0, 0, tanks.Sum(x => x.Common.battleLifeTime));

            if (playerData.Clan != null)
            {
                currentStatisticViewModel.Clan = new ClanModel(playerData.Clan, playerData.Role, playerData.Since);
            }
            return currentStatisticViewModel;
        }

        public static PlayerStatisticViewModel Create(PlayerStatisticEntity statisticEntity)
        {
            return new PlayerStatisticViewModel(statisticEntity);
        }

        private static double GetPerformanceRating(PlayerStatisticViewModel playerStatistic, List<TankJson> tanks)
        {
            double expDamage = tanks.Select(x => x.A15x15.battlesCount * x.Description.Expectancy.PRNominalDamage).Sum();
            return RatingHelper.PerformanceRating(playerStatistic.BattlesCount, playerStatistic.Wins, expDamage, playerStatistic.DamageDealt, playerStatistic.Tier);
        }

        private static double GetWN8Rating(PlayerStatisticViewModel playerStatistic, List<TankJson> tanks)
        {
            double battles = tanks.Sum(x => x.A15x15.battlesCount);
            
            double damage = tanks.Select(x => x.A15x15.damageDealt).Sum() / battles;
            double spotted = tanks.Select(x => x.A15x15.spotted).Sum() / battles;
            double def = tanks.Select(x => x.A15x15.droppedCapturePoints).Sum() / battles;
            double winRate = tanks.Sum(x => x.A15x15.wins) / battles;
            double frags = tanks.Select(x => x.A15x15.frags).Sum() / battles;

            double expDamage = tanks.Select(x => x.A15x15.battlesCount * x.Description.Expectancy.Wn8NominalDamage).Sum() / battles;
            double expSpotted = tanks.Select(x => x.A15x15.battlesCount * x.Description.Expectancy.Wn8NominalSpotted).Sum() / battles;
            double expDef = tanks.Select(x => x.A15x15.battlesCount * x.Description.Expectancy.Wn8NominalDefence).Sum() / battles;
            double expWinRate = tanks.Select(x => (x.A15x15.battlesCount * x.Description.Expectancy.Wn8NominalWinRate) / 100.0).Sum() / battles;
            double expFrags = tanks.Select(x => x.A15x15.battlesCount * x.Description.Expectancy.Wn8NominalFrags).Sum() / battles;
            return RatingHelper.CalcWN8(damage, expDamage, frags, expFrags, spotted, expSpotted, def, expDef, winRate, expWinRate);
        }

        private static double GetRBR(PlayerStatisticViewModel playerStatistic, List<TankJson> tanks)
        {
            int battlesCount88 = tanks.Sum(x => x.A15x15.battlesCount - x.A15x15.battlesCountBefore8_8);
            int xp88 = tanks.Sum(x => x.A15x15.originalXP);
            double avgXp88 = xp88 / (double)(battlesCount88 != 0 ? battlesCount88 : 1);
            double wins = playerStatistic.Wins / (double)playerStatistic.BattlesCount;
            double survive = playerStatistic.SurvivedBattles / (double)playerStatistic.BattlesCount;
            double hit = playerStatistic.HitsPercents / 100.0;
            double avgDamageDealt = playerStatistic.AvgDamageDealt;

            double rbr = RatingHelper.RatingWG(playerStatistic.BattlesCount, battlesCount88, wins, survive, hit, avgDamageDealt, avgXp88);
            return rbr;
        }

        public static List<TankStatisticRowViewModel> Create(IEnumerable<TankStatisticEntity> tankStatisticEntities)
        {
            return tankStatisticEntities.GroupBy(x => x.TankId).Select(ToStatisticViewModel).OrderByDescending(x => x.Tier).ThenBy(x => x.Tank).ToList();
        }

        private static TankStatisticRowViewModel ToStatisticViewModel(IGrouping<int, TankStatisticEntity> tankStatisticEntities)
        {
            IEnumerable<TankJson> statisticViewModels = tankStatisticEntities.Select(x => UnZipObject(x.Raw)).ToList();
            TankJson currentStatistic = statisticViewModels.OrderByDescending(x => x.A15x15.battlesCount).First();
            IEnumerable<TankJson> prevStatisticViewModels = statisticViewModels.Where(x => x.A15x15.battlesCount != currentStatistic.A15x15.battlesCount);
            TankStatisticRowViewModel model = new TankStatisticRowViewModel(currentStatistic, prevStatisticViewModels.Any() ? prevStatisticViewModels : new List<TankJson>{TankJson.Initial});
            model.IsFavorite = tankStatisticEntities.First().TankIdObject.IsFavorite;
            return model;
        }

        private static TankJson UnZipObject(byte[] x)
        {
            TankJson tankJson = WotApiHelper.UnZipObject<TankJson>(x);
            WotApiClient.Instance.ExtendPropertiesData(tankJson);
            return tankJson;
        }
    }
}
