using System.Data;
using System.Reflection;
using NHibernate.Driver;
using NHibernate.SqlTypes;

namespace WotDossier.Dal.NHibernate
{
    public class ExSqlServerCeDriver : SqlServerCeDriver
    {
        protected override void InitializeParameter(IDbDataParameter dbParam, string name, SqlType sqlType)
        {
            base.InitializeParameter(dbParam, name, sqlType);

            if (sqlType is BinarySqlType)
            {
                PropertyInfo dbParamSqlDbTypeProperty = dbParam.GetType().GetProperty("SqlDbType");
                dbParamSqlDbTypeProperty.SetValue(dbParam, SqlDbType.Image, null);
            }
        }
    }
}
