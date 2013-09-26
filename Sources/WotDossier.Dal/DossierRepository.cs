using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Common.Logging;
using WotDossier.Dal.NHibernate;
using WotDossier.Domain.Entities;
using WotDossier.Domain.Player;
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

        public IEnumerable<PlayerStatisticEntity> GetPlayerStatistic(int playerId)
        {
            _dataProvider.OpenSession();
            _dataProvider.BeginTransaction();
            PlayerEntity player = null;
            PlayerStatisticEntity statistic = null;
            IList<PlayerStatisticEntity> list = null;
            try
            {
                list = _dataProvider.QueryOver(() => statistic)
                                    .Inner.JoinAlias(x => x.PlayerIdObject, () => player)
                                    .Where(x => player.PlayerId == playerId).List<PlayerStatisticEntity>();
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

        public PlayerEntity UpdatePlayerStatistic(Ratings ratings, List<TankJson> tanks, int playerId)
        {
            _dataProvider.OpenSession();
            _dataProvider.BeginTransaction();
            PlayerEntity playerEntity = null;

            try
            {
                playerEntity = GetPlayer(playerId);

                PlayerStatisticEntity currentSnapshot = _dataProvider.QueryOver<PlayerStatisticEntity>().Where(x => x.PlayerId == playerEntity.Id)
                                               .OrderBy(x => x.Updated)
                                               .Desc.Take(1)
                                               .SingleOrDefault<PlayerStatisticEntity>() ?? new PlayerStatisticEntity{PlayerId = playerEntity.Id};

                PlayerStatAdapter newSnapshot = new PlayerStatAdapter(tanks);

                //new battles
                if (currentSnapshot.BattlesCount < newSnapshot.Battles_count)
                {
                    //create new record
                    if (IsNewSnapshotShouldBeAdded(currentSnapshot.Updated, newSnapshot.Updated))
                    {
                        currentSnapshot = new PlayerStatisticEntity {PlayerId = playerEntity.Id};
                    }

                    currentSnapshot.Update(newSnapshot);
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
            newSnapshotUpdated = newSnapshotUpdated.AddHours(WotDossierSettings.SliceTime);
            currentSnapshotUpdated = currentSnapshotUpdated.AddHours(WotDossierSettings.SliceTime);
            return newSnapshotUpdated.Date != currentSnapshotUpdated.Date;
        }

        public PlayerEntity GetOrCreatePlayer(string name, int id, DateTime creaded)
        {
            _dataProvider.OpenSession();
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
            _dataProvider.CloseSession();
            return playerEntity;
        }

        private PlayerEntity GetPlayer(int id)
        {
            return _dataProvider.QueryOver<PlayerEntity>()
                .Where(x => x.PlayerId == id)
                .Take(1)
                .SingleOrDefault<PlayerEntity>();
        }

        public PlayerEntity UpdateTankStatistic(int playerId, List<TankJson> tanks)
        {
            _dataProvider.OpenSession();
            _dataProvider.BeginTransaction();

            PlayerEntity playerEntity = GetPlayer(playerId);

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
                        tankEntity.Icon = tank.Icon.iconid;
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
                            currentSnapshotBattlesCount = currentSnapshot.Tankdata.battlesCount;
                        }
                        else
                        {
                            statisticEntity = new TankStatisticEntity();
                            statisticEntity.TankIdObject = tankEntity;
                        }

                        if (currentSnapshotBattlesCount < tank.Tankdata.battlesCount)
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
