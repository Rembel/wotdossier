using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlServerCe;
using System.IO;
using System.Linq;
using System.Reflection;
using Common.Logging;

namespace WotDossier.Applications.Update
{
    public class DatabaseManager
    {
        private static readonly ILog Logger = LogManager.GetLogger("ShellViewModel");

        public void Update()
        {
            List<IDbUpdate> updates = GetDbUpdates();

            long version = GetCurrentDbVersion();

            SqlCeConnection connection = null;
            SqlCeTransaction transaction = null;

            try
            {
                connection = GetConnection();
                Logger.Debug("Update. Source connection obtained");

                transaction = BeginTransaction(connection);
                Logger.Debug("Update. Dest transaction started");

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
                Logger.Error("Exception", e);
                throw;
            }
            finally
            {
                CloseConnection(connection);
                Logger.Debug("Update. Source connection closed");
            }
        }

        private void UpdateDbVersion(long max, SqlCeConnection connection, SqlCeTransaction transaction)
        {
            //Logger.Debug("BatchImportBcg. Source connection obtained");
            SqlCeCommand command = new SqlCeCommand(@"update DbVersion set SchemaVersion = @version", connection, transaction);
            command.CommandType = CommandType.Text;
            command.Parameters.Add("@version", SqlDbType.NVarChar).Value = max.ToString();

            command.ExecuteNonQuery();
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

                if (result == null)
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
                Logger.Error("Get Current DbVersion error", e);
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
            var types = type.Assembly.GetTypes().Where(type1 => type.IsAssignableFrom(type1) && type1 != type);

            string currentDirectory = Folder.AssemblyDirectory();
            string[] strings = Directory.GetFiles(Path.Combine(currentDirectory, "Updates"), "*.sql");

            List<IDbUpdate> updates = strings.Select(x => (IDbUpdate)new SqlUpdate(x)).ToList();
            updates.AddRange(types.Select(Activator.CreateInstance).Cast<IDbUpdate>());

            return updates.OrderBy(x => x.Version).ToList();
        }

        public void InitDatabase()
        {
            string currentDirectory = Folder.AssemblyDirectory();
            string path = Path.Combine(currentDirectory, @"Data\dossier.sdf");
            if (!File.Exists(path))
            {
                byte[] embeddedResource = GetEmbeddedResource(@"WotDossier.Data.init.sdf", Assembly.GetEntryAssembly());
                using (FileStream fileStream = File.OpenWrite(path))
                {
                    fileStream.Write(embeddedResource, 0, embeddedResource.Length);    
                    fileStream.Flush();
                }
            }
        }

        public static byte[] GetEmbeddedResource(string resourceName, Assembly assembly)
        {
            using (Stream resourceStream = assembly.GetManifestResourceStream(resourceName))
            {
                if (resourceStream == null)
                    return null;

                int length = Convert.ToInt32(resourceStream.Length); // get strem length
                byte[] byteArr = new byte[length]; // create a byte array
                resourceStream.Read(byteArr, 0, length);

                return byteArr;
            }
        }
    }
}
