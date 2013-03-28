using System.ComponentModel.Composition;
using WotDossier.Applications.View;
using WotDossier.Applications.ViewModel.Rows;
using WotDossier.Framework.Applications;

namespace WotDossier.Applications.ViewModel
{
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [Export(typeof(TankStatisticViewModel))]
    public class TankStatisticViewModel : ViewModel<ITankStatisticView>
    {
        private TankStatisticRowViewModel _tankStatistic;

        public TankStatisticRowViewModel TankStatistic
        {
            get { return _tankStatistic; }
            set
            {
                _tankStatistic = value;
                RaisePropertyChanged("TankStatistic");
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModel&lt;TView&gt;"/> class and
        /// attaches itself as <c>DataContext</c> to the view.
        /// </summary>
        /// <param name="view">The view.</param>
        [ImportingConstructor]
        public TankStatisticViewModel([Import(typeof(ITankStatisticView))]ITankStatisticView view)
            : base(view)
        {
        }

        public virtual void Show()
        {
            ViewTyped.Show();
        }
    }
}
