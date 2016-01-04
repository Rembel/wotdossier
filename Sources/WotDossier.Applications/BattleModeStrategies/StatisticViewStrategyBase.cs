using System;
using System.Collections.Generic;
using System.Linq;
using WotDossier.Applications.ViewModel.Rows;
using WotDossier.Applications.ViewModel.Statistic;
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
            Player playerData = null);

        /// <summary>
        /// Gets the player statistic.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="player">The player.</param>
        /// <param name="tanks">The tanks.</param>
        /// <param name="playerData">The player data.</param>
        /// <returns></returns>
        protected PlayerStatisticViewModel GetPlayerStatistic<T>(PlayerEntity player, List<TankJson> tanks, Player playerData = null)
            where T : StatisticEntity
        {
            List<T> statisticEntities = DossierRepository.GetPlayerStatistic<T>(player.AccountId).ToList();

            T currentStatistic = statisticEntities.OrderByDescending(x => x.BattlesCount).First();
            List<StatisticSlice> oldStatisticEntities = statisticEntities.Where(x => x.Id != currentStatistic.Id)
                .Select(x => ToViewModel(x).ToStatisticSlice()).ToList();

            PlayerStatisticViewModel currentStatisticViewModel = ToViewModel(currentStatistic, oldStatisticEntities);
            currentStatisticViewModel.Name = player.Name;
            currentStatisticViewModel.Created = player.Creaded;
            currentStatisticViewModel.AccountId = player.AccountId;
            var days = (DateTime.Now - player.Creaded).Days;
            currentStatisticViewModel.BattlesPerDay = currentStatisticViewModel.BattlesCount / (days == 0 ? 1 : days);
            currentStatisticViewModel.PlayTime = new TimeSpan(0, 0, 0, tanks.Sum(x => x.Common.battleLifeTime));

            return currentStatisticViewModel;
        }

        /// <summary>
        /// Updates the player statistic.
        /// </summary>
        /// <param name="playerId">The player identifier.</param>
        /// <param name="tanks">The tanks.</param>
        /// <param name="serverStatistic">The server statistic.</param>
        /// <returns></returns>
        public abstract PlayerEntity UpdatePlayerStatistic(int playerId, List<TankJson> tanks, Player serverStatistic);

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
        protected abstract PlayerStatisticViewModel ToViewModel(StatisticEntity currentStatistic, List<StatisticSlice> oldStatisticEntities);

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
            var playerEntity = DossierRepository.GetPlayerById(playerId);
            IEnumerable<T> entities = DossierRepository.GetTanksStatistic<T>(playerId);

            return entities.GroupBy(x => x.TankId).Select(x =>
            {
                var row = ToTankStatisticRow(x, Predicate);
                row.PlayerId = playerId;
                row.PlayerName = playerEntity.Name;
                return row;
            }).OrderByDescending(x => x.Tier).ThenBy(x => x.Tank).Where(x => x.BattlesCount > 0).ToList();
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
        protected abstract ITankStatisticRow ToTankStatisticRow(TankJson currentStatistic, List<StatisticSlice> prevStatisticViewModels = null);

        /// <summary>
        /// To the tank statistic row.
        /// </summary>
        /// <param name="groupedEntities">The tank statistic entities grouped by tankId.</param>
        /// <param name="predicate">Predicate to get tank statistic</param>
        /// <returns></returns>
        protected ITankStatisticRow ToTankStatisticRow(IGrouping<int, TankStatisticEntityBase> groupedEntities, Func<TankJson, StatisticJson> predicate)
        {
            var lastStatisticEntity = groupedEntities.OrderByDescending(x => x.BattlesCount).First();
            List<TankStatisticEntityBase> oldStatisticEntities = groupedEntities.Where(x => x.BattlesCount != lastStatisticEntity.BattlesCount).ToList();

            TankJson lastStatistic = UnZipObject(lastStatisticEntity.Raw);
            var model = ToTankStatisticRow(lastStatistic, ToStatisticSlices(oldStatisticEntities));
            model.IsFavorite = groupedEntities.First().TankIdObject.IsFavorite;
            return model;
        }

        private List<StatisticSlice> ToStatisticSlices(List<TankStatisticEntityBase> slices)
        {
            var statisticSlices = slices.Select(
                delegate(TankStatisticEntityBase x)
                {
                    return new StatisticSlice(x.Updated,
                        new Lazy<PeriodStatisticViewModel>(
                            () =>
                                (PeriodStatisticViewModel)
                                    ToTankStatisticRow(UnZipObject(x.Raw), new List<StatisticSlice>())));

                }).ToList();

            if (!statisticSlices.Any())
            {
                statisticSlices.Add(new StatisticSlice(DateTime.MinValue, (PeriodStatisticViewModel)ToTankStatisticRow( TankJson.Initial)));
            }

            return statisticSlices;
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
            return !Dictionaries.Instance.NotExistsedTanksList.Contains(tankDescription.UniqueId());
        }

        /// <summary>
        /// Unszips object from byte array.
        /// </summary>
        /// <param name="x">The byte array.</param>
        /// <returns></returns>
        private static TankJson UnZipObject(byte[] x)
        {
            TankJson tankJson = CompressHelper.DecompressObject<TankJson>(x);
            CacheFileHelper.ExtendPropertiesData(tankJson);
            return tankJson;
        }
    }
}
