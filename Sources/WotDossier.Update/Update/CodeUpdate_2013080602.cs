using System;
using System.Data;
using System.Data.SQLite;

namespace WotDossier.Applications.Update
{
    public class CodeUpdate_2013080602 : CodeUpdateBase
    {
        private long _version = 2013080602;

        public override long Version
        {
            get { return _version; }
            set { _version = value; }
        }

        public override void Execute(SQLiteConnection sqlCeConnection, SQLiteTransaction transaction)
        {
            TimeSpan utcOffset = TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now);

            string commandText = @"Update PlayerStatistic set Updated = DATEADD(hour,@zone,Updated)";
            SQLiteCommand command = new SQLiteCommand(commandText, sqlCeConnection, transaction);

            command.CommandType = CommandType.Text;
            command.Parameters.Add("@zone", DbType.Int32).Value = utcOffset.Hours;

            command.ExecuteNonQuery();
        }
    }
}