using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Logging;
using Newtonsoft.Json;
using WotDossier.Common.Extensions;
using WotDossier.Dal;
using WotDossier.Domain;
using WotDossier.Domain.Entities;

namespace WotDossier.Applications
{
    public class SyncManager
    {
        protected static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly DossierRepository _repository;


        public SyncManager(DossierRepository repository)
        {
            _repository = repository;
        }

        public void Sync()
        {
            AppSettings settings = SettingsReader.Get();

            if (settings.PlayerId > 0)
            {
                new Uri(string.Format("http://localhost:8080/api/{0}/player/{1}", settings.Server, settings.PlayerId)).Delete();

                try
                {
                    int rev = GetServerDataVersion(settings.PlayerId, settings.Server);
                    PlayerEntity playerEntity = _repository.GetPlayer(settings.PlayerId);

                    if (rev > playerEntity.Rev)
                    {
                        UpdateLocalStatistic(playerEntity.Rev);
                    }
                    else if(rev < playerEntity.Rev)
                    {
                        UpdateServerStatistic(playerEntity, rev);
                    }
                }
                catch (Exception e)
                {
                    Log.Error("Error on get server statistic revision", e);
                }
            }
        }

        private void UpdateServerStatistic(PlayerEntity player, int rev)
        {
            AppSettings settings = SettingsReader.Get();
            IList<TankEntity> tanks = _repository.GetTanks(player, rev);
            
            //_repository.GetTanksStatistic<>(player, rev);
            var data = new
            {
                Player = player, 
                Tanks = tanks,
                RandomStatistic = _repository.GetPlayerStatistic<PlayerStatisticEntity>(player.PlayerId, rev),
                //HistoricalStatistic = _repository.GetPlayerStatistic<HistoricalBattlesStatisticEntity>(player.PlayerId, rev),
                //TeamStatistic = _repository.GetPlayerStatistic<TeamBattlesStatisticEntity>(player.PlayerId, rev)
            };
            new Uri(string.Format("http://localhost:8080/api/statistic/{0}", rev)).Post(JsonConvert.SerializeObject(data));
        }

        private void UpdateLocalStatistic(int rev)
        {
            AppSettings settings = SettingsReader.Get();
            string statistic = new Uri(string.Format("http://localhost:8080/api/statistic/{0}", rev)).Get();
        }

        private int GetServerDataVersion(int playerId, string server)
        {
            string player = new Uri(string.Format("http://localhost:8080/api/{0}/player/{1}", server, playerId)).Get();
            var list = JsonConvert.DeserializeObject<List<PlayerEntity>>(player);
            if (list.Count > 0)
            {
                return list.First().Rev;
            }
            return 0;
        }
    }
}
