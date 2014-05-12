using System.Diagnostics;
using WotDossier.Framework.Forms.Commands;

namespace WotDossier.Framework
{
    public class ApplicationStaticCommands
    {
        private static DelegateCommand<object> _openLinkCommand;

        public static DelegateCommand<object> OpenLinkCommand
        {
            get
            {
                if (_openLinkCommand == null)
                {
                    _openLinkCommand = new DelegateCommand<object>(OnOpenLink);
                }
                return _openLinkCommand;
            }
            set { _openLinkCommand = value; }
        }

        protected static void OnOpenLink(object link)
        {
            if (link != null)
            {
                Process.Start(link.ToString());
            }
        }
    }
}
