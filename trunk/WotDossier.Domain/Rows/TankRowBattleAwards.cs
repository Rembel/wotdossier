﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WotDossier.Domain.Rows
{
    public class TankRowBattleAwards
    {
        private int _tier;
        private int _icon;
        private string _tank;
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

        public int Tier
        {
            get { return _tier; }
            set { _tier = value; }
        }

        public int Icon
        {
            get { return _icon; }
            set { _icon = value; }
        }

        public string Tank
        {
            get { return _tank; }
            set { _tank = value; }
        }

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

        public TankRowBattleAwards(Tank tank)
        {
            _tier = tank.Common.tier;
            _tank = tank.Name;
            _battleHero = tank.Battle.battleHeroes;
            _topGun = tank.Battle.warrior;
            _invader = tank.Battle.invader;
            _sniper	= tank.Battle.sniper;
            _defender = tank.Battle.sniper;
            _steelWall = tank.Battle.steelwall;
            _confederate = tank.Battle.supporter;
            _scout = tank.Battle.scout;
            _patrolDuty = tank.Battle.evileye;
            _brothersInArms = tank.Epic.BrothersInArms == 0 ? tank.Epic.medalBrothersInArms : tank.Epic.BrothersInArms;
            _crucialContribution = tank.Epic.CrucialContribution == 0 ? tank.Epic.medalCrucialContribution : tank.Epic.CrucialContribution;
        }
    }
}