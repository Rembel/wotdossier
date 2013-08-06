using System.Data;
using System.Data.SqlServerCe;
using System.IO;

namespace WotDossier.Applications.Update
{
    public class SqlUpdate : IDbUpdate
    {
        public SqlUpdate(string sqlScriptPath)
        {
            FileInfo info = new FileInfo(sqlScriptPath);
            SqlScript = File.ReadAllText(sqlScriptPath);
            Version = long.Parse(info.Name.Replace(info.Extension, string.Empty));
        }

        public long Version { get; set; }

        public string SqlScript { get; set; }

        public void Execute(SqlCeConnection sqlCeConnection, SqlCeTransaction transaction)
        {
            SqlCeCommand command = new SqlCeCommand(SqlScript, sqlCeConnection, transaction);

            command.CommandType = CommandType.Text;

            command.ExecuteNonQuery();
        }
    }
}