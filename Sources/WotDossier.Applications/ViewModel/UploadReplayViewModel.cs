using System.ComponentModel.Composition;
using WotDossier.Applications.View;
using WotDossier.Framework.Applications;

namespace WotDossier.Applications.ViewModel
{
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [Export(typeof (UploadReplayViewModel))]
    public class UploadReplayViewModel : ViewModel<IUploadReplayView>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModel&lt;TView&gt;"/> class and
        /// attaches itself as <c>DataContext</c> to the view.
        /// </summary>
        /// <param name="view">The view.</param>
        [ImportingConstructor]
        public UploadReplayViewModel([Import(typeof(IUploadReplayView))]IUploadReplayView view)
            : base(view)
        {
        }

        public void Show()
        {
            ViewTyped.Show();
        }
    }
}
