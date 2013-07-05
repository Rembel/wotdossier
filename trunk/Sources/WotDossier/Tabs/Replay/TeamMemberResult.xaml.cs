using System.Windows;
using System.Windows.Controls;
using WotDossier.Framework.Forms.Commands;

namespace WotDossier.Tabs.Replay
{
    /// <summary>
    /// Interaction logic for TeamMemberResult.xaml
    /// </summary>
    public partial class TeamMemberResult : UserControl
    {
        #region public DelegateCommand HideTeamMemberResultsCommand

        /// <summary>
        /// Identifies the HideTeamMemberResultsCommand dependency property.
        /// </summary>
        public static DependencyProperty HideTeamMemberResultsCommandProperty =
            DependencyProperty.Register("HideTeamMemberResultsCommand", typeof(DelegateCommand), typeof(TeamMemberResult), new PropertyMetadata(default(DelegateCommand)));

        /// <summary>
        /// 
        /// </summary>
        public DelegateCommand HideTeamMemberResultsCommand
        {
            get { return (DelegateCommand) GetValue(HideTeamMemberResultsCommandProperty); }

            set { SetValue(HideTeamMemberResultsCommandProperty, value); }
        }

        #endregion public DelegateCommand HideTeamMemberResultsCommand

        public TeamMemberResult()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            if (HideTeamMemberResultsCommand != null)
            {
                HideTeamMemberResultsCommand.Execute();
            }
        }
    }
}
