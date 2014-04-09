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
        private readonly List<int> _notExistsedTanksList = new List<int>
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
        public List<int> NotExistsedTanksList
        {
            get { return _notExistsedTanksList; }
        }

        public List<TankRowMasterTanker> GetMasterTankerList(List<ITankStatisticRow> tanks)
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

        private bool IsExistedtank(TankDescription tankDescription)
        {
            return !NotExistsedTanksList.Contains(tankDescription.UniqueId());
        }

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

        public List<ITankStatisticRow> CreateStatistic(IEnumerable<TankStatisticEntity> tankStatisticEntities, Func<TankJson, StatisticJson> func)
        {
            return tankStatisticEntities.GroupBy(x => x.TankId).Select(x => ToStatisticViewModel(x, func)).OrderByDescending(x => x.Tier).ThenBy(x => x.Tank).Where(x => x.BattlesCount > 0).ToList();
        }

        private ITankStatisticRow ToStatisticViewModel(IGrouping<int, TankStatisticEntity> tankStatisticEntities, Func<TankJson, StatisticJson> func)
        {
            List<TankJson> statisticViewModels = tankStatisticEntities.Select(x => UnZipObject(x.Raw)).ToList();
            TankJson currentStatistic = statisticViewModels.OrderByDescending(x => func(x).battlesCount).First();
            List<TankJson> prevStatisticViewModels = statisticViewModels.Where(x => func(x).battlesCount != func(currentStatistic).battlesCount).ToList();
            var model = GetTankStatisticRow(currentStatistic, prevStatisticViewModels);
            model.IsFavorite = tankStatisticEntities.First().TankIdObject.IsFavorite;
            return model;
        }

        protected abstract ITankStatisticRow GetTankStatisticRow(TankJson currentStatistic, List<TankJson> prevStatisticViewModels);

        private static TankJson UnZipObject(byte[] x)
        {
            TankJson tankJson = WotApiHelper.UnZipObject<TankJson>(x);
            WotApiClient.Instance.ExtendPropertiesData(tankJson);
            return tankJson;
        }

        protected abstract PlayerStatisticViewModel GetModel(StatisticEntity currentStatistic);

        protected abstract PlayerStatisticViewModel GetModel(StatisticEntity currentStatistic,
            List<PlayerStatisticViewModel> oldStatisticEntities);

        public abstract List<StatisticEntity> GetStatistic(DossierRepository repository, PlayerEntity player);
    }
}
