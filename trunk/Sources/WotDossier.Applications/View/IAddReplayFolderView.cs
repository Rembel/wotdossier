using System.Windows;
using WotDossier.Framework.Applications;

namespace WotDossier.Applications.View
{
    public interface IAddReplayFolderView : IView
    {
        Window Owner { set; get; }
    }
}
