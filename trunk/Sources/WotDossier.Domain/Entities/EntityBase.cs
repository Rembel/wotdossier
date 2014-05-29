namespace WotDossier.Domain.Entities
{
    /// <summary>
    /// 	Base class for all entities connected with tables
    /// </summary>
    public abstract class EntityBase
    {
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