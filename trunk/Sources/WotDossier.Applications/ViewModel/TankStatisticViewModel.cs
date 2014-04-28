using System.ComponentModel.Composition;
using System.Windows;
using WotDossier.Applications.View;
using WotDossier.Applications.ViewModel.Rows;
using WotDossier.Common;
using WotDossier.Domain;
using WotDossier.Framework.Applications;

namespace WotDossier.Applications.ViewModel
{
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [Export(typeof(TankStatisticViewModel))]
    public class TankStatisticViewModel : ViewModel<ITankStatisticView>
    {
        private ITankStatisticRow _tankStatistic;

        public ITankStatisticRow TankStatistic
        {
            get { return _tankStatistic; }
            set
            {
                _tankStatistic = value;
                RaisePropertyChanged("TankStatistic");
            }
        }

        private static readonly string PropPeriodTabHeader = TypeHelper.GetPropertyName<ShellViewModel>(x => x.PeriodTabHeader);

        private string _periodTabHeader;

        /// <summary>
        /// Gets or sets the period tab header.
        /// </summary>
        /// <value>
        /// The period tab header.
        /// </value>
        public string PeriodTabHeader
        {
            get { return _periodTabHeader; }
            set
            {
                _periodTabHeader = value;
                RaisePropertyChanged(PropPeriodTabHeader);
            }
        }

        public CommonChartsViewModel ChartView { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModel&lt;TView&gt;"/> class and
        /// attaches itself as <c>DataContext</c> to the view.
        /// </summary>
        /// <param name="view">The view.</param>
        [ImportingConstructor]
        public TankStatisticViewModel([Import(typeof(ITankStatisticView))]ITankStatisticView view)
            : base(view)
        {
            ViewTyped.Loaded += OnShellViewActivated;
            ChartView = new CommonChartsViewModel();
            SetPeriodTabHeader();
        }

        private void SetPeriodTabHeader()
        {
            AppSettings appSettings = SettingsReader.Get();
            PeriodTabHeader = Resources.Resources.ResourceManager.GetFormatedEnumResource(appSettings.PeriodSettings.Period, appSettings.PeriodSettings.Period == StatisticPeriod.Custom ? (object)appSettings.PeriodSettings.PrevDate : appSettings.PeriodSettings.LastNBattles);
        }

        private void OnShellViewActivated(object sender, RoutedEventArgs e)
        {
            ViewTyped.Loaded -= OnShellViewActivated;
            ChartView.InitCharts((StatisticViewModelBase)TankStatistic);
        }

        public virtual void Show()
        {
            ViewTyped.ShowDialog();
        }
    }
}
