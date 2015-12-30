using System;
using System.Collections.Generic;
using System.Linq;
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

        public const string ApiBaseUrl = "http://localhost:5000/api/sync/";

        private readonly DossierRepository _repository;


        public SyncManager(DossierRepository repository)
        {
            _repository = repository;
        }

        public void Sync()
        {
            if (!SupportedDbVersion())
            {
                return;
            }

            AppSettings settings = SettingsReader.Get();

            if (settings.PlayerId > 0)
            {
                ApiMethod($"player/{settings.Server}/{settings.PlayerId}").Delete();

                try
                {
                    int rev = GetServerDataVersion(settings.PlayerId, settings.Server);
                    PlayerEntity playerEntity = _repository.GetPlayer(settings.PlayerId);

                    if (rev > playerEntity.Rev)
                    {
            //            UpdateLocalStatistic(playerEntity.Rev);
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

        private Uri ApiMethod(string method)
        {
            return new Uri(ApiBaseUrl + method);
        }

        private bool SupportedDbVersion()
        {
            try
            {
                var response = ApiMethod("dbversion").Get();
                var serverDbVersion = JsonConvert.DeserializeObject<DbVersionEntity>(response);
                var clientDbVersion = _repository.GetCurrentDbVersion();
                return serverDbVersion.SchemaVersion.Equals(clientDbVersion.SchemaVersion, StringComparison.Ordinal);
            }
            catch (Exception e)
            {
                Log.Error("Sync. Error on check SupportedDbVersion", e);
            }
            return false;
        }

        private void UpdateServerStatistic(PlayerEntity player, int rev)
        {
            var data = new ClientStat
            {
                Player = player, 
                Tanks = _repository.GetTanks(player, rev),
                RandomStatistic = _repository.GetPlayerStatistic<RandomBattlesStatisticEntity>(player.AccountId, rev),
                TankRandomStatistic = _repository.GetTanksStatistic<TankRandomBattlesStatisticEntity>(player.AccountId, rev),
                //HistoricalStatistic = _repository.GetPlayerStatistic<HistoricalBattlesStatisticEntity>(player.PlayerId, rev),
                //TeamStatistic = _repository.GetPlayerStatistic<TeamBattlesStatisticEntity>(player.PlayerId, rev)
            };

            var serializeObject = SerializeStatistic(data);
            ApiMethod($"statistic").Post(serializeObject);
        }

        private static string SerializeStatistic(ClientStat data)
        {
            var compress = CompressHelper.Compress(JsonConvert.SerializeObject(data));
            var base64String = Convert.ToBase64String(compress);
            var serializeObject = JsonConvert.SerializeObject(new Statistic {CompressedData = base64String});
            return serializeObject;

            //ApiMethod($"statistic").Post(JsonConvert.SerializeObject(data));
        }

        private void UpdateLocalStatistic(int rev)
        {
            AppSettings settings = SettingsReader.Get();
            string statistic = ApiMethod($"statistic/{rev}").Get();
        }

        private int GetServerDataVersion(int playerId, string server)
        {
            string player = ApiMethod($"player/{server}/{playerId}").Get();
            if (!string.IsNullOrEmpty(player))
            {
                var entity = JsonConvert.DeserializeObject<PlayerEntity>(player);
                return entity.Rev;
            }
            return 0;
        }
    }
}
