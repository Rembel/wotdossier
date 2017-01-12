using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using Common.Logging;
using NHibernate.Criterion;
using WotDossier.Common;
using WotDossier.Dal.NHibernate;
using WotDossier.Domain.Entities;
using WotDossier.Domain.Replay;
using WotDossier.Domain.Server;
using WotDossier.Domain.Tank;
using System.Linq;
using Newtonsoft.Json;
using Player = WotDossier.Domain.Server.Player;

namespace WotDossier.Dal
{
    /// <summary>
    /// 
    /// </summary>
    [Export]
    public class DossierRepository
    {
        protected static readonly ILog Log = LogManager.GetLogger<DossierRepository>();

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
        /// <param name="accountId">The unique wot identifier.</param>
        /// <returns></returns>
        public IEnumerable<T> GetPlayerStatistic<T>(int accountId, int rev = 0) where T : StatisticEntity
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
                                    .Where(x => player.AccountId == accountId && statistic.Rev > rev).List<T>();
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
        /// <param name="accountId">The unique wot identifier.</param>
        /// <returns></returns>
        public PlayerEntity UpdatePlayerStatistic<T>(IStatisticAdapter<T> newSnapshot, Player serverStatistic, int accountId) where T : StatisticEntity, new()
        {
            _dataProvider.OpenSession();
            _dataProvider.BeginTransaction();
            PlayerEntity playerEntity = null;

            try
            {
                playerEntity = GetPlayerByAccountId(accountId) ??
                               //recreate payer record in case db was deleted but exists user configured in application setting 
                               CreatePlayer(serverStatistic.dataField.nickname, serverStatistic.dataField.account_id,
                               Utils.UnixDateToDateTime((long)serverStatistic.dataField.created_at), serverStatistic.server);

                T currentSnapshot = _dataProvider.QueryOver<T>().Where(x => x.PlayerId == playerEntity.Id)
                                               .OrderBy(x => x.Updated)
                                               .Desc.Take(1)
                                               .SingleOrDefault<T>() ?? new T { PlayerId = playerEntity.Id };

                //new battles
                if (currentSnapshot.BattlesCount < newSnapshot.BattlesCount)
                {
                    //create new record
                    if (IsNewSnapshotShouldBeAdded(currentSnapshot.Updated, newSnapshot.Updated))
                    {
                        currentSnapshot = new T { PlayerId = playerEntity.Id, PlayerUId = playerEntity.UId, UId = Guid.NewGuid() };
                    }

                    newSnapshot.Update(currentSnapshot);
                }

                RevisionProvider.SetParentContext(playerEntity);

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

        private static bool IsNewSnapshotShouldBeAdded(TankStatisticEntityBase statisticEntity, Func<TankJson, StatisticJson> predicate, TankJson dossierTank, DateTime lastBattleTime)
        {
            TankJson currentSnapshot = CompressHelper.DecompressObject<TankJson>(statisticEntity.Raw);
            var currentSnapshotBattlesCount = predicate(currentSnapshot).battlesCount;

            if (currentSnapshotBattlesCount < predicate(dossierTank).battlesCount)
            {
                var dbStatisticUpdateTime = statisticEntity.Updated;
                if (currentSnapshotBattlesCount == 0)
                {
                    return true;
                }

                lastBattleTime = lastBattleTime.AddHours(-AppConfigSettings.SliceTime);
                dbStatisticUpdateTime = dbStatisticUpdateTime.AddHours(-AppConfigSettings.SliceTime);
                return lastBattleTime.Date != dbStatisticUpdateTime.Date;
            }
            return false;
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
        /// <param name="accountId">The unique wot identifier.</param>
        /// <param name="creaded">Creaded at.</param>
        /// <param name="server">The server.</param>
        /// <returns></returns>
        public PlayerEntity GetOrCreatePlayer(string name, int accountId, DateTime creaded, string server)
        {
            _dataProvider.OpenSession();
            _dataProvider.BeginTransaction();
            PlayerEntity playerEntity = _dataProvider.QueryOver<PlayerEntity>()
                                        .Where(x => x.AccountId == accountId)
                                        .Take(1)
                                        .SingleOrDefault<PlayerEntity>();

            if (playerEntity == null)
            {
                playerEntity = new PlayerEntity();
                playerEntity.AccountId = accountId;
                playerEntity.Creaded = creaded;
            }

            playerEntity.Name = name;
            playerEntity.Server = server;
            _dataProvider.Save(playerEntity);

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
        /// <param name="server">The server.</param>
        /// <returns></returns>
        private PlayerEntity CreatePlayer(string name, int id, DateTime creaded, string server)
        {
            PlayerEntity playerEntity = new PlayerEntity();
            playerEntity.Name = name;
            playerEntity.AccountId = id;
            playerEntity.Creaded = creaded;
            playerEntity.Server = server;
            _dataProvider.Save(playerEntity);

            return playerEntity;
        }

        private PlayerEntity GetPlayerByAccountId(int accountId)
        {
            return _dataProvider.QueryOver<PlayerEntity>()
                .Where(x => x.AccountId == accountId)
                .Take(1)
                .SingleOrDefault<PlayerEntity>();
        }

        public PlayerEntity GetPlayerById(int id)
        {
            _dataProvider.OpenSession();
            var playerEntity = _dataProvider.QueryOver<PlayerEntity>()
                .Where(x => x.Id == id)
                .Take(1)
                .SingleOrDefault<PlayerEntity>();
            _dataProvider.ClearCache();
            _dataProvider.CloseSession();
            return playerEntity;
        }

        /// <summary>
        /// Gets the player.
        /// </summary>
        /// <param name="accountId">The unique wot identifier.</param>
        /// <returns></returns>
        public PlayerEntity GetPlayer(int accountId)
        {
            _dataProvider.OpenSession();
            PlayerEntity player = GetPlayerByAccountId(accountId);
            _dataProvider.ClearCache();
            _dataProvider.CloseSession();
            return player;
        }

        /// <summary>
        /// Updates the tank statistic.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="accountId">The unique wot identifier.</param>
        /// <param name="tanks">The tanks.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public PlayerEntity UpdateTankStatistic<T>(int accountId, List<TankJson> tanks, Func<TankJson, StatisticJson> predicate) where T : TankStatisticEntityBase, new()
        {
            _dataProvider.OpenSession();
            _dataProvider.BeginTransaction();

            PlayerEntity playerEntity = GetPlayerByAccountId(accountId);

            try
            {
                IList<TankEntity> tankEntities = _dataProvider.QueryOver<TankEntity>().Where(x => x.PlayerId == playerEntity.Id).List<TankEntity>();

                DateTime lastBattleTime = tanks.Max(x => x.Common.lastBattleTimeR);

                foreach (TankJson dossierTank in tanks)
                {
                    T statisticEntity = null;

                    TankEntity tankEntity = tankEntities.SingleOrDefault(x => x.TankId == dossierTank.Common.tankid && x.CountryId == dossierTank.Common.countryid);
                    var tankNotExists = tankEntity == null;
                    if (tankNotExists)
                    {
                        tankEntity = CreateTankEntity(dossierTank, playerEntity);
                        _dataProvider.Save(tankEntity);
                    }
                    else
                    {
                        TankEntity tankAlias = null;
                        statisticEntity = _dataProvider.QueryOver<T>()
                            .JoinAlias(x => x.TankIdObject, () => tankAlias)
                            .Where(x =>
                                tankAlias.PlayerId == playerEntity.Id
                                && tankAlias.TankId == dossierTank.Common.tankid && tankAlias.CountryId == dossierTank.Common.countryid)
                            .OrderBy(x => x.Updated).Desc.Take(1).SingleOrDefault<T>();
                    }

                    if (tankNotExists || statisticEntity == null || IsNewSnapshotShouldBeAdded(statisticEntity, predicate, dossierTank, lastBattleTime))
                    {
                        statisticEntity = new T
                        {
                            TankIdObject = tankEntity,
                            TankUId = tankEntity.UId
                        };
                    }

                    Update(statisticEntity, dossierTank, predicate);
                    _dataProvider.Save(statisticEntity);
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

        private TankEntity CreateTankEntity(TankJson dossierTank, PlayerEntity playerEntity)
        {
            TankEntity tankEntity = new TankEntity
            {
                CountryId = dossierTank.Common.countryid,
                TankId = dossierTank.Common.tankid,
                Icon = dossierTank.Description.Icon.IconId,
                PlayerId = playerEntity.Id,
                PlayerUId = playerEntity.UId,
                IsPremium = dossierTank.Common.premium == 1,
                Name = dossierTank.Common.tanktitle,
                TankType = dossierTank.Common.type,
                Tier = dossierTank.Common.tier,
                UniqueId = dossierTank.UniqueId()
            };
            return tankEntity;
        }

        private void Update(TankStatisticEntityBase statisticEntity, TankJson tank, Func<TankJson, StatisticJson> predicate)
        {
            statisticEntity.Updated = tank.Common.creationTimeR > tank.Common.lastBattleTimeR ? tank.Common.creationTimeR : tank.Common.lastBattleTimeR;
            statisticEntity.Version = tank.Common.basedonversion;
            statisticEntity.Raw = tank.Raw;
            statisticEntity.BattlesCount = predicate(tank).battlesCount;
        }

        /// <summary>
        /// Gets the tanks statistic.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="playerId">The player identifier.</param>
        /// <param name="rev"></param>
        /// <returns></returns>
        public IEnumerable<T> GetTanksStatistic<T>(int playerId, int rev = 0) where T : TankStatisticEntityBase
        {
            _dataProvider.OpenSession();
            TankEntity tankAlias = null;
            IList<T> tankStatisticEntities = _dataProvider.QueryOver<T>()
                .JoinAlias(x => x.TankIdObject, () => tankAlias).Where(x => tankAlias.PlayerId == playerId && x.Rev > rev).List<T>();
            _dataProvider.CloseSession();
            return tankStatisticEntities;
        }

        /// <summary>
        /// Gets the tanks statistic.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="playerId">The player identifier.</param>
        /// <param name="tanksUniqueIds"></param>
        /// <returns></returns>
        public IEnumerable<T> GetTanksStatistic<T>(int playerId, List<int> tanksUniqueIds) where T : TankStatisticEntityBase
        {
            _dataProvider.OpenSession();
            TankEntity tankAlias = null;
            IList<T> tankStatisticEntities = _dataProvider.QueryOver<T>()
                .JoinAlias(x => x.TankIdObject, () => tankAlias).Where(x => tankAlias.PlayerId == playerId && tankAlias.UniqueId.IsIn(tanksUniqueIds)).List<T>();
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
                    .JoinAlias(x => x.PlayerIdObject, () => playerAlias).Where(x => playerAlias.AccountId == playerId).SingleOrDefault<TankEntity>();

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
                var replayEntities = _dataProvider.QueryOver<ReplayEntity>().Where(x => x.ReplayId == replayId).List<ReplayEntity>();

                foreach (var entity in replayEntities)
                {
                    _dataProvider.Delete(entity);
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

        public IList<PlayerEntity> GetPlayers()
        {
            _dataProvider.OpenSession();
            try
            {
                return _dataProvider.QueryOver<PlayerEntity>().Where(x => x.Server != null).List();
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
            finally
            {
                _dataProvider.CloseSession();
            }
            return new List<PlayerEntity>();
        }

        public List<FavoritePlayerEntity> GetFavoritePlayers()
        {
            string path = Path.Combine(Folder.GetDossierAppDataFolder(), "favorite_players");
            if (File.Exists(path))
            {
                var content = File.ReadAllText(path);
                try
                {
                    return JsonConvert.DeserializeObject<List<FavoritePlayerEntity>>(content);
                }
                catch (Exception e)
                {
                    Log.Error("Error on favorite players loading", e);
                }
            }
            return new List<FavoritePlayerEntity>();
        }

        public void SetFavoritePlayers(List<FavoritePlayerEntity> players)
        {
            string path = Path.Combine(Folder.GetDossierAppDataFolder(), "favorite_players");
            try
            {
                File.WriteAllText(path, JsonConvert.SerializeObject(players));
            }
            catch (Exception e)
            {
                Log.Error("Error on favorite players save", e);
            }
        }

        public void DeletePlayerData(int playerId)
        {
            _dataProvider.OpenSession();

            try
            {
                var query = DataProvider.CreateQuery("delete from TankRandomBattlesStatisticEntity where TankId in (select Id from TankEntity where PlayerId = :id)");
                query.SetParameter("id", playerId);
                query.ExecuteUpdate();

                query = DataProvider.CreateQuery("delete from RandomBattlesStatisticEntity where PlayerId = :id");
                query.SetParameter("id", playerId);
                query.ExecuteUpdate();

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

        public IList<TankEntity> GetTanks(PlayerEntity player, int rev)
        {
            _dataProvider.OpenSession();
            IList<TankEntity> tanks = _dataProvider.QueryOver<TankEntity>().Where(x => x.Rev > rev && x.PlayerId == player.Id).List();
            _dataProvider.CloseSession();
            return tanks;
        }

        public DbVersionEntity GetCurrentDbVersion()
        {
            _dataProvider.OpenSession();
            DbVersionEntity entity = _dataProvider.QueryOver<DbVersionEntity>().Take(1).List().First();
            _dataProvider.CloseSession();
            return entity;
        }
    }
}
