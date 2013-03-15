
using WotDossier.Domain.Entities;

namespace WotDossier.Dal
{
    /// <summary>
    /// 	Represents provider interface for working with database.
    /// </summary>
    public interface IDataProvider
    {
        /// <summary>
        /// 	Opens current SQL session.
        /// </summary>
        void OpenSession();

        /// <summary>
        /// 	Close current SQL session.
        /// </summary>
        void CloseSession();

        /// <summary>
        /// 	Begins SQL transaction.
        /// </summary>
        void BeginTransaction();

        /// <summary>
        /// 	Commits SQL transaction.
        /// </summary>
        void CommitTransaction();

        /// <summary>
        /// 	Rollbacks SQL transaction and close session.
        /// </summary>
        void RollbackTransaction();

        /// <summary>
        /// 	Saves entity to database.
        /// </summary>
        object Save<T>(T entity)
            where T:EntityBase;

        /// <summary>
        /// Gets the entity by id.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        T GetEntityById<T>(object id)
            where T : EntityBase;

        /// <summary>
        /// Удаление сущности
        /// </summary>
        /// <param name="entity">сущность для удаления</param>
        void Delete(EntityBase entity);

        /// <summary>
        /// Удалить сущность из кэша
        /// </summary>
        /// <param name="entity">Сущность, удаляемая из кэша</param>
        void ClearCache(EntityBase entity);

        /// <summary>
        /// Очистка кэша сущностей
        /// </summary>
        void ClearCache();

        /// <summary>
        /// удаление сущности
        /// </summary>
        /// <param name="id">идентификатор сущности для удаления</param>
        void Delete<T>(object id) where T : EntityBase;
    }

}
