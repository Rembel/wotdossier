using System.ComponentModel.Composition;
using Common.Logging;
using WotDossier.Applications.View;
using WotDossier.Dal;
using WotDossier.Framework.Applications;

namespace WotDossier.Applications.ViewModel
{
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [Export(typeof (ClanViewModel))]
    public class ClanViewModel : ViewModel<IClanView>
    {
        private readonly DossierRepository _repository;
        private static readonly ILog _log = LogManager.GetLogger("ClanViewModel");

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModel&lt;TView&gt;" /> class and
        /// attaches itself as <c>DataContext</c> to the view.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="repository">The repository.</param>
        [ImportingConstructor]
        public ClanViewModel([Import(typeof(IClanView))]IClanView view, [Import(typeof(DossierRepository))]DossierRepository repository)
            : base(view)
        {
            _repository = repository;
        }

        public void Show()
        {
            ViewTyped.ShowDialog();
        }
    }
}
