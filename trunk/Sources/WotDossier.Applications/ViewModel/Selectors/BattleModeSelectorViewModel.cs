using System.Collections.Generic;
using WotDossier.Domain;

namespace WotDossier.Applications.ViewModel.Selectors
{
    public class BattleModeSelectorViewModel : Framework.Foundation.Model
    {
        private List<ListItem<BattleMode>> _battleModes = new List<ListItem<BattleMode>>
        {
            new ListItem<BattleMode>(BattleMode.RandomCompany, Resources.Resources.BattleMode_RandomCompany),
            new ListItem<BattleMode>(BattleMode.TeamBattle, Resources.Resources.BattleMode_TeamBattle), 
            new ListItem<BattleMode>(BattleMode.HistoricalBattle, Resources.Resources.BattleMode_HistoricalBattle), 
            new ListItem<BattleMode>(BattleMode.Clan, Resources.Resources.BattleMode_Clan), 
        };
        
        public List<ListItem<BattleMode>> BattleModes
        {
            get { return _battleModes; }
            set { _battleModes = value; }
        }

        private BattleMode _battleMode;
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