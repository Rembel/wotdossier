using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlServerCe;
using System.IO;
using System.Linq;

namespace WotDossier.Applications.Update
{
    public class DatabaseManager
    {
        public void Update()
        {
            List<DbUpdate> updates = GetDbUpdates();

            long version = GetCurrentDbVersion();

            SqlCeConnection connection = null;
            SqlCeTransaction transaction = null;

            try
            {
                connection = GetConnection();
                //Logger.Debug("BatchImportBcg. Source connection obtained");

                transaction = BeginTransaction(connection);
                //Logger.Debug("BatchImportBcg. Dest transaction started");

                foreach (var dbUpdate in updates)
                {
                    //TODO transactions
                    if (dbUpdate.Version > version)
                    {
                        dbUpdate.Execute(connection, transaction);
                        UpdateDbVersion(dbUpdate.Version, connection, transaction);
                    }
                }

                transaction.Commit();
            }
            catch (Exception e)
            {
                RollbackTransaction(transaction);
            }
            finally
            {
                CloseConnection(connection);
                //Logger.Debug("BatchImportBcg. Source connection closed");
            }
        }

        private void UpdateDbVersion(long max, SqlCeConnection sqlCeConnection, SqlCeTransaction transaction)
        {
            
        }

        private void CloseConnection(SqlCeConnection connection)
        {
            connection.Close();
        }

        private void RollbackTransaction(SqlCeTransaction transaction)
        {
            transaction.Rollback();
        }

        private SqlCeTransaction BeginTransaction(SqlCeConnection sourceConnection)
        {
            return sourceConnection.BeginTransaction();
        }

        private SqlCeConnection GetConnection()
        {
            SqlCeConnection connection = new SqlCeConnection("Data Source=Data/dossier.sdf");
            connection.Open();
            return connection;
        }

        private long GetCurrentDbVersion()
        {
            SqlCeConnection connection = null;

            try
            {
                connection = GetConnection();
                //Logger.Debug("BatchImportBcg. Source connection obtained");
                SqlCeCommand command = new SqlCeCommand(@"Select top 1 SchemaVersion from DbVersion", connection);
                command.CommandType = CommandType.Text;
                object result = command.ExecuteScalar();

                if (result == DBNull.Value)
                {
                    return 0;
                }
                else
                {
                    return int.Parse(result.ToString());
                }
            }
            catch (Exception e)
            {          
                return int.MaxValue;
            }
            finally
            {
                CloseConnection(connection);
                //Logger.Debug("BatchImportBcg. Source connection closed");
            }
        }

        private List<DbUpdate> GetDbUpdates()
        {
            string currentDirectory = Folder.AssemblyDirectory();
            string[] strings = Directory.GetFiles(Path.Combine(currentDirectory, "Updates"), "*.sql");

            return strings.Select(x => new DbUpdate(x)).OrderBy(x => x.Version).ToList();
        }
    }

    public interface IDbUpdate
    {
        long Version { get; set; }
        void Execute(SqlCeConnection sqlCeConnection, SqlCeTransaction transaction);
    }

    public class DbUpdate : IDbUpdate
    {
        public DbUpdate(string sqlScriptPath)
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
