using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Common.Logging;
using WotDossier.Common;
using WotDossier.Dal.NHibernate;
using WotDossier.Domain.Entities;
using WotDossier.Domain.Replay;
using WotDossier.Domain.Server;
using WotDossier.Domain.Tank;
using System.Linq;

namespace WotDossier.Dal
{
    /// <summary>
    /// 
    /// </summary>
    [Export]
    public class DossierRepository
    {
        protected static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private DataProvider _dataProvider;
        public DataProvider DataProvider
        {
            get { return _dataProvider; }
            set { _dataProvider = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        [ImportingConstructor]
        public DossierRepository([Import(typeof(DataProvider))]DataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        /// <summary>
        /// Gets the player statistic.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="playerId">The player identifier.</param>
        /// <returns></returns>
        public IEnumerable<T> GetPlayerStatistic<T>(int playerId) where T : StatisticEntity
        {
            _dataProvider.OpenSession();
            _dataProvider.BeginTransaction();
            PlayerEntity player = null;
            T statistic = null;
            IList<T> list = null;
            try
            {
                list = _dataProvider.QueryOver(() => statistic)
                                    .Inner.JoinAlias(x => x.PlayerIdObject, () => player)
                                    .Where(x => player.PlayerId == playerId).List<T>();
                _dataProvider.CommitTransaction();
            }
            catch (Exception e)
            {
                Log.Error(e);
                _dataProvider.RollbackTransaction();
            }
            finally
            {
                _dataProvider.CloseSession();
            }
            return list;
        }

        /// <summary>
        /// Updates the player statistic.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="newSnapshot">The new snapshot.</param>
        /// <param name="serverStatistic">The server statistic.</param>
        /// <param name="playerId">The player identifier.</param>
        /// <returns></returns>
        public PlayerEntity UpdatePlayerStatistic<T>(IStatisticAdapter<T> newSnapshot, ServerStatWrapper serverStatistic, int playerId) where T : StatisticEntity, new()
        {
            _dataProvider.OpenSession();
            _dataProvider.BeginTransaction();
            PlayerEntity playerEntity = null;

            try
            {
                playerEntity = GetPlayerInternal(playerId) ??
                               //recreate payer record in case db was deleted but exists user configured in application setting 
                               GetOrCreatePlayerInternal(serverStatistic.Player.dataField.nickname, serverStatistic.Player.dataField.account_id, Utils.UnixDateToDateTime((long)serverStatistic.Player.dataField.created_at));

                T currentSnapshot = _dataProvider.QueryOver<T>().Where(x => x.PlayerId == playerEntity.Id)
                                               .OrderBy(x => x.Updated)
                                               .Desc.Take(1)
                                               .SingleOrDefault<T>() ?? new T { PlayerId = playerEntity.Id};

                //new battles
                if (currentSnapshot.BattlesCount < newSnapshot.BattlesCount)
                {
                    //create new record
                    if (IsNewSnapshotShouldBeAdded(currentSnapshot.Updated, newSnapshot.Updated))
                    {
                        currentSnapshot = new T { PlayerId = playerEntity.Id };
                    }

                    newSnapshot.Update(currentSnapshot);
                }

                if (serverStatistic != null && serverStatistic.Ratings != null)
                {
                    currentSnapshot.UpdateRatings(serverStatistic.Ratings);
                }

                _dataProvider.Save(currentSnapshot);
                _dataProvider.CommitTransaction();
            }
            catch (Exception e)
            {
                Log.Error(e);
                _dataProvider.RollbackTransaction();
            }
            finally
            {
                _dataProvider.ClearCache();
                _dataProvider.CloseSession();
            }

            return playerEntity;
        }

        private static bool IsNewSnapshotShouldBeAdded(DateTime currentSnapshotUpdated, DateTime newSnapshotUpdated)
        {
            if (currentSnapshotUpdated == DateTime.MinValue)
            {
                return true;
            }

            newSnapshotUpdated = newSnapshotUpdated.AddHours(-AppConfigSettings.SliceTime);
            currentSnapshotUpdated = currentSnapshotUpdated.AddHours(-AppConfigSettings.SliceTime);
            return newSnapshotUpdated.Date != currentSnapshotUpdated.Date;
        }

        /// <summary>
        /// Get the or create player.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="id">The identifier.</param>
        /// <param name="creaded">Creaded at.</param>
        /// <returns></returns>
        public PlayerEntity GetOrCreatePlayer(string name, int id, DateTime creaded)
        {
            _dataProvider.OpenSession();
            _dataProvider.BeginTransaction();
            PlayerEntity playerEntity = _dataProvider.QueryOver<PlayerEntity>()
                                        .Where(x => x.PlayerId == id)
                                        .Take(1)
                                        .SingleOrDefault<PlayerEntity>();

            if (playerEntity == null)
            {
                playerEntity = new PlayerEntity();
                playerEntity.Name = name;
                playerEntity.PlayerId = id;
                playerEntity.Creaded = creaded;

                _dataProvider.Save(playerEntity);
            }
            else
            {
                //user change name
                if (!Equals(playerEntity.Name, name))
                {
                    playerEntity.Name = name;
                }
            }
            
            _dataProvider.CommitTransaction();

            _dataProvider.CloseSession();
            return playerEntity;
        }

        /// <summary>
        /// Get the or create player.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="id">The identifier.</param>
        /// <param name="creaded">Creaded at.</param>
        /// <returns></returns>
        private PlayerEntity GetOrCreatePlayerInternal(string name, int id, DateTime creaded)
        {
            PlayerEntity playerEntity = new PlayerEntity();
            playerEntity.Name = name;
            playerEntity.PlayerId = id;
            playerEntity.Creaded = creaded;
            _dataProvider.Save(playerEntity);
            
            return playerEntity;
        }

        private PlayerEntity GetPlayerInternal(int id)
        {
            return _dataProvider.QueryOver<PlayerEntity>()
                .Where(x => x.PlayerId == id)
                .Take(1)
                .SingleOrDefault<PlayerEntity>();
        }

        /// <summary>
        /// Gets the player.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public PlayerEntity GetPlayer(int id)
        {
            _dataProvider.OpenSession();
            PlayerEntity player = GetPlayerInternal(id);
            _dataProvider.ClearCache();
            _dataProvider.CloseSession();
            return player;
        }

        /// <summary>
        /// Updates the tank statistic.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="playerId">The player identifier.</param>
        /// <param name="tanks">The tanks.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public PlayerEntity UpdateTankStatistic<T>(int playerId, List<TankJson> tanks, Func<TankJson, StatisticJson> predicate) where T : TankStatisticEntityBase, new ()
        {
            _dataProvider.OpenSession();
            _dataProvider.BeginTransaction();

            PlayerEntity playerEntity = GetPlayerInternal(playerId);

            try
            {
                IList<TankEntity> tankEntities = _dataProvider.QueryOver<TankEntity>().Where(x => x.PlayerId == playerEntity.Id).List<TankEntity>();

                DateTime updated = tanks.Max(x => x.Common.lastBattleTimeR);

                foreach (TankJson tank in tanks)
                {
                    int tankId = tank.Common.tankid;
                    int countryId = tank.Common.countryid;

                    TankEntity tankEntity = tankEntities.SingleOrDefault(x => x.TankId == tankId && x.CountryId == countryId);
                    if (tankEntity == null)
                    {
                        tankEntity = new TankEntity();
                        tankEntity.CountryId = countryId;
                        tankEntity.TankId = tankId;
                        tankEntity.Icon = tank.Description.Icon.IconId;
                        tankEntity.PlayerId = playerEntity.Id;
                        tankEntity.IsPremium = tank.Common.premium == 1;
                        tankEntity.Name = tank.Common.tanktitle;
                        tankEntity.TankType = tank.Common.type;
                        tankEntity.Tier = tank.Common.tier;
                        _dataProvider.Save(tankEntity);

                        T statisticEntity = new T();
                        statisticEntity.TankIdObject = tankEntity;
                        Update(statisticEntity, tank, predicate);
                        _dataProvider.Save(statisticEntity);
                    }
                    else
                    {
                        TankEntity tankAlias = null;
                        T statisticEntity = _dataProvider.QueryOver<T>()
                            .JoinAlias(x => x.TankIdObject, () => tankAlias)
                            .Where(x =>
                                tankAlias.PlayerId == playerEntity.Id
                                && tankAlias.TankId == tankId
                                && tankAlias.CountryId == countryId)
                            .OrderBy(x => x.Updated).Desc.Take(1).SingleOrDefault<T>();

                        int currentSnapshotBattlesCount = 0;

                        if (statisticEntity != null)
                        {
                            TankJson currentSnapshot = CompressHelper.DecompressObject<TankJson>(statisticEntity.Raw);
                            currentSnapshotBattlesCount = predicate(currentSnapshot).battlesCount;
                        }
                        else
                        {
                            statisticEntity = new T();
                            statisticEntity.TankIdObject = tankEntity;
                        }

                        if (currentSnapshotBattlesCount < predicate(tank).battlesCount)
                        {
                            //create new record
                            if (IsNewSnapshotShouldBeAdded(statisticEntity.Updated, updated))
                            {
                                statisticEntity = new T();
                                statisticEntity.TankIdObject = tankEntity;
                            }

                            statisticEntity.Updated = updated;
                            Update(statisticEntity, tank, predicate);
                            _dataProvider.Save(statisticEntity);
                        }
                    }
                }

                _dataProvider.CommitTransaction();

                return playerEntity;
            }
            catch (Exception e)
            {
                Log.Error(e);
                _dataProvider.RollbackTransaction();
            }
            finally
            {
                _dataProvider.ClearCache();
                _dataProvider.CloseSession();
            }

            return playerEntity;
        }

        private void Update(TankStatisticEntityBase statisticEntity, TankJson tank, Func<TankJson, StatisticJson> predicate)
        {
            statisticEntity.Updated = tank.Common.lastBattleTimeR;
            statisticEntity.Version = tank.Common.basedonversion;
            statisticEntity.Raw = tank.Raw;
            statisticEntity.BattlesCount = predicate(tank).battlesCount;
        }

        /// <summary>
        /// Gets the tanks statistic.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="playerId">The player identifier.</param>
        /// <returns></returns>
        public IEnumerable<T> GetTanksStatistic<T>(int playerId) where T : TankStatisticEntityBase
        {
            _dataProvider.OpenSession();
            TankEntity tankAlias = null;
            IList<T> tankStatisticEntities = _dataProvider.QueryOver<T>()
                .JoinAlias(x => x.TankIdObject, () => tankAlias).Where(x => tankAlias.PlayerId == playerId).List<T>();
            _dataProvider.CloseSession();
            return tankStatisticEntities;
        }

        /// <summary>
        /// Gets the replays.
        /// </summary>
        /// <returns></returns>
        public IList<ReplayEntity> GetReplays()
        {
            _dataProvider.OpenSession();
            IList<ReplayEntity> replays = _dataProvider.QueryOver<ReplayEntity>().List<ReplayEntity>();
            _dataProvider.CloseSession();
            return replays;
        }

        /// <summary>
        /// Saves the replay.
        /// </summary>
        /// <param name="playerId">The player identifier.</param>
        /// <param name="replayId">The replay identifier.</param>
        /// <param name="link">The link.</param>
        public void SaveReplay(long playerId, long replayId, string link)
        {
            SaveReplay(playerId, replayId, link, null);
        }

        /// <summary>
        /// Sets the favorite.
        /// </summary>
        /// <param name="tankId">The tank identifier.</param>
        /// <param name="countryId">The country identifier.</param>
        /// <param name="playerId">The player identifier.</param>
        /// <param name="favorite">if set to <c>true</c> [favorite].</param>
        public void SetFavorite(int tankId, int countryId, int playerId, bool favorite)
        {
            _dataProvider.OpenSession();
            _dataProvider.BeginTransaction();
            try
            {
                PlayerEntity playerAlias = null;
                TankEntity entity = _dataProvider.QueryOver<TankEntity>().Where(x => x.TankId == tankId && x.CountryId == countryId)
                    .JoinAlias(x => x.PlayerIdObject, () => playerAlias).Where(x => playerAlias.PlayerId == playerId).SingleOrDefault<TankEntity>();

                if (entity != null)
                {
                    entity.IsFavorite = favorite;
                    _dataProvider.Save(entity);
                }

                _dataProvider.CommitTransaction();
            }
            catch (Exception e)
            {
                Log.Error(e);
                _dataProvider.RollbackTransaction();
            }
            finally
            {
                _dataProvider.CloseSession();
            }
        }

        public void SaveReplay(long playerId, long replayId, string link, Replay replayData)
        {
            _dataProvider.OpenSession();
            _dataProvider.BeginTransaction();
            try
            {
                ReplayEntity entity = _dataProvider.QueryOver<ReplayEntity>().Where(x => x.PlayerId == playerId && x.ReplayId == replayId)
                    .SingleOrDefault<ReplayEntity>();

                ReplayEntity replayEntity = entity ?? new ReplayEntity();

                replayEntity.PlayerId = playerId;
                replayEntity.ReplayId = replayId;
                replayEntity.Link = link;
                if (replayData != null)
                {
                    replayEntity.Raw = CompressHelper.CompressObject(replayData);
                }

                _dataProvider.Save(replayEntity);

                _dataProvider.CommitTransaction();
            }
            catch (Exception e)
            {
                Log.Error(e);
                _dataProvider.RollbackTransaction();
            }
            finally
            {
                _dataProvider.CloseSession();
            }
        }

        public void DeleteReplay(long replayId)
        {
            _dataProvider.OpenSession();
            _dataProvider.BeginTransaction();
            try
            {
                ReplayEntity entity = _dataProvider.QueryOver<ReplayEntity>().Where(x => x.ReplayId == replayId)
                    .SingleOrDefault<ReplayEntity>();
                
                _dataProvider.Delete(entity);

                _dataProvider.CommitTransaction();
            }
            catch (Exception e)
            {
                Log.Error(e);
                _dataProvider.RollbackTransaction();
            }
            finally
            {
                _dataProvider.CloseSession();
            }
        }
    }
}
