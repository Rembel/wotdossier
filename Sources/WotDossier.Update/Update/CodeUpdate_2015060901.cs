using System.Data.SQLite;
using WotDossier.Dal;
using WotDossier.Domain;

namespace WotDossier.Update.Update
{
    /// <summary>
    /// Delete double tank statistic rows
    /// </summary>
    public class CodeUpdate_2015060901 : CodeUpdateBase
    {
        private long _version = 2015060901;

        public override long Version
        {
            get { return _version; }
            set { _version = value; }
        }

        public override void Execute(SQLiteConnection sqlCeConnection, SQLiteTransaction transaction)
        {
            AppSettings appSettings = SettingsReader.Get();
            if (string.IsNullOrEmpty(appSettings.DossierCachePath))
            {
                appSettings.DossierCachePath = Folder.GetDossierCacheFolder();
            }
            SettingsReader.Save(appSettings);
        }
    }
}