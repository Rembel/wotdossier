using NHibernate;

namespace WotDossier.Dal.NHibernate
{
    /// <summary>
    /// 	Represents NHibernate session storage interface.
    /// </summary>
    public interface ISessionStorage
    {
        /// <summary>
        /// 	Gets/Sets current NHibernate session.
        /// </summary>
        ISession CurrentSession { get; set; }
    }
}
