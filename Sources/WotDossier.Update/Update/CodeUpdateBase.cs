using System.Data.SQLite;

namespace WotDossier.Update.Update
{
    public abstract class CodeUpdateBase : IDbUpdate
    {
        public abstract long Version { get; set; }

        public abstract void Execute(SQLiteConnection sqlCeConnection, SQLiteTransaction transaction);
    }
}