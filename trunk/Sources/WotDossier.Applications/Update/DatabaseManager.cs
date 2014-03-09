﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Common.Logging;

namespace WotDossier.Applications.Update
{
    public class DatabaseManager
    {
        private const string DATA_DOSSIER_SDF = @"\Data\dossier.sdf";
        private const string BACKUP_DOSSIER_SDF = @"\backup\dossier_{0}.sdf";
        private static readonly ILog Logger = LogManager.GetLogger("DatabaseManager");

        public void Update()
        {
            List<IDbUpdate> updates = GetDbUpdates();

            MigrateToSqlite();

            long version = GetCurrentDbVersion();

            SQLiteConnection connection = null;
            SQLiteTransaction transaction = null;

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

        private void MigrateToSqlite()
        {
            string directoryName = Folder.AssemblyDirectory();
            string path = Path.Combine(directoryName, @"Data\dossier.s3db");

            string ceDbFilePath = directoryName + DATA_DOSSIER_SDF;
            string ceDbFileBackupPath = directoryName + string.Format(BACKUP_DOSSIER_SDF, DateTime.Now.Ticks);
            string migrationScriptPath = Path.Combine(Path.GetTempPath(), "data.sql");

            if (File.Exists(ceDbFilePath))
            {
                using (File.Open(path, FileMode.Truncate)) { }

                var proc = new Process();
                proc.EnableRaisingEvents = false;
                proc.StartInfo.CreateNoWindow = true;
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.FileName = directoryName + @"\External\ExportSqlCe40.exe";
                proc.StartInfo.Arguments = string.Format(@"""Data Source={0}"" ""{1}"" sqlite", ceDbFilePath, migrationScriptPath);
                proc.Start();
                proc.WaitForExit();

                Migrate(migrationScriptPath);

                //backup ce db
                BackupSdf(ceDbFileBackupPath, ceDbFilePath);
            }
        }

        private void Migrate(string migrationScriptPath)
        {
            SQLiteConnection sqLiteConnection = null;
            
            try
            {
                sqLiteConnection = GetConnection();
                string sqlScript = File.ReadAllText(migrationScriptPath);
                SQLiteCommand command = new SQLiteCommand(sqlScript, sqLiteConnection);
                command.CommandType = CommandType.Text;
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Logger.Error("Migrate failed", e);
            }
            finally
            {
                CloseConnection(sqLiteConnection);
            }
        }

        private static void BackupSdf(string ceDbFileBackupPath, string ceDbFilePath)
        {
            string backupDir = new FileInfo(ceDbFileBackupPath).DirectoryName;
            if (backupDir != null && !Directory.Exists(backupDir))
            {
                Directory.CreateDirectory(backupDir);
            }
            File.Move(ceDbFilePath, ceDbFileBackupPath);
        }

        private void UpdateDbVersion(long max, SQLiteConnection connection, SQLiteTransaction transaction)
        {
            //Logger.Debug("BatchImportBcg. Source connection obtained");
            SQLiteCommand command = new SQLiteCommand(@"update DbVersion set SchemaVersion = @version", connection, transaction);
            command.CommandType = CommandType.Text;
            command.Parameters.Add("@version", DbType.String).Value = max.ToString();

            command.ExecuteNonQuery();
        }

        private void CloseConnection(SQLiteConnection connection)
        {
            if (connection != null)
            {
                connection.Close();
            }
        }

        private void RollbackTransaction(SQLiteTransaction transaction)
        {
            transaction.Rollback();
        }

        private SQLiteTransaction BeginTransaction(SQLiteConnection sourceConnection)
        {
            return sourceConnection.BeginTransaction();
        }

        private SQLiteConnection GetConnection()
        {
            SQLiteConnection connection = new SQLiteConnection("Data Source=Data/dossier.s3db;Version=3;");
            connection.Open();
            return connection;
        }

        private long GetCurrentDbVersion()
        {
            SQLiteConnection connection = null;

            try
            {
                connection = GetConnection();
                //Logger.Debug("BatchImportBcg. Source connection obtained");
                SQLiteCommand command = new SQLiteCommand(@"Select SchemaVersion from DbVersion limit 1", connection);
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
            string path = Path.Combine(currentDirectory, @"Data\dossier.s3db");
            if (!File.Exists(path))
            {
                byte[] embeddedResource = GetEmbeddedResource(@"WotDossier.Data.init.s3db", Assembly.GetEntryAssembly());
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
