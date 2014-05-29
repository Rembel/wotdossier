using System.ComponentModel.Composition;
using System.Threading;
using NHibernate;

namespace WotDossier.Dal.NHibernate
{
    [Export(typeof(ISessionStorage))]
    public class DesktopAppSessionStorage : ISessionStorage
    {
        private const string SESSION_KEY = "23d1f199-0342-4eba-a79b-4322fe3d3430";

        /// <summary>
        /// 	Gets/Sets current NHibernate session.
        /// </summary>
        public ISession CurrentSession
        {
            get
            {
                return (ISession)Thread.GetData(Thread.GetNamedDataSlot(SESSION_KEY));
            }
            set
            {
                Thread.SetData(Thread.GetNamedDataSlot(SESSION_KEY), value);
            }
        }

    }
}
