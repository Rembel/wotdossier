using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Common.Logging;
using WotDossier.Dal.NHibernate;
using WotDossier.Domain.Entities;
using WotDossier.Domain.Server;
using WotDossier.Domain.Tank;
using System.Linq;

namespace WotDossier.Dal
{
    [Export]
    public class DossierRepository
    {
        protected static readonly ILog Log = LogManager.GetLogger("DossierRepository");

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

        public IEnumerable<T> GetStatistic<T>(int playerId) where T : StatisticEntity
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

        public PlayerEntity UpdateStatistic<T>(IStatisticAdapter<T> newSnapshot, Ratings ratings, int playerId) where T : StatisticEntity, new()
        {
            _dataProvider.OpenSession();
            _dataProvider.BeginTransaction();
            PlayerEntity playerEntity = null;

            try
            {
                playerEntity = GetPlayerInternal(playerId);

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

                if (ratings != null)
                {
                    currentSnapshot.UpdateRatings(ratings);
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
                _dataProvider.CommitTransaction();
            }
            _dataProvider.CloseSession();
            return playerEntity;
        }

        private PlayerEntity GetPlayerInternal(int id)
        {
            return _dataProvider.QueryOver<PlayerEntity>()
                .Where(x => x.PlayerId == id)
                .Take(1)
                .SingleOrDefault<PlayerEntity>();
        }

        public PlayerEntity GetPlayer(int id)
        {
            _dataProvider.OpenSession();
            PlayerEntity player = GetPlayerInternal(id);
            _dataProvider.ClearCache();
            _dataProvider.CloseSession();
            return player;
        }

        public PlayerEntity UpdateTankStatistic(int playerId, List<TankJson> tanks)
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
                        tankEntity.CountryCode = WotApiHelper.GetCountryNameCode(countryId);
                        tankEntity.TankId = tankId;
                        tankEntity.Icon = tank.Description.Icon.IconId;
                        tankEntity.PlayerId = playerEntity.Id;
                        tankEntity.IsPremium = tank.Common.premium == 1;
                        tankEntity.Name = tank.Common.tanktitle;
                        tankEntity.TankType = tank.Common.type;
                        tankEntity.Tier = tank.Common.tier;
                        TankStatisticEntity statisticEntity = new TankStatisticEntity();
                        statisticEntity.TankIdObject = tankEntity;
                        Update(statisticEntity, tank);
                        tankEntity.TankStatisticEntities.Add(statisticEntity);
                        _dataProvider.Save(tankEntity);
                    }
                    else
                    {
                        TankEntity tankAlias = null;
                        TankStatisticEntity statisticEntity = _dataProvider.QueryOver<TankStatisticEntity>()
                            .JoinAlias( x => x.TankIdObject, () => tankAlias)
                            .Where(x => 
                                tankAlias.PlayerId == playerEntity.Id
                                && tankAlias.TankId == tankId
                                && tankAlias.CountryId == countryId)
                            .OrderBy(x => x.Updated).Desc.Take(1).SingleOrDefault<TankStatisticEntity>();

                        int currentSnapshotBattlesCount = 0;

                        if (statisticEntity != null)
                        {
                            TankJson currentSnapshot = WotApiHelper.UnZipObject<TankJson>(statisticEntity.Raw);
                            currentSnapshotBattlesCount = currentSnapshot.A15x15.battlesCount;
                        }
                        else
                        {
                            statisticEntity = new TankStatisticEntity();
                            statisticEntity.TankIdObject = tankEntity;
                        }

                        if (currentSnapshotBattlesCount < tank.A15x15.battlesCount || (currentSnapshotBattlesCount == tank.A15x15.battlesCount && tank.Common.basedonversion == 26))
                        {
                            //create new record
                            if (IsNewSnapshotShouldBeAdded(statisticEntity.Updated,  updated))
                            {
                                statisticEntity = new TankStatisticEntity();
                                statisticEntity.TankIdObject = tankEntity;
                            }
                            
                            statisticEntity.Updated = updated;
                            Update(statisticEntity, tank);
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

        private void Update(TankStatisticEntity statisticEntity, TankJson tank)
        {
            statisticEntity.Updated = tank.Common.lastBattleTimeR;
            statisticEntity.Version = tank.Common.basedonversion;
            statisticEntity.Raw = tank.Raw;
        }

        public IEnumerable<TankStatisticEntity> GetTanksStatistic(int playerId)
        {
            _dataProvider.OpenSession();
            TankEntity tankAlias = null;
            IList<TankStatisticEntity> tankStatisticEntities = _dataProvider.QueryOver<TankStatisticEntity>()
                .JoinAlias(x => x.TankIdObject, () => tankAlias).Where(x => tankAlias.PlayerId == playerId).List<TankStatisticEntity>();
            _dataProvider.CloseSession();
            return tankStatisticEntities;
        }

        public IList<ReplayEntity> GetReplays()
        {
            _dataProvider.OpenSession();
            IList<ReplayEntity> replays = _dataProvider.QueryOver<ReplayEntity>().List<ReplayEntity>();
            _dataProvider.CloseSession();
            return replays;
        }

        public void SaveReplay(long playerId, long replayId, string link)
        {
            _dataProvider.OpenSession();
            _dataProvider.BeginTransaction();
            try
            {
                ReplayEntity entity = _dataProvider.QueryOver<ReplayEntity>().Where(x => x.PlayerId == playerId && x.ReplayId == replayId).SingleOrDefault<ReplayEntity>();

                ReplayEntity replayEntity = entity ?? new ReplayEntity();

                replayEntity.PlayerId = playerId;
                replayEntity.ReplayId = replayId;
                replayEntity.Link = link;

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
    }
}
