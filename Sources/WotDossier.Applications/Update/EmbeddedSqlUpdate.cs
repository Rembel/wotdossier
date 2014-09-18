using System.Data;
using System.Data.SQLite;
using System.Reflection;
using WotDossier.Common;

namespace WotDossier.Applications.Update
{
    public class EmbeddedSqlUpdate : IDbUpdate
    {
        private readonly Assembly _assembly;
        private readonly string _resourceName;

        public EmbeddedSqlUpdate(Assembly assembly, string resourceName, string extension)
        {
            _assembly = assembly;
            _resourceName = resourceName;

            var replace = resourceName.Replace(extension, string.Empty);
            
            Version = long.Parse(replace.Substring(replace.LastIndexOf(".") + 1));
        }

        public long Version { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public void Execute(SQLiteConnection sqlCeConnection, SQLiteTransaction transaction)
        {
            string sqlScript = AssemblyExtensions.GetTextEmbeddedResource(_resourceName, _assembly);

            SQLiteCommand command = new SQLiteCommand(sqlScript, sqlCeConnection, transaction);

            command.CommandType = CommandType.Text;

            command.ExecuteNonQuery();
        }
    }
}