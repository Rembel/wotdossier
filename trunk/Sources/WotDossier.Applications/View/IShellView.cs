using System.Windows;
using WotDossier.Framework.Applications;

namespace WotDossier.Applications.View
{
    public interface IShellView : IView
    {
        Window Owner { set; get; }
    }
}
