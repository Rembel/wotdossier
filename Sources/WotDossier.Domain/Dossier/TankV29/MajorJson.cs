namespace WotDossier.Domain.Dossier.TankV29
{
    public class MajorJson29
    {
        private int _abrams;
        private int _carius;
        private int _ekins;
        private int _kay;
        private int _knispel;
        private int _lavrinenko;
        private int _leClerc;
        private int _poppel;
        public int medalAbrams;
        public int medalCarius;
        public int medalEkins;
        public int medalKay;
        public int medalKnispel;
        public int medalLavrinenko;
        public int medalLeClerc;
        public int medalPoppel;

        public int Abrams
        {
            get { return _abrams == 0 ? medalAbrams : _abrams; }
            set { _abrams = value; }
        }

        public int Carius
        {
            get { return _carius == 0 ? medalCarius : _carius; }
            set { _carius = value; }
        }

        public int Ekins
        {
            get { return _ekins == 0 ? medalEkins : _ekins; }
            set { _ekins = value; }
        }

        public int Kay
        {
            get { return _kay == 0 ? medalKay : _kay; }
            set { _kay = value; }
        }

        public int Knispel
        {
            get { return _knispel == 0 ? medalKnispel : _knispel; }
            set { _knispel = value; }
        }

        public int Lavrinenko
        {
            get { return _lavrinenko == 0 ? medalLavrinenko : _lavrinenko; }
            set { _lavrinenko = value; }
        }

        public int LeClerc
        {
            get { return _leClerc == 0 ? medalLeClerc : _leClerc; }
            set { _leClerc = value; }
        }

        public int Poppel
        {
            get { return _poppel == 0 ? medalPoppel : _poppel; }
            set { _poppel = value; }
        }
    }
}