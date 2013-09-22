using System.Data.SqlServerCe;
using WotDossier.Dal;
using WotDossier.Domain;

namespace WotDossier.Applications.Update
{
    public class CodeUpdate_2013092201 : CodeUpdateBase
    {
        private long _version = 2013092201;

        public override long Version
        {
            get { return _version; }
            set { _version = value; }
        }

        public override void Execute(SqlCeConnection sqlCeConnection, SqlCeTransaction transaction)
        {
            AppSettings appSettings = SettingsReader.Get();
            if (appSettings.PlayerId != null && appSettings.PlayerUniqueId == 0)
            {
                var player = WotApiClient.Instance.SearchPlayer(appSettings);
                if (player != null)
                {
                    appSettings.PlayerUniqueId = player.id;
                }
                else
                {
                    appSettings.PlayerId = null;
                }
                SettingsReader.Save(appSettings);
            }
        }
    }
}