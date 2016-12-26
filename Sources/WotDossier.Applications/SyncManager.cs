using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using Common.Logging;
using Newtonsoft.Json;
using ProtoBuf;
using ProtoBuf.Meta;
using WotDossier.Common.Extensions;
using WotDossier.Dal;
using WotDossier.Domain;
using WotDossier.Domain.Entities;

namespace WotDossier.Applications
{
    public static class RuntimeTypeModelExt
    {
        public static MetaType Add<T>(this RuntimeTypeModel model)
        {
            var propertyInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(prop => prop.IsDefined(typeof(DataMemberAttribute), false) && prop.CanWrite).Select(x => x.Name).ToArray();

            return model.Add(typeof(T), true).Add(propertyInfos);
        }
    }

    public class SyncManager
    {
        protected static readonly ILog Log = LogManager.GetLogger<SyncManager>();

        public const string ApiBaseUrl = "http://localhost:5000/api/sync/";

        private readonly DossierRepository _repository;


        public SyncManager(DossierRepository repository)
        {
            _repository = repository;

            InitProtobuf();
        }

        private static void InitProtobuf()
        {
            RuntimeTypeModel.Default.AllowParseableTypes = true;
            RuntimeTypeModel.Default.AutoAddMissingTypes = true;

            RuntimeTypeModel.Default.Add<ClientStat>();
            RuntimeTypeModel.Default.Add<EntityBase>()
                .AddSubType(100, RuntimeTypeModel.Default.Add<PlayerEntity>().Type)
                .AddSubType(200, RuntimeTypeModel.Default.Add<TankEntity>().Type)
                .AddSubType(300, RuntimeTypeModel.Default.Add<StatisticEntity>()
                                    .AddSubType(400, RuntimeTypeModel.Default.Add<RandomBattlesStatisticEntity>().Type)
                                 .Type)
                .AddSubType(700, RuntimeTypeModel.Default.Add<RandomBattlesAchievementsEntity>().Type)
                .AddSubType(500, RuntimeTypeModel.Default.Add<TankStatisticEntityBase>()
                                    .AddSubType(600, RuntimeTypeModel.Default.Add<TankRandomBattlesStatisticEntity>().Type).Type)
                                    ;
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
                var serverDbVersion = ApiMethod("dbversion").Get<DbVersionEntity>();
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
            var tankRandomStatistic = _repository.GetTanksStatistic<TankRandomBattlesStatisticEntity>(player.Id, rev);
            var data = new ClientStat
            {
                Player = player, 
                Tanks = _repository.GetTanks(player, rev),
                RandomStatistic = _repository.GetPlayerStatistic<RandomBattlesStatisticEntity>(player.AccountId, rev),
                TankRandomStatistic = tankRandomStatistic,
                //HistoricalStatistic = _repository.GetPlayerStatistic<HistoricalBattlesStatisticEntity>(player.PlayerId, rev),
                //TeamStatistic = _repository.GetPlayerStatistic<TeamBattlesStatisticEntity>(player.PlayerId, rev)
            };

            var serializeObject = SerializeStatistic(data);

            //var compress = CompressHelper.Compress(JsonConvert.SerializeObject(data));
            //var base64String = Convert.ToBase64String(compress);
            //var serializeObject = JsonConvert.SerializeObject(new Statistic { CompressedData = base64String });
            //return serializeObject;

            //File.WriteAllBytes(@"c:\stat.json", serializeObject);

            ApiMethod($"statistic").Post(serializeObject);
        }

        private static byte[] SerializeStatistic(ClientStat data)
        {
            byte[] compress = new byte[0];
            using (var stream = new MemoryStream())
            {
                Serializer.Serialize(stream, data);
                compress = stream.ToArray();
                stream.Position = 0;
                var clientStat = Serializer.Deserialize<ClientStat>(stream);
            }

            return compress;
        }

        private void UpdateLocalStatistic(int rev)
        {
            //AppSettings settings = SettingsReader.Get();
            //string statistic = ApiMethod($"statistic/{rev}").Get();
        }

        private int GetServerDataVersion(int playerId, string server)
        {
            var player = ApiMethod($"player/{server}/{playerId}").Get<PlayerEntity>();
            if (player != null)
            {
                return player.Rev;
            }
            return 0;
        }
    }
}
