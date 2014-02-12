using System.Data.SQLite;

namespace WotDossier.Applications.Update
{
    public interface IDbUpdate
    {
        long Version { get; set; }
        void Execute(SQLiteConnection sqlCeConnection, SQLiteTransaction transaction);
    }
}