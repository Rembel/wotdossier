using WotDossier.Framework.Applications;

namespace WotDossier.Applications.View
{
    public interface IUploadReplayView : IView
    {
        bool? ShowDialog();
        void Close();
    }
}
