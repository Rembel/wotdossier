using System;
using System.Collections.Generic;
using System.Linq;
using WotDossier.Applications.Logic;
using WotDossier.Applications.ViewModel.Rows;
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

        public static double GetWN8RatingForPeriod(List<ITankStatisticRow> tanks)
        {
            double battles = tanks.Sum(x => x.BattlesCountDelta);

            double damage = tanks.Sum(x => x.DamageDealtDelta) / battles;
            double spotted = tanks.Sum(x => x.SpottedDelta) / battles;
            double def = tanks.Sum(x => x.DroppedCapturePointsDelta) / battles;
            double winRate = tanks.Sum(x => x.WinsDelta) / battles;
            double frags = tanks.Sum(x => x.FragsDelta) / battles;

            double expDamage = tanks.Sum(x => x.BattlesCountDelta * x.Description.Expectancy.Wn8NominalDamage) / battles;
            double expSpotted = tanks.Sum(x => x.BattlesCountDelta * x.Description.Expectancy.Wn8NominalSpotted) / battles;
            double expDef = tanks.Sum(x => x.BattlesCountDelta * x.Description.Expectancy.Wn8NominalDefence) / battles;
            double expWinRate = tanks.Sum(x => (x.BattlesCountDelta * x.Description.Expectancy.Wn8NominalWinRate) / 100.0) / battles;
            double expFrags = tanks.Sum(x => x.BattlesCountDelta * x.Description.Expectancy.Wn8NominalFrags) / battles;
            return RatingHelper.CalcWN8(damage, expDamage, frags, expFrags, spotted, expSpotted, def, expDef, winRate, expWinRate);
        }

        public static double GetPerformanceRatingForPeriod(List<ITankStatisticRow> tanks)
        {
            double expDamage = tanks.Select(x => x.BattlesCountDelta * x.Description.Expectancy.PRNominalDamage).Sum();
            int battlesCount = tanks.Sum(x => x.BattlesCountDelta);
            int wins = tanks.Sum(x => x.WinsDelta);
            int playerDamage = tanks.Sum(x => x.DamageDealtDelta);
            double avgTier = tanks.Sum(x => x.BattlesCountDelta * x.Tier) / battlesCount;
            return RatingHelper.PerformanceRating(battlesCount, wins, expDamage, playerDamage, avgTier, false);
        }

        public static List<ITankStatisticRow> CreateStatistic(IEnumerable<TankStatisticEntity> tankStatisticEntities, Func<TankJson, StatisticJson> func)
        {
            return tankStatisticEntities.GroupBy(x => x.TankId).Select(x => ToStatisticViewModel(x, func)).OrderByDescending(x => x.Tier).ThenBy(x => x.Tank).Where(x => x.BattlesCount > 0).ToList();
        }

        private static ITankStatisticRow ToStatisticViewModel(IGrouping<int, TankStatisticEntity> tankStatisticEntities, Func<TankJson, StatisticJson> func)
        {
            IEnumerable<TankJson> statisticViewModels = tankStatisticEntities.Select(x => UnZipObject(x.Raw)).ToList();
            TankJson currentStatistic = statisticViewModels.OrderByDescending(x => func(x).battlesCount).First();
            IEnumerable<TankJson> prevStatisticViewModels = statisticViewModels.Where(x => func(x).battlesCount != func(currentStatistic).battlesCount);
            TankStatisticRowViewModel model = new TankStatisticRowViewModel(currentStatistic, prevStatisticViewModels.Any() ? prevStatisticViewModels : new List<TankJson> { TankJson.Initial });
            model.IsFavorite = tankStatisticEntities.First().TankIdObject.IsFavorite;
            return model;
        }

        private static TankJson UnZipObject(byte[] x)
        {
            TankJson tankJson = WotApiHelper.UnZipObject<TankJson>(x);
            WotApiClient.Instance.ExtendPropertiesData(tankJson);
            return tankJson;
        }

        public static List<TankRowMasterTanker> GetMasterTankerList(List<ITankStatisticRow> tanks)
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
