using WotDossier.Domain.Tank;

namespace WotDossier.Domain.Rows
{
    public class TankRowBattleAwards : TankRowBase, ITankRowBattleAwards
    {
        private int _battleHero;
        private int _topGun;
        private int _invader;
        private int _sniper;
        private int _defender;
        private int _steelWall;
        private int _confederate;
        private int _scout;
        private int _patrolDuty;
        private int _brothersInArms;
        private int _crucialContribution;
        private int _coolHeaded;
        private int _luckyDevil;
        private int _spartan;

        public int BattleHero
        {
            get { return _battleHero; }
            set { _battleHero = value; }
        }

        public int TopGun
        {
            get { return _topGun; }
            set { _topGun = value; }
        }

        public int Invader
        {
            get { return _invader; }
            set { _invader = value; }
        }

        public int Sniper
        {
            get { return _sniper; }
            set { _sniper = value; }
        }

        public int Defender
        {
            get { return _defender; }
            set { _defender = value; }
        }

        public int SteelWall
        {
            get { return _steelWall; }
            set { _steelWall = value; }
        }

        public int Confederate
        {
            get { return _confederate; }
            set { _confederate = value; }
        }

        public int Scout
        {
            get { return _scout; }
            set { _scout = value; }
        }

        public int PatrolDuty
        {
            get { return _patrolDuty; }
            set { _patrolDuty = value; }
        }

        public int BrothersInArms
        {
            get { return _brothersInArms; }
            set { _brothersInArms = value; }
        }

        public int CrucialContribution
        {
            get { return _crucialContribution; }
            set { _crucialContribution = value; }
        }

        public int CoolHeaded
        {
            get { return _coolHeaded; }
            set { _coolHeaded = value; }
        }

        public int LuckyDevil
        {
            get { return _luckyDevil; }
            set { _luckyDevil = value; }
        }

        public int Spartan
        {
            get { return _spartan; }
            set { _spartan = value; }
        }

        public TankRowBattleAwards(TankJson tank) : base(tank)
        {
            _battleHero = tank.Battle.battleHeroes;
            _topGun = tank.Battle.warrior;
            _invader = tank.Battle.invader;
            _sniper	= tank.Battle.sniper;
            _defender = tank.Battle.sniper;
            _steelWall = tank.Battle.steelwall;
            _confederate = tank.Battle.supporter;
            _scout = tank.Battle.scout;
            _patrolDuty = tank.Battle.evileye;
            _brothersInArms = tank.Epic.BrothersInArms;
            _crucialContribution = tank.Epic.CrucialContribution;
            _coolHeaded = tank.Special.alaric;
            _luckyDevil = tank.Special.luckyDevil;
            _spartan = tank.Special.sturdy;
        }
    }
}
