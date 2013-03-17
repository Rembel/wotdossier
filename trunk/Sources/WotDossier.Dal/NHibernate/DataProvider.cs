using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using Common.Logging;
using Croc.Aws.DataAccess;
using Croc.Aws.DataAccess.NHibernate;
using FluentNHibernate.Cfg;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Event;
using NHibernate.Linq;
using WotDossier.Domain.Entities;

namespace WotDossier.Dal.NHibernate
{
    /// <summary>
    /// 	Represents provider class for working with database.
    /// </summary>
    public class DataProvider: IDataProvider
    {
        protected static readonly ILog Log = LogManager.GetLogger("DataProvider");

        private readonly ISessionFactory _factory;
        private readonly ISessionStorage _storage;

        /// <summary>
        /// 	Creates new instance of <see cref = "DataProvider" />.
        /// </summary>
        public DataProvider(ISessionStorage storage)
        {
            _storage = storage;

            Configuration configuration = new Configuration().Configure();
            configuration.EventListeners.FlushEntityEventListeners = new IFlushEntityEventListener[] { new FlushEntityEventListener() };
            _factory = InitFluentMappings(configuration).BuildSessionFactory();
        }

        private static FluentConfiguration InitFluentMappings(Configuration configuration)
        {
            FluentConfiguration fluentConfiguration = Fluently.Configure(configuration);

            IEnumerable<Assembly> assemblies = GetDomainAssemblies();

            foreach (Assembly assembly in assemblies)
            {
                Assembly temp = assembly;
                fluentConfiguration.Mappings(v => v.FluentMappings.AddFromAssembly(temp));
            }

            return fluentConfiguration;
        }

        //определяем набор доменных сборок в которых определены мапинги для NH
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2001:AvoidCallingProblematicMethods", MessageId = "System.Reflection.Assembly.LoadFile")]
        private static IEnumerable<Assembly> GetDomainAssemblies()
        {
            string currentDirectory = AssemblyDirectory();
            string[] strings = Directory.GetFiles(currentDirectory, "*Dal*.dll");

            List<Assembly> list = new List<Assembly>();

            strings.ForEach(x => list.Add(Assembly.LoadFile(x)));

            return list;
        }

        private static string AssemblyDirectory()
        {
            var uri = new UriBuilder(Assembly.GetExecutingAssembly().CodeBase);
            return Path.GetDirectoryName(Uri.UnescapeDataString(uri.Path));
        }

        /// <summary>
        /// 	Gets/Sets current NHibernate session.
        /// </summary>
        private ISession CurrentSession
        {
            get { return _storage.CurrentSession; }
            set { _storage.CurrentSession = value; }
        }

        /// <summary>
        /// 	Opens current NHibernate session.
        /// </summary>
        public void OpenSession()
        {
            CurrentSession = _factory.OpenSession();
            CurrentSession.FlushMode = FlushMode.Never;
        }

        /// <summary>
        /// 	Close current NHibernate session.
        /// </summary>
        public void CloseSession()
        {
            if (CurrentSession != null && CurrentSession.IsOpen)
            {
                CurrentSession.Close();
                CurrentSession = null;
            }
        }

        /// <summary>
        /// 	Begins NHibernate transaction.
        /// </summary>
        public void BeginTransaction()
        {
            if (CurrentSession != null)
            {
                CurrentSession.BeginTransaction();
            }
        }

        /// <summary>
        /// 	Commits NHibernate transaction.
        /// </summary>
        public void CommitTransaction()
        {
            try
            {
                if (CurrentSession != null && CurrentSession.Transaction.IsActive)
                {
                    CurrentSession.Transaction.Commit();
                }
            }
            catch(Exception e)
            {
                Log.Error("Ошибка при коммите транзакции", e);
                RollbackTransaction();
                throw;
            }
        }

        /// <summary>
        /// 	Rollbacks NHibernate transaction and close session.
        /// </summary>
        public void RollbackTransaction()
        {
            try
            {
                if (CurrentSession != null && CurrentSession.Transaction.IsActive)
                {
                    _storage.CurrentSession.Transaction.Rollback();
                }
            }
            catch (Exception e)
            {
                Log.Error("Ошибка при откате транзакции транзакции", e);
                throw;
            }
            finally
            {
                CloseSession();
            }
        }

        /// <summary>
        /// 	Creates root criteria for current session.
        /// </summary>
        public ICriteria CreateCriteria<T>()
            where T:EntityBase
        {
            return CurrentSession.CreateCriteria(typeof (T));
        }

        /// <summary>
        /// Creates a new <c>IQueryOver&lt;T&gt;</c> for the entity class.
        /// </summary>
        /// <typeparam name="T">The entity class</typeparam>
        /// <returns>
        /// An IQueryOver&lt;T&gt; object
        /// </returns>
        public IQueryOver<T, T> QueryOver<T>() where T : class
        {
            return CurrentSession.QueryOver<T>();
        }

