﻿using System;
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
        public static PlayerStatisticViewModel Create(List<PlayerStatisticEntity> statisticEntities, List<TankJsonV2> tanks, string name, DateTime created, ServerStatWrapper playerData)
        {
            PlayerStatisticEntity currentStatistic = statisticEntities.OrderByDescending(x => x.BattlesCount).First();
            List<PlayerStatisticViewModel> oldStatisticEntities = statisticEntities.Where(x => x.Id != currentStatistic.Id)
                .Select(Create).ToList();

            PlayerStatisticViewModel currentStatisticViewModel = new PlayerStatisticViewModel(currentStatistic, oldStatisticEntities);
            currentStatisticViewModel.Name = name;
            currentStatisticViewModel.Created = created;
            currentStatisticViewModel.DamageTaken = tanks.Sum(x => x.A15x15.damageReceived);
            currentStatisticViewModel.BattlesPerDay = currentStatisticViewModel.BattlesCount / (DateTime.Now - created).Days;
            currentStatisticViewModel.PerformanceRating = GetPerformanceRating(currentStatisticViewModel, tanks);
            currentStatisticViewModel.WN8Rating = GetWN8Rating(currentStatisticViewModel, tanks);
            currentStatisticViewModel.RBR = GetRBR(currentStatisticViewModel, tanks);

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

        private static double GetPerformanceRating(PlayerStatisticViewModel playerStatistic, List<TankJsonV2> tanks)
        {
            double expDamage = tanks.Select(x => x.A15x15.battlesCount * x.Description.Expectancy.PRNominalDamage).Sum();
            return RatingHelper.PerformanceRating(playerStatistic.BattlesCount, playerStatistic.Wins, expDamage, playerStatistic.DamageDealt, playerStatistic.Tier);
        }

        private static double GetWN8Rating(PlayerStatisticViewModel playerStatistic, List<TankJsonV2> tanks)
        {
            double battles = (double)tanks.Sum(x => x.A15x15.battlesCount);

            double damage = tanks.Select(x => x.A15x15.damageDealt).Sum() / battles;
            double spotted = tanks.Select(x => x.A15x15.spotted).Sum() / battles;
            double def = tanks.Select(x => x.A15x15.droppedCapturePoints).Sum() / battles;
            double winRate = tanks.Sum(x => x.A15x15.wins) / battles;
            double frags = tanks.Select(x => x.A15x15.frags).Sum() / battles;

            //double damage = playerStatistic.AvgDamageDealt;
            //double spotted = playerStatistic.AvgSpotted;
            //double def = playerStatistic.AvgDroppedCapturePoints;
            //double winRate = playerStatistic.WinsPercent / 100.0;
            //double frags = playerStatistic.AvgFrags;

            double expDamage = tanks.Select(x => x.A15x15.battlesCount * x.Description.Expectancy.Wn8NominalDamage).Sum() / battles;
            double expSpotted = tanks.Select(x => x.A15x15.battlesCount * x.Description.Expectancy.Wn8NominalSpotted).Sum() / battles;
            double expDef = tanks.Select(x => x.A15x15.battlesCount * x.Description.Expectancy.Wn8NominalDefence).Sum() / battles;
            double expWinRate = tanks.Average(x => x.Description.Expectancy.Wn8NominalWinRate) / 100.0;
            double expFrags = tanks.Select(x => x.A15x15.battlesCount * x.Description.Expectancy.Wn8NominalFrags).Sum() / battles;
            return RatingHelper.CalcWN8(damage, expDamage, frags, expFrags, spotted, expSpotted, def, expDef, winRate, expWinRate);
        }

        private static double GetRBR(PlayerStatisticViewModel playerStatistic, List<TankJsonV2> tanks)
        {
            int battlesCount88 = playerStatistic.BattlesCount - tanks.Sum(x => x.A15x15.battlesCountBefore8_8 != 0 ? x.A15x15.battlesCountBefore8_8 : x.A15x15.battlesCount);
            int xp88 = tanks.Sum(x => x.A15x15.originalXP);
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
            IEnumerable<TankJsonV2> statisticViewModels = tankStatisticEntities.Select(x => UnZipObject(x.Raw)).ToList();
            TankJsonV2 currentStatistic = statisticViewModels.OrderByDescending(x => x.A15x15.battlesCount).First();
            IEnumerable<TankJsonV2> prevStatisticViewModels =
                statisticViewModels.Where(x => x.A15x15.battlesCount != currentStatistic.A15x15.battlesCount);
            TankStatisticRowViewModel model = new TankStatisticRowViewModel(currentStatistic, prevStatisticViewModels);
            model.IsFavorite = tankStatisticEntities.First().TankIdObject.IsFavorite;
            return model;
        }

        private static TankJsonV2 UnZipObject(byte[] x)
        {
            TankJsonV2 tankJson = WotApiHelper.UnZipObject<TankJsonV2>(x);
            WotApiClient.Instance.ExtendPropertiesData(tankJson);
            return tankJson;
        }
    }
}
