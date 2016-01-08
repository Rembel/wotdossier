using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using WotDossier.Dal;
using WotDossier.Domain.Entities;
using WotDossier.Domain.Tank;

namespace WotDossier.Update.Update
{
    /// <summary>
    /// BattleCount column added to tank statistic
    /// </summary>
    public class CodeUpdate_2016010501 : CodeUpdateBase
    {
        private long _version = 2016010501;

        public override long Version
        {
            get { return _version; }
            set { _version = value; }
        }

        public override void Execute(SQLiteConnection sqlCeConnection, SQLiteTransaction transaction)
        {
            string commandText = @"Select Id, Raw from TankRandomBattlesStatistic";
            SQLiteCommand command = new SQLiteCommand(commandText, sqlCeConnection, transaction);
            List<TankRandomBattlesStatisticEntity> list = new List<TankRandomBattlesStatisticEntity>();
            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    TankRandomBattlesStatisticEntity entity = new TankRandomBattlesStatisticEntity();
                    entity.Id = (int)(long) reader["Id"];
                    entity.Raw = (byte[]) reader["Raw"];
                    list.Add(entity);
                }
            }

            foreach (TankRandomBattlesStatisticEntity entity in list)
            {
                TankJson tank = CompressHelper.DecompressObject<TankJson>(entity.Raw);
                tank.Achievements7x7 = null;
                tank.AchievementsHistorical = null;
                tank.FortAchievements = null;
                tank.A7x7 = null;
                tank.AchievementsClan = null;
                tank.Clan = null;
                tank.Company = null;
                tank.FortBattles = null;
                tank.FortSorties = null;
                tank.Historical = null;
                commandText = @"Update TankRandomBattlesStatistic set Raw=@raw where Id=@id";
                command = new SQLiteCommand(commandText, sqlCeConnection, transaction);
                command.Parameters.Add("@id", DbType.Int32).Value = entity.Id;
                command.Parameters.Add("@raw", DbType.Binary).Value = CompressHelper.CompressObject(tank);
                command.ExecuteNonQuery();
            }
        }
    }
}