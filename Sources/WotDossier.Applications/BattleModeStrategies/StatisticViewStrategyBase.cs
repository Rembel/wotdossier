﻿using System;
using System.Collections.Generic;
using System.Linq;
using WotDossier.Applications.Logic;
using WotDossier.Applications.Model;
using WotDossier.Applications.ViewModel;
using WotDossier.Applications.ViewModel.Rows;
using WotDossier.Dal;
using WotDossier.Domain.Entities;
using WotDossier.Domain.Server;
using WotDossier.Domain.Tank;

namespace WotDossier.Applications.BattleModeStrategies
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class StatisticViewStrategyBase
    {
        private readonly DossierRepository _dossierRepository;

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

        /// <summary>
        /// Gets the not existsed tanks list.
        /// </summary>
        /// <value>
        /// The not existsed tanks list.
        /// </value>
        public List<int> NotExistsedTanksList
        {
            get { return _notExistsedTanksList; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StatisticViewStrategyBase"/> class.
        /// </summary>
        /// <param name="dossierRepository">The dossier repository.</param>
        protected StatisticViewStrategyBase(DossierRepository dossierRepository)
        {
            _dossierRepository = dossierRepository;
        }

        /// <summary>
        /// Predicate to get tank statistic
        /// </summary>
        public abstract Func<TankJson, StatisticJson> Predicate { get; }

        /// <summary>
        /// Gets the dossier repository.
        /// </summary>
        /// <value>
        /// The dossier repository.
        /// </value>
        protected DossierRepository DossierRepository
        {
            get { return _dossierRepository; }
        }

        /// <summary>
        /// Gets the player statistic.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <param name="tanks">The tanks.</param>
        /// <param name="playerData">The player data.</param>
        /// <returns></returns>
        public abstract PlayerStatisticViewModel GetPlayerStatistic(PlayerEntity player, List<TankJson> tanks,
            ServerStatWrapper playerData = null);

        /// <summary>
        /// Gets the player statistic.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="player">The player.</param>
        /// <param name="tanks">The tanks.</param>
        /// <param name="playerData">The player data.</param>
        /// <returns></returns>
        protected PlayerStatisticViewModel GetPlayerStatistic<T>(PlayerEntity player, List<TankJson> tanks, ServerStatWrapper playerData = null)
            where T : StatisticEntity
        {
            List<T> statisticEntities = DossierRepository.GetPlayerStatistic<T>(player.PlayerId).ToList();

            T currentStatistic = statisticEntities.OrderByDescending(x => x.BattlesCount).First();
            List<PlayerStatisticViewModel> oldStatisticEntities = statisticEntities.Where(x => x.Id != currentStatistic.Id)
                .Select(ToViewModel).ToList();

            PlayerStatisticViewModel currentStatisticViewModel = ToViewModel(currentStatistic, oldStatisticEntities);
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

        /// <summary>
        /// Updates the player statistic.
        /// </summary>
        /// <param name="playerId">The player identifier.</param>
        /// <param name="tanks">The tanks.</param>
        /// <param name="ratings">The ratings.</param>
        /// <returns></returns>
        public abstract PlayerEntity UpdatePlayerStatistic(int playerId, List<TankJson> tanks, Ratings ratings);

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="currentStatistic">The current statistic.</param>
        /// <returns></returns>
        protected abstract PlayerStatisticViewModel ToViewModel(StatisticEntity currentStatistic);

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="currentStatistic">The current statistic.</param>
        /// <param name="oldStatisticEntities">The old statistic entities.</param>
        /// <returns></returns>
        protected abstract PlayerStatisticViewModel ToViewModel(StatisticEntity currentStatistic, List<PlayerStatisticViewModel> oldStatisticEntities);

        /// <summary>
        /// Gets the tanks statistic.
        /// </summary>
        /// <param name="playerId">The player identifier.</param>
        /// <returns></returns>
        public abstract List<ITankStatisticRow> GetTanksStatistic(int playerId);

        /// <summary>
        /// Gets the tanks statistic.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="playerId">The player identifier.</param>
        /// <returns></returns>
        protected List<ITankStatisticRow> GetTanksStatistic<T>(int playerId) where T : TankStatisticEntityBase
        {
            IEnumerable<T> entities = DossierRepository.GetTanksStatistic<T>(playerId);

            return entities.GroupBy(x => x.TankId).Select(x => ToTankStatisticRow(x, Predicate)).OrderByDescending(x => x.Tier).ThenBy(x => x.Tank).Where(x => x.BattlesCount > 0).ToList();
        }

        /// <summary>
        /// Updates the tank statistic.
        /// </summary>
        /// <param name="playerId">The player identifier.</param>
        /// <param name="tanks">The tanks.</param>
        /// <returns></returns>
        public abstract PlayerEntity UpdateTankStatistic(int playerId, List<TankJson> tanks);

        /// <summary>
        /// Updates the tank statistic.
        /// </summary>
        /// <param name="playerId">The player identifier.</param>
        /// <param name="tanks">The tanks.</param>
        /// <returns></returns>
        public PlayerEntity UpdateTankStatistic<T>(int playerId, List<TankJson> tanks) where T : TankStatisticEntityBase, new()
        {
            return DossierRepository.UpdateTankStatistic<T>(playerId, tanks, Predicate);
        }

        /// <summary>
        /// To the tank statistic row.
        /// </summary>
        /// <param name="currentStatistic">The current statistic.</param>
        /// <param name="prevStatisticViewModels">The previous statistic view models.</param>
        /// <returns></returns>
        protected abstract ITankStatisticRow ToTankStatisticRow(TankJson currentStatistic, List<TankJson> prevStatisticViewModels);

        /// <summary>
        /// To the tank statistic row.
        /// </summary>
        /// <param name="groupedEntities">The tank statistic entities grouped by tankId.</param>
        /// <param name="predicate">Predicate to get tank statistic</param>
        /// <returns></returns>
        private ITankStatisticRow ToTankStatisticRow(IGrouping<int, TankStatisticEntityBase> groupedEntities, Func<TankJson, StatisticJson> predicate)
        {
            List<TankJson> statisticViewModels = groupedEntities.Select(x => UnZipObject(x.Raw)).ToList();
            TankJson currentStatistic = statisticViewModels.OrderByDescending(x => predicate(x).battlesCount).First();
            List<TankJson> prevStatisticViewModels = statisticViewModels.Where(x => predicate(x).battlesCount != predicate(currentStatistic).battlesCount).ToList();
            var model = ToTankStatisticRow(currentStatistic, prevStatisticViewModels);
            model.IsFavorite = groupedEntities.First().TankIdObject.IsFavorite;
            return model;
        }


        /// <summary>
        /// Gets the master tanker list.
        /// </summary>
        /// <param name="tanks">The tanks.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Determines whether the specified tank description is existed tank.
        /// </summary>
        /// <param name="tankDescription">The tank description.</param>
        /// <returns></returns>
        private bool IsExistedtank(TankDescription tankDescription)
        {
            return !NotExistsedTanksList.Contains(tankDescription.UniqueId());
        }

        /// <summary>
        /// Unszips object from byte array.
        /// </summary>
        /// <param name="x">The byte array.</param>
        /// <returns></returns>
        private static TankJson UnZipObject(byte[] x)
        {
            TankJson tankJson = WotApiHelper.UnZipObject<TankJson>(x);
            WotApiClient.Instance.ExtendPropertiesData(tankJson);
            return tankJson;
        }
    }
}