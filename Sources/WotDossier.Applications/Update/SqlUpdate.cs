using System.Data;
using System.Data.SQLite;
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public void Execute(SQLiteConnection sqlCeConnection, SQLiteTransaction transaction)
        {
            SQLiteCommand command = new SQLiteCommand(SqlScript, sqlCeConnection, transaction);

            command.CommandType = CommandType.Text;

            command.ExecuteNonQuery();
        }
    }
}