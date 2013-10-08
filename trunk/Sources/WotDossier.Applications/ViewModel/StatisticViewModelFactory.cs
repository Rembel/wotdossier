using System;
using System.Collections.Generic;
using System.Linq;
using WotDossier.Applications.Logic;
using WotDossier.Applications.ViewModel.Rows;
using WotDossier.Common;
using WotDossier.Dal;
using WotDossier.Domain.Entities;
using WotDossier.Domain.Player;
using WotDossier.Domain.Tank;

namespace WotDossier.Applications.ViewModel
{
    public class StatisticViewModelFactory
    {
        public static PlayerStatisticViewModel Create(List<PlayerStatisticEntity> statisticEntities, List<TankJson> tanks, string name, DateTime created, ServerStatWrapper playerData)
        {
            PlayerStatisticEntity currentStatistic = statisticEntities.OrderByDescending(x => x.BattlesCount).First();
            List<PlayerStatisticViewModel> oldStatisticEntities = statisticEntities.Where(x => x.Id != currentStatistic.Id)
                .Select(Create).ToList();

            PlayerStatisticViewModel currentStatisticViewModel = new PlayerStatisticViewModel(currentStatistic, oldStatisticEntities);
            currentStatisticViewModel.Name = name;
            currentStatisticViewModel.Created = created;
            currentStatisticViewModel.DamageTaken = tanks.Sum(x => x.Tankdata.damageReceived);
            currentStatisticViewModel.BattlesPerDay = currentStatisticViewModel.BattlesCount / (DateTime.Now - created).Days;
            currentStatisticViewModel.PerformanceRating = GetPerformanceRating(currentStatisticViewModel, tanks);
            currentStatisticViewModel.RBR = GetRBR(currentStatisticViewModel, tanks);

            if (playerData.Clan != null)
            {
                currentStatisticViewModel.Clan = new PlayerStatisticClanViewModel(playerData.Clan, playerData.Role, playerData.Since);
            }
            return currentStatisticViewModel;
        }

        public static PlayerStatisticViewModel Create(PlayerStatisticEntity statisticEntity)
        {
            return new PlayerStatisticViewModel(statisticEntity);
        }

        private static double GetPerformanceRating(PlayerStatisticViewModel playerStatistic, List<TankJson> tanks)
        {
            double damage = tanks.Join(WotApiClient.Instance.TanksDictionary.Values, x => x.UniqueId(), y => y.UniqueId(),
                (x, y) => x.Tankdata.battlesCount * y.nominal_damage).Sum();

            return RatingHelper.PerformanceRating(playerStatistic.BattlesCount, playerStatistic.Wins, damage, playerStatistic.DamageDealt, playerStatistic.Tier);
        }

        private static double GetRBR(PlayerStatisticViewModel playerStatistic, List<TankJson> tanks)
        {
            int battlesCount88 = playerStatistic.BattlesCount - tanks.Sum(x => x.Tankdata.battlesCountBefore8_8 != 0 ? x.Tankdata.battlesCountBefore8_8 : x.Tankdata.battlesCount);
            int xp88 = tanks.Sum(x => x.Tankdata.originalXP);
            double avgXP88 = xp88 / (double)(battlesCount88 != 0 ? battlesCount88 : 1);

            double rbr = RatingHelper.RBR(playerStatistic.BattlesCount, battlesCount88, playerStatistic.Wins / (double)playerStatistic.BattlesCount,
                playerStatistic.SurvivedBattles / (double)playerStatistic.BattlesCount, playerStatistic.HitsPercents / 100.0, playerStatistic.AvgDamageDealt, avgXP88);
            return rbr;
        }

        public static List<TankStatisticRowViewModel> Create(IEnumerable<TankStatisticEntity> tankStatisticEntities)
        {
            return tankStatisticEntities.GroupBy(x => x.TankId).Select(ToStatisticViewModel).OrderByDescending(x => x.Tier).ThenBy(x => x.Tank).ToList();
        }

        private static TankStatisticRowViewModel ToStatisticViewModel(IGrouping<int, TankStatisticEntity> tankStatisticEntities)
        {
            IEnumerable<TankJson> statisticViewModels = tankStatisticEntities.Select(x => UnZipObject(x.Raw)).ToList();
            TankJson currentStatistic = statisticViewModels.OrderByDescending(x => x.Tankdata.battlesCount).First();
            IEnumerable<TankJson> prevStatisticViewModels =
                statisticViewModels.Where(x => x.Tankdata.battlesCount != currentStatistic.Tankdata.battlesCount);
            TankStatisticRowViewModel model = new TankStatisticRowViewModel(currentStatistic, prevStatisticViewModels);
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
