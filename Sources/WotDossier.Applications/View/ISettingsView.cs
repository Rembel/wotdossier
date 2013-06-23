using System.ComponentModel;
using System.Windows;
using WotDossier.Framework.Applications;

namespace WotDossier.Applications.View
{
    public interface ISettingsView : IView
    {
        Window Owner { set; get; }
    }
}
