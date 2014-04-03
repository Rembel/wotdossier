using System.Collections.Generic;

namespace WotDossier.Applications.ViewModel
{
    public class BattleModeSelectorViewModel : Framework.Foundation.Model
    {
        private List<ListItem<BattleMode>> _battleModes = new List<ListItem<BattleMode>>
        {
            new ListItem<BattleMode>(BattleMode.RandomCompany, Resources.Resources.BattleMode_RandomCompany),
            new ListItem<BattleMode>(BattleMode.TeamBattle, Resources.Resources.BattleMode_TeamBattle), 
            new ListItem<BattleMode>(BattleMode.HistoricalBattle, Resources.Resources.BattleMode_HistoricalBattle), 
        };
        private BattleMode _battleMode;

        public List<ListItem<BattleMode>> BattleModes
        {
            get { return _battleModes; }
            set { _battleModes = value; }
        }

        public BattleMode BattleMode
        {
            get { return _battleMode; }
            set
            {
                _battleMode = value;
                RaisePropertyChanged("BattleMode");
            }
        }
    }
}