using System.Data.SQLite;
using System.IO;
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

        public override void Execute(SQLiteConnection sqlCeConnection, SQLiteTransaction transaction)
        {
            var filePath = SettingsReader.GetFilePath();

            if (File.Exists(filePath))
            {
                string replace;
                using (StreamReader stream = File.OpenText(filePath))
                {
                    var readToEnd = stream.ReadToEnd();
                    replace = readToEnd.Replace("PlayerId", "PlayerName");
                }

                using (StreamWriter stream = File.CreateText(filePath))
                {
                    stream.Write(replace);
                }
            }

            AppSettings appSettings = SettingsReader.Get();
            if (appSettings.PlayerName != null && appSettings.PlayerId == 0)
            {
                var player = WotApiClient.Instance.SearchPlayer(appSettings, appSettings.PlayerName);
                if (player != null)
                {
                    appSettings.PlayerId = player.id;
                }
                else
                {
                    appSettings.PlayerName = null;
                }
                SettingsReader.Save(appSettings);
            }
        }
    }
}