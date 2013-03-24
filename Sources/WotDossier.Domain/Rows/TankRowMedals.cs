using WotDossier.Domain.Tank;

namespace WotDossier.Domain.Rows
{
    public class TankRowMedals : TankRowBase, ITankRowMedals
    {
        private int _kay;
        private int _carius;
        private int _knispel;
        private int _poppel;
        private int _abrams;
        private int _leclerk;
        private int _lavrinenko;
        private int _ekins;

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

        public TankRowMedals(TankJson tank)
            : base(tank)
        {
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
