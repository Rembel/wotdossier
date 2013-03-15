using System;
using System.Collections.Generic;
using WotDossier.Dal.NHibernate;
using WotDossier.Domain.Entities;

namespace WotDossier.Dal
{
    public class DossierRepository
    {
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
            }
            catch (Exception e)
            {
                _dataProvider.RollbackTransaction();
            }
            _dataProvider.CommitTransaction();
            _dataProvider.CloseSession();
            return list;
        }
    }
}
