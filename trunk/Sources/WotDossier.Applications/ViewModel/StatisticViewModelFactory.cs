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
        private static readonly List<int> _notExistsedTanksList = new List<int>
                {
                    30251,//T-34-1 training
                    255,//Spectator
                    10226,//pziii_training
                    10227,//pzvib_tiger_ii_training
                    10228,//pzv_training
                    220,//t_34_85_training
                    20212,//m4a3e8_sherman_training
                    5,//KV
                    20009,//T23
                    222,//t44_122
                    223,//t44_85
                    210,//su_85i
                    30002,//Type 59 G
                    20211,//sexton_i
                    30003,//WZ-111
                };
        public static List<int> NotExistsedTanksList
        {
            get { return _notExistsedTanksList; }
        }

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
            currentStatisticViewModel.WN8Rating = GetWN8Rating(tanks);
            currentStatisticViewModel.RBR = GetRBR(currentStatisticViewModel, tanks);
            currentStatisticViewModel.PlayTime = new TimeSpan(0, 0, 0, tanks.Sum(x => x.Common.battleLifeTime));

            if (playerData.Clan != null)
            {
                currentStatisticViewModel.Clan = new ClanModel(playerData.Clan, playerData.Role, playerData.Since);
            }
            return currentStatisticViewModel;
        }

        public static PlayerStatisticViewModel Create(List<TeamBattlesStatisticEntity> statisticEntities, List<TankJson> tanks, PlayerEntity player)
        {
            TeamBattlesStatisticEntity currentStatistic = statisticEntities.OrderByDescending(x => x.BattlesCount).First();
            List<PlayerStatisticViewModel> oldStatisticEntities = statisticEntities.Where(x => x.Id != currentStatistic.Id)
                .Select(Create).ToList();

            PlayerStatisticViewModel currentStatisticViewModel = new PlayerStatisticViewModel(currentStatistic, oldStatisticEntities);
            currentStatisticViewModel.Name = player.Name;
            currentStatisticViewModel.Created = player.Creaded;
            currentStatisticViewModel.AccountId = player.PlayerId;
            currentStatisticViewModel.DamageTaken = tanks.Sum(x => x.A15x15.damageReceived);
            currentStatisticViewModel.BattlesPerDay = currentStatisticViewModel.BattlesCount / (DateTime.Now - player.Creaded).Days;
            currentStatisticViewModel.PerformanceRating = GetPerformanceRating(currentStatisticViewModel, tanks);
            currentStatisticViewModel.WN8Rating = GetWN8Rating(tanks);
            currentStatisticViewModel.RBR = GetRBR(currentStatisticViewModel, tanks);
            currentStatisticViewModel.PlayTime = new TimeSpan(0, 0, 0, tanks.Sum(x => x.Common.battleLifeTime));

            return currentStatisticViewModel;
        }

        public static PlayerStatisticViewModel Create(PlayerStatisticEntity statisticEntity)
        {
            return new PlayerStatisticViewModel(statisticEntity);
        }

        public static PlayerStatisticViewModel Create(TeamBattlesStatisticEntity statisticEntity)
        {
            return new PlayerStatisticViewModel(statisticEntity);
        }

        private static double GetPerformanceRating(PlayerStatisticViewModel playerStatistic, List<TankJson> tanks)
        {
            double expDamage = tanks.Select(x => x.A15x15.battlesCount * x.Description.Expectancy.PRNominalDamage).Sum();
            int battlesCount = playerStatistic.BattlesCount;
            int wins = playerStatistic.Wins;
            int playerDamage = playerStatistic.DamageDealt;
            double avgTier = playerStatistic.Tier;
            return RatingHelper.PerformanceRating(battlesCount, wins, expDamage, playerDamage, avgTier);
        }

        private static double GetPerformanceRating(TankJson tank)
        {
            return RatingHelper.PerformanceRating(tank.A15x15.battlesCount, tank.A15x15.wins,
                tank.A15x15.battlesCount*tank.Description.Expectancy.PRNominalDamage,
                tank.A15x15.damageDealt, tank.Description.Tier);
        }

        private static double GetWN8Rating(List<TankJson> tanks)
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

        private static double GetWN8Rating(TankJson tank)
        {
            double battles = tank.A15x15.battlesCount;

            double damage = tank.A15x15.damageDealt / battles;
            double spotted = tank.A15x15.spotted / battles;
            double def = tank.A15x15.droppedCapturePoints / battles;
            double winRate = tank.A15x15.wins / battles;
            double frags = tank.A15x15.frags / battles;

            double expDamage = tank.A15x15.battlesCount * tank.Description.Expectancy.Wn8NominalDamage / battles;
            double expSpotted = tank.A15x15.battlesCount * tank.Description.Expectancy.Wn8NominalSpotted / battles;
            double expDef = tank.A15x15.battlesCount * tank.Description.Expectancy.Wn8NominalDefence / battles;
            double expWinRate = tank.A15x15.battlesCount * tank.Description.Expectancy.Wn8NominalWinRate / 100.0 / battles;
            double expFrags = tank.A15x15.battlesCount * tank.Description.Expectancy.Wn8NominalFrags / battles;
            return RatingHelper.CalcWN8(damage, expDamage, frags, expFrags, spotted, expSpotted, def, expDef, winRate, expWinRate);
        }

        private static double GetRBR(PlayerStatisticViewModel playerStatistic, List<TankJson> tanks)
        {
            int battlesCount88 = tanks.Sum(x => x.A15x15.battlesCount - x.A15x15.battlesCountBefore8_8);
            battlesCount88 = battlesCount88 != 0 ? battlesCount88 : 1;
            int xp88 = tanks.Sum(x => x.A15x15.originalXP);
            int xpRadio88 = tanks.Sum(x => x.A15x15.damageAssistedRadio);
            int xpTrack88 = tanks.Sum(x => x.A15x15.damageAssistedTrack);
            double avgXp88 = xp88 / (double)battlesCount88;
            double avgXpRadio88 = xpRadio88 / (double)battlesCount88;
            double avgXpTrack88 = xpTrack88 / (double)battlesCount88;
            double wins = playerStatistic.Wins / (double)playerStatistic.BattlesCount;
            double survive = playerStatistic.SurvivedBattles / (double)playerStatistic.BattlesCount;
            double avgDamageDealt = playerStatistic.AvgDamageDealt;

            double rbr = RatingHelper.RatingWG(playerStatistic.BattlesCount, battlesCount88, wins, survive, avgDamageDealt, avgXp88, avgXpRadio88, avgXpTrack88);
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
            model.PerformanceRating = GetPerformanceRating(currentStatistic);
            //model.WN8Rating = GetWN8Rating(currentStatistic);
            return model;
        }

        private static TankJson UnZipObject(byte[] x)
        {
            TankJson tankJson = WotApiHelper.UnZipObject<TankJson>(x);
            WotApiClient.Instance.ExtendPropertiesData(tankJson);
            return tankJson;
        }

        public static List<TankRowMasterTanker> GetMasterTankerList(List<TankStatisticRowViewModel> tanks)
        {
            IEnumerable<int> killed =
                tanks.SelectMany(x => x.TankFrags).Select(x => x.TankUniqueId).Distinct().OrderBy(x => x);
            List<TankRowMasterTanker> masterTanker = Dictionaries.Instance.Tanks
                                                                 .Where(x => !killed.Contains(x.Key) && IsExistedtank(x.Value))
                                                                 .Select(x => new TankRowMasterTanker(x.Value))
                                                                 .OrderBy(x => x.IsPremium)
                                                                 .ThenBy(x => x.Tier).ToList();
            return masterTanker;
        }

        private static bool IsExistedtank(TankDescription tankDescription)
        {
            return !NotExistsedTanksList.Contains(tankDescription.UniqueId());
        }
    }
}
