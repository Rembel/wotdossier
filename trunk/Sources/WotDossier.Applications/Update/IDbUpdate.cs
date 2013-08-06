using System.Data.SqlServerCe;

namespace WotDossier.Applications.Update
{
    public interface IDbUpdate
    {
        long Version { get; set; }
        void Execute(SqlCeConnection sqlCeConnection, SqlCeTransaction transaction);
    }
}