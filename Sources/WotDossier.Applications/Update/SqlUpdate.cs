using System.Data;
using System.Data.SQLite;
using System.IO;

namespace WotDossier.Applications.Update
{
    public class SqlUpdate : IDbUpdate
    {
        private readonly string _sqlScriptPath;

        public SqlUpdate(string sqlScriptPath)
        {
            _sqlScriptPath = sqlScriptPath;
            FileInfo info = new FileInfo(sqlScriptPath);
            Version = long.Parse(info.Name.Replace(info.Extension, string.Empty));
        }

        public long Version { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public void Execute(SQLiteConnection sqlCeConnection, SQLiteTransaction transaction)
        {
            string sqlScript = File.ReadAllText(_sqlScriptPath);

            SQLiteCommand command = new SQLiteCommand(sqlScript, sqlCeConnection, transaction);

            command.CommandType = CommandType.Text;

            command.ExecuteNonQuery();
        }
    }
}