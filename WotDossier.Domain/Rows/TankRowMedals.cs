using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WotDossier.Domain.Rows
{
    public class TankRowMedals
    {
        private int _tier;
        private int _icon;
        private string _tank;
        private int _kay;
        private int _carius;
        private int _knispel;
        private int _poppel;
        private int _abrams;
        private int _leclerk;
        private int _lavrinenko;
        private int _ekins;

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

        public int Kay
        {
            get { return _kay; }
            set { _kay = value; }
        }

        public int Carius
        {
            get { return _carius; }
            set { _carius = value; }
        }

        public int Knispel
        {
            get { return _knispel; }
            set { _knispel = value; }
        }

        public int Poppel
        {
            get { return _poppel; }
            set { _poppel = value; }
        }

        public int Abrams
        {
            get { return _abrams; }
            set { _abrams = value; }
        }

        public int Leclerk
        {
            get { return _leclerk; }
            set { _leclerk = value; }
        }

        public int Lavrinenko
        {
            get { return _lavrinenko; }
            set { _lavrinenko = value; }
        }

        public int Ekins
        {
            get { return _ekins; }
            set { _ekins = value; }
        }
        
        public TankRowMedals(Tank tank)
        {
            _tier = tank.Common.tier;
            _tank = tank.Name;
            _kay = tank.Major.Kay;
            _carius = tank.Major.Carius;
            _knispel = tank.Major.Knispel;
            _poppel = tank.Major.Poppel;
            _abrams = tank.Major.Abrams;
            _leclerk = tank.Major.LeClerc;
            _lavrinenko = tank.Major.Lavrinenko;
            _ekins = tank.Major.Ekins;
        }
    }
}
