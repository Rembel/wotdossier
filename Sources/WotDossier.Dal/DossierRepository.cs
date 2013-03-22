using System;
using System.Collections.Generic;
using Common.Logging;
using WotDossier.Common;
using WotDossier.Dal.NHibernate;
using WotDossier.Domain.Entities;
using WotDossier.Domain.Player;

namespace WotDossier.Dal
{
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
        public DossierRepository(DataProvider dataProvider)
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

        public PlayerEntity GetOrCreatePlayer(string playerId, PlayerStat stat)
        {
            _dataProvider.OpenSession();
            _dataProvider.BeginTransaction();
            PlayerEntity playerEntity = null;
            try
            {
                playerEntity =
                    _dataProvider.QueryOver<PlayerEntity>()
                                 .Where(x => x.Name == playerId)
                                 .Take(1)
                                 .SingleOrDefault<PlayerEntity>();

                PlayerStatisticEntity statisticEntity;
                if (playerEntity == null)
                {
                    statisticEntity = new PlayerStatisticEntity();
                    playerEntity = new PlayerEntity();
                    playerEntity.Name = stat.data.name;
                    playerEntity.PlayerId = stat.data.id;
                    playerEntity.Creaded = Utils.UnixDateToDateTime((long) stat.data.created_at);

                    _dataProvider.Save(playerEntity);

                    statisticEntity.PlayerId = playerEntity.Id;
                    statisticEntity.Update(stat);
                    _dataProvider.Save(statisticEntity);
                }
                else
                {
                    statisticEntity = _dataProvider.QueryOver<PlayerStatisticEntity>()
                                                   .OrderBy(x => x.Updated)
                                                   .Desc.Take(1)
                                                   .SingleOrDefault<PlayerStatisticEntity>();
                    DateTime updated = Utils.UnixDateToDateTime((long) stat.data.updated_at).Date;
                    if (statisticEntity == null ||
                        (statisticEntity.Updated.Date != updated.Date && statisticEntity.Updated < updated))
                    {
                        statisticEntity = new PlayerStatisticEntity();
                        statisticEntity.PlayerId = playerEntity.Id;
                        statisticEntity.Update(stat);
                        _dataProvider.Save(statisticEntity);
                    }
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
    }
}
