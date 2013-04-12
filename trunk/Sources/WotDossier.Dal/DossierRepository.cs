using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Common.Logging;
using WotDossier.Common;
using WotDossier.Dal.NHibernate;
using WotDossier.Domain;
using WotDossier.Domain.Entities;
using WotDossier.Domain.Player;
using WotDossier.Domain.Tank;
using System.Linq;

namespace WotDossier.Dal
{
    [Export]
    public class DossierRepository
    {
        protected static readonly ILog _log = LogManager.GetLogger("DossierRepository");

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

        public IEnumerable<PlayerStatisticEntity> GetPlayerStatistic(string playerName)
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
                                    .Where(x => player.Name == playerName).List<PlayerStatisticEntity>();
                _dataProvider.CommitTransaction();
            }
            catch (Exception e)
            {
                _log.Error(e);
                _dataProvider.RollbackTransaction();
            }
            finally
            {
                _dataProvider.CloseSession();    
            }
            return list;
        }

        public PlayerEntity UpdatePlayerStatistic(string playerId, PlayerStat stat)
        {
            _dataProvider.OpenSession();
            _dataProvider.BeginTransaction();
            PlayerEntity playerEntity = null;
            try
            {
                string name = stat.data.name;
                int id = stat.data.id;
                DateTime creaded = Utils.UnixDateToDateTime((long)stat.data.created_at);

                playerEntity = GetOrCreatePlayer(playerId, name, id, creaded);

                PlayerStatisticEntity statisticEntity = _dataProvider.QueryOver<PlayerStatisticEntity>().Where(x => x.PlayerId == playerEntity.Id)
                                               .OrderBy(x => x.Updated)
                                               .Desc.Take(1)
                                               .SingleOrDefault<PlayerStatisticEntity>();
                DateTime updated = Utils.UnixDateToDateTime((long) stat.data.updated_at).Date;
                //create new record
                if (statisticEntity == null ||
                    (statisticEntity.Updated.Date != updated.Date && statisticEntity.BattlesCount < stat.data.summary.Battles_count))
                {
                    statisticEntity = new PlayerStatisticEntity();
                    statisticEntity.PlayerId = playerEntity.Id;
                    statisticEntity.Update(stat);
                    _dataProvider.Save(statisticEntity);
                }
                //update current date record
                else if (statisticEntity.Updated.Date == updated.Date)
                {
                    statisticEntity.Update(stat);
                    _dataProvider.Save(statisticEntity);
                }

                _dataProvider.CommitTransaction();
            }
            catch (Exception e)
            {
                _log.Error(e);
                _dataProvider.RollbackTransaction();
            }
            finally
            {
                _dataProvider.ClearCache();
                _dataProvider.CloseSession();
            }

            return playerEntity;
        }

        public PlayerEntity GetOrCreatePlayer(string playerId, string name, int id, DateTime creaded)
        {
            PlayerEntity playerEntity = _dataProvider.QueryOver<PlayerEntity>()
                                        .Where(x => x.Name == playerId)
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
            return playerEntity;
        }

        public PlayerEntity UpdateTankStatistic(string playerName, List<TankJson> tanks)
        {
            _dataProvider.OpenSession();
            _dataProvider.BeginTransaction();

            PlayerSearchJson player;
            try
            {
                player = WotApiClient.Instance.SearchPlayer(new AppSettings { PlayerId = playerName, Server = "ru" }) ?? 
                         WotApiClient.Instance.SearchPlayer(new AppSettings { PlayerId = playerName, Server = "eu" });
            }
            catch (Exception e)
            {
                _log.Error("Can't get player info from server", e);
                return null;
            }

            string name = player.name;
            int id = player.id;
            DateTime creaded = Utils.UnixDateToDateTime((long)player.created_at);
            PlayerEntity playerEntity = GetOrCreatePlayer(playerName, name, id, creaded);

            try
            {
                IList<TankEntity> tankEntities = _dataProvider.QueryOver<TankEntity>().Where(x => x.PlayerId == playerEntity.Id).List<TankEntity>();

                foreach (TankJson tank in tanks)
                {
                    TankEntity tankEntity = tankEntities.SingleOrDefault(x => x.TankId == tank.Common.tankid && x.CountryId == tank.Common.countryid);
                    if (tankEntity == null)
                    {
                        tankEntity = new TankEntity();
                        tankEntity.CountryId = tank.Common.countryid;
                        tankEntity.CountryCode = WotApiHelper.GetCountryNameCode(tank.Common.countryid);
                        tankEntity.TankId = tank.Common.tankid;
                        tankEntity.Icon = tank.Icon.iconid;
                        tankEntity.PlayerId = playerEntity.Id;
                        tankEntity.IsPremium = tank.Common.premium == 1;
                        tankEntity.Name = tank.Common.tanktitle;
                        tankEntity.TankType = tank.Common.type;
                        tankEntity.Tier = tank.Common.tier;
                        TankStatisticEntity statisticEntity = new TankStatisticEntity();
                        statisticEntity.TankIdObject = tankEntity;
                        statisticEntity.Update(tank);
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
                                && tankAlias.TankId == tank.Common.tankid 
                                && tankAlias.CountryId == tank.Common.countryid)
                            .OrderBy(x => x.Updated).Desc.Take(1).SingleOrDefault<TankStatisticEntity>();
                        DateTime updated = tank.Common.lastBattleTimeR.Date;
                        //create new record
                        if (statisticEntity == null || statisticEntity.Updated < updated)
                        {
                            statisticEntity = new TankStatisticEntity();
                            statisticEntity.TankIdObject = tankEntity;
                            statisticEntity.Update(tank);
                            _dataProvider.Save(statisticEntity);
                        }
                        //update current date record
                        else if (statisticEntity.Updated.Date == updated)
                        {
                            statisticEntity.Update(tank);
                            _dataProvider.Save(statisticEntity);
                        }
                    }
                }

                _dataProvider.CommitTransaction();

                return playerEntity;
            }
            catch (Exception e)
            {
                _log.Error(e);
                _dataProvider.RollbackTransaction();
            }
            finally
            {
                _dataProvider.ClearCache();
                _dataProvider.CloseSession();
            }

            return playerEntity;
        }

        public IEnumerable<TankStatisticEntity> GetTanksStatistic(PlayerEntity player)
        {
            _dataProvider.OpenSession();
            TankEntity tankAlias = null;
            IList<TankStatisticEntity> tankStatisticEntities = _dataProvider.QueryOver<TankStatisticEntity>()
                .JoinAlias(x => x.TankIdObject, () => tankAlias).Where(x => tankAlias.PlayerId == player.Id).List<TankStatisticEntity>();
            _dataProvider.CloseSession();
            return tankStatisticEntities;
        }
    }
}
