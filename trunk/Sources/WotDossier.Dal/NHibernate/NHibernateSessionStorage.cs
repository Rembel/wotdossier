using System.Threading;
using System.Web;
using NHibernate;

namespace WotDossier.Dal.NHibernate
{
    public class NHibernateSessionStorage : ISessionStorage
    {
        private const string SESSION_KEY = "23d1f199-0342-4eba-a79b-4322fe3d3430";

        /// <summary>
        /// 	Gets/Sets current NHibernate session.
        /// </summary>
        public ISession CurrentSession
        {
            get
            {
                if (HttpContext.Current != null && HttpContext.Current.Items != null)
                {
                    return (ISession)HttpContext.Current.Items[SESSION_KEY];
                }
                return (ISession)Thread.GetData(Thread.GetNamedDataSlot(SESSION_KEY));
            }
            set
            {
                if (HttpContext.Current != null && HttpContext.Current.Items != null)
                {
                    HttpContext.Current.Items[SESSION_KEY] = value;
                }
                else
                {
                    Thread.SetData(Thread.GetNamedDataSlot(SESSION_KEY), value);
                }
            }
        }

    }
}
