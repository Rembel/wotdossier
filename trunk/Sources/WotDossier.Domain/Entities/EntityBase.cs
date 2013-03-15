using WotDossier.Common;

namespace WotDossier.Domain.Entities
{
    /// <summary>
    /// 	Base class for all entities connected with tables
    /// </summary>
    public abstract class EntityBase
    {
        #region Property names

        public static readonly string PropId = TypeHelper.GetPropertyName<EntityBase>(v => v.Id);
        public static readonly string PropVersion = TypeHelper.GetPropertyName<EntityBase>(v => v.Version);

        #endregion

        /// <summary>
        /// 	Identifier of an entity
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 	Gets/Sets version of current entity.
        /// </summary>
        public virtual int Version { get; set; }
    }
}