        /// <summary>
        /// Creates a new <c>IQueryOver&lt;T&gt;</c> for the entity class.
        /// </summary>
        /// <typeparam name="T">The entity class</typeparam><param name="alias">The alias of the entity</param>
        /// <returns>
        /// An IQueryOver&lt;T&gt; object
        /// </returns>
        public IQueryOver<T, T> QueryOver<T>(Expression<Func<T>> alias) where T : class
        {
            return CurrentSession.QueryOver(alias);
        }

        /// <summary>
        /// Creates a new <c>IQueryOver{T};</c> for the entity class.
        /// </summary>
        /// <typeparam name="T">The entity class</typeparam><param name="entityName">The name of the entity to Query</param>
        /// <returns>
        /// An IQueryOver{T} object
        /// </returns>
        public IQueryOver<T, T> QueryOver<T>(string entityName) where T : class
        {
            return CurrentSession.QueryOver<T>(entityName);
        }

        /// <summary>
        /// Creates a new <c>IQueryOver{T}</c> for the entity class.
        /// </summary>
        /// <typeparam name="T">The entity class</typeparam><param name="entityName">The name of the entity to Query</param><param name="alias">The alias of the entity</param>
        /// <returns>
        /// An IQueryOver{T} object
        /// </returns>
        public IQueryOver<T, T> QueryOver<T>(string entityName, Expression<Func<T>> alias) where T : class
        {
            return CurrentSession.QueryOver(entityName, alias);
        }

        /// <summary>
        /// Create a new instance of <c>Query</c> for the given query string
        /// </summary>
        /// <param name="queryString">A hibernate query string</param>
        /// <returns>
        /// The query
        /// </returns>
        public IQuery CreateQuery(string queryString)
        {
            return CurrentSession.CreateQuery(queryString);
        }

        /// <summary>
        /// Create a new instance of <see cref="T:NHibernate.ISQLQuery"/> for the given SQL query string.
        /// </summary>
        /// <param name="queryString">a query expressed in SQL</param>
        /// <returns>
        /// An <see cref="T:NHibernate.ISQLQuery"/> from the SQL string
        /// </returns>
        public ISQLQuery CreateSQLQuery(string queryString)
        {
            return CurrentSession.CreateSQLQuery(queryString);
        }


        /// <summary>
        /// 	Saves entity to database.
        /// </summary>
        public object Save<T>(T entity)
            where T:EntityBase
        {

            try
            {

                var id = CurrentSession.Save(entity);
                //TODO: CR: Isn't it dangerous for performance?
                CurrentSession.Flush();
                return id;
            }
            catch (StaleObjectStateException e)
            {
                Log.Error("Ошибка при сохранении", e);
                throw new ConcurrencyException(e.EntityName, e.Identifier);
            }
        }

        /// <summary>
        /// удаление сущности
        /// </summary>
        /// <param name="id">идентификатор сущности для удаления</param>
        public void Delete<T>(object id) where T : EntityBase
        {
            if (id == null)
                throw new ArgumentNullException("id");

            EntityBase entityBase = GetEntityById<T>(id);
            Delete(entityBase);
        }

        /// <summary>
        /// Удаление сущности
        /// </summary>
        /// <param name="entity">сущность для удаления</param>
        public void Delete(EntityBase entity)
        {
            if (entity != null)
            {
                //ExceptionProcessor.Invoke(() =>
                //{
                //    CheckDeadlocks(entity);
                //    UpdateTransactionMode();
                    CurrentSession.Delete(entity);
                    CurrentSession.Flush();
                //});
            }
        }

        /// <summary>
        /// Gets the entity by id.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public T GetEntityById<T>(object id) where T : EntityBase
        {
            return CurrentSession.Get<T>(id);
        }

        /// <summary>
        /// Удалить сущность из кэша
        /// </summary>
        /// <param name="entity">Сущность, удаляемая из кэша</param>
        public void ClearCache(EntityBase entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            CurrentSession.Evict(entity);
        }


        /// <summary>
        /// Очистка кэша сущностей
        /// </summary>
        public void ClearCache()
        {
            CurrentSession.Clear();
        }

        public void Flush()
        {
            CurrentSession.Flush();
        }

        /// <summary>
        /// Obtain an instance of <see cref="T:NHibernate.IQuery"/> for a named query string defined in the
        ///             mapping file.
        /// </summary>
        /// <param name="queryName">The name of a query defined externally.</param>
        /// <returns>
        /// An <see cref="T:NHibernate.IQuery"/> from a named query string.
        /// </returns>
        /// <remarks>
        /// The query can be either in <c>HQL</c> or <c>SQL</c> format.
        /// </remarks>
        public IQuery GetNamedQuery(string queryName)
        {
            return CurrentSession.GetNamedQuery(queryName);
        }
    }
}
