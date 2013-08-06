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
            List<IDbUpdate> updates = GetDbUpdates();

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

        private List<IDbUpdate> GetDbUpdates()
        {
            var type = typeof(CodeUpdateBase);
            var types = type.Assembly.GetTypes().Where(type.IsAssignableFrom);

            string currentDirectory = Folder.AssemblyDirectory();
            string[] strings = Directory.GetFiles(Path.Combine(currentDirectory, "Updates"), "*.sql");

            List<IDbUpdate> updates = strings.Select(x => (IDbUpdate)new SqlUpdate(x)).ToList();
            updates.AddRange(types.Select(Activator.CreateInstance).Cast<IDbUpdate>());

            return updates.OrderBy(x => x.Version).ToList();
        }
    }
}
