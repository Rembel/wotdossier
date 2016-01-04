using System;
using System.Data.SQLite;
using System.IO;
using Common.Logging;
using WotDossier.Dal;

namespace WotDossier.Update.Update
{
    /// <summary>
    /// Delete replays cache
    /// </summary>
    public class CodeUpdate_2015071301 : CodeUpdateBase
    {
        private static readonly ILog _log = LogManager.GetLogger<CodeUpdate_2015071301>();

        private long _version = 2015071301;

        public override long Version
        {
            get { return _version; }
            set { _version = value; }
        }

        public override void Execute(SQLiteConnection sqlCeConnection, SQLiteTransaction transaction)
        {
            var path = Path.Combine(Folder.GetDossierAppDataFolder(), "replays.cache");
            if (File.Exists(path))
            {
                try
                {
                    File.Delete(path);
                }
                catch (Exception e)
                {
                    _log.Error("Can't delete replays cache", e);
                }
            }
        }
    }
}