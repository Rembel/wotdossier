using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Common.Logging;
using WotDossier.Common;
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
                _log.Error(e);
                _dataProvider.RollbackTransaction();
            }
            finally
            {
                _dataProvider.CloseSession();    
            }
            return list;
        }

        public PlayerEntity UpdatePlayerStatistic(PlayerStat stat, List<TankJson> tanks)
        {
            _dataProvider.OpenSession();
            _dataProvider.BeginTransaction();
            PlayerEntity playerEntity = null;

            try
            {
                playerEntity = GetOrCreatePlayer(stat);

                PlayerStatisticEntity statisticEntity = _dataProvider.QueryOver<PlayerStatisticEntity>().Where(x => x.PlayerId == playerEntity.Id)
                                               .OrderBy(x => x.Updated)
                                               .Desc.Take(1)
                                               .SingleOrDefault<PlayerStatisticEntity>();

                //init server stat adapter
                //PlayerStatAdapter serverStat = new PlayerStatAdapter(stat);
                //init local stat adapter
                PlayerStatAdapter localStat = new PlayerStatAdapter(tanks);

                //by default using server statistic
                PlayerStatAdapter playerStatAdapter = localStat;

                //but if in local statistic battle count more then in server 
                //if (localStat.Battles_count > serverStat.Battles_count)
                //{
                //    //use local
                //    playerStatAdapter = localStat;
                //}

                //create new record
                if (statisticEntity == null ||
                    (statisticEntity.Updated.Date != playerStatAdapter.Updated.Date && statisticEntity.BattlesCount < playerStatAdapter.Battles_count))
                {
                    statisticEntity = new PlayerStatisticEntity();
                    statisticEntity.PlayerId = playerEntity.Id;
                    statisticEntity.Update(playerStatAdapter);
                }
                //update current date record
                else if (statisticEntity.Updated.Date == playerStatAdapter.Updated.Date)
                {
                    statisticEntity.Update(playerStatAdapter);
                }

                statisticEntity.UpdateRatings(stat);
            
                _dataProvider.Save(statisticEntity);
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

        private PlayerEntity GetOrCreatePlayer(PlayerStat stat)
        {
            return GetOrCreatePlayer(stat.data.name, stat.data.id, Utils.UnixDateToDateTime((long)stat.data.created_at));
        }

        public PlayerEntity GetOrCreatePlayer(string name, int id, DateTime creaded)
        {
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
            return playerEntity;
        }

        public PlayerEntity UpdateTankStatistic(PlayerStat player, List<TankJson> tanks)
        {
            _dataProvider.OpenSession();
            _dataProvider.BeginTransaction();

            PlayerEntity playerEntity = GetOrCreatePlayer(player);

            try
            {
                IList<TankEntity> tankEntities = _dataProvider.QueryOver<TankEntity>().Where(x => x.PlayerId == playerEntity.Id).List<TankEntity>();

                DateTime updated = DateTime.Now;
                DateTime updatedDate = updated.Date;

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
                                && tankAlias.TankId == tank.Common.tankid 
                                && tankAlias.CountryId == tank.Common.countryid)
                            .OrderBy(x => x.Updated).Desc.Take(1).SingleOrDefault<TankStatisticEntity>();
                        
                        TankJson prevTank = null;
                        if (statisticEntity != null)
                        {
                            prevTank = WotApiHelper.UnZipObject<TankJson>(statisticEntity.Raw);
                        }

                        //create new record
                        if (statisticEntity == null || statisticEntity.Updated.Date != updatedDate && prevTank.Tankdata.battlesCount < tank.Tankdata.battlesCount)
                        {
                            statisticEntity = new TankStatisticEntity();
                            statisticEntity.TankIdObject = tankEntity;
                            statisticEntity.Updated = updated;
                            Update(statisticEntity, tank);
                            _dataProvider.Save(statisticEntity);
                        }
                        //update current date record
                        else if (statisticEntity.Updated.Date == updatedDate)
                        {
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

        public virtual void Update(TankStatisticEntity statisticEntity, TankJson tank)
        {
            statisticEntity.Updated = tank.Common.lastBattleTimeR;
            statisticEntity.Version = tank.Common.basedonversion;
            statisticEntity.Raw = tank.Raw;
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
