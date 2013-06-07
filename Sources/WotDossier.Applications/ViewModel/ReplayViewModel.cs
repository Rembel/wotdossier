using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading;
using WotDossier.Applications.View;
using WotDossier.Dal;
using WotDossier.Domain.Replay;
using WotDossier.Domain.Tank;
using WotDossier.Framework.Applications;

namespace WotDossier.Applications.ViewModel
{
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [Export(typeof(ReplayViewModel))]
    public class ReplayViewModel : ViewModel<IReplayView>
    {
        private ReplayFile _replay;

        public ReplayFile Replay
        {
            get { return _replay; }
            set { _replay = value; }
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModel&lt;TView&gt;"/> class and
        /// attaches itself as <c>DataContext</c> to the view.
        /// </summary>
        /// <param name="view">The view.</param>
        [ImportingConstructor]
        public ReplayViewModel([Import(typeof(IReplayView))]IReplayView view)
            : base(view)
        {
        }

        public void Show()
        {
            ViewTyped.Show();

            //convert dossier cache file to json
            CacheHelper.ReplayToJson(Replay.FileInfo);
            Thread.Sleep(1000);
            Replay replay = WotApiClient.Instance.ReadReplay(Replay.FileInfo.FullName.Replace(Replay.FileInfo.Extension, ".json"));
        }
    }
}
