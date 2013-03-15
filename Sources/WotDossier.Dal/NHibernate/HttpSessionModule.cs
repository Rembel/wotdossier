using System.Web;

namespace WotDossier.Dal.NHibernate
{
    /// <summary>
    /// 	Represents Http module for working with NHibernate session.
    /// </summary>
    /// TODO: CR: PYA: Consider remaking it to be called only when necessary and not to open transaction at all till the very first DB-related request.
    public class HttpSessionModule : IHttpModule
    {
        /// <summary>
        /// 	Initializes a module and prepares it to handle requests.
        /// </summary>
        /// <param name = "context">An <see cref = "T:System.Web.HttpApplication" /> that provides access to the methods, properties, and events common to all application objects within an ASP.NET application 
        /// </param>
        public void Init(HttpApplication context)
        {
            var dataProvider = SpringSingleton<IDataProvider>.Instance;
            context.BeginRequest += delegate
            {
                dataProvider.OpenSession();
                dataProvider.BeginTransaction();
            };

            context.EndRequest += delegate
            {
                dataProvider.CommitTransaction();
                dataProvider.CloseSession();
            };
            context.Error += delegate
            {
                dataProvider.RollbackTransaction();
            };
        }

        /// <summary>
        /// 	Disposes of the resources (other than memory) used by the module that implements <see cref = "T:System.Web.IHttpModule" />.
        /// </summary>
        public void Dispose()
        {
            //  _dataProvider.CloseSession();
        }
    }
}