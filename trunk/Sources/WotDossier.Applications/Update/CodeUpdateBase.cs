using System.Data.SqlServerCe;

namespace WotDossier.Applications.Update
{
    public abstract class CodeUpdateBase : IDbUpdate
    {
        public abstract long Version { get; set; }

        public abstract void Execute(SqlCeConnection sqlCeConnection, SqlCeTransaction transaction);
    }
}