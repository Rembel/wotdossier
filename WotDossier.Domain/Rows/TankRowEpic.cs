namespace WotDossier.Domain.Rows
{
    public class TankRowEpic : TankRowBase
    {
        private int _boelter;
        private int _radleyWalters;
        private int _lafayettePool;
        private int _orlik;
        private int _oskin;
        private int _lehvaslaiho;
        private int _nikolas;
        private int _halonen;
        private int _burda;
        private int _pascucci;
        private int _dumitru;
        private int _tamadaYoshio;
        private int _billotte;
        private int _brunoPietro;
        private int _tarczay;
        private int _kolobanov;
        private int _fadin;
        private int _heroesOfRaseiniai;
        private int _deLanglade;

        public TankRowEpic(Tank tank)
        {
            Tier = tank.Common.tier;
            Tank = tank.Name;
            Icon = tank.TankContour;
            _boelter = tank.Epic.Boelter;
            _radleyWalters = tank.Epic.RadleyWalters;
            _lafayettePool = tank.Epic.LafayettePool;
            _orlik = tank.Epic.Orlik;
            _oskin = tank.Epic.Oskin;
            _lehvaslaiho = tank.Epic.Lehvaslaiho;
            _nikolas = tank.Epic.Nikolas;
            _halonen = tank.Epic.Halonen;
            _burda = tank.Epic.Burda;
            _pascucci = tank.Epic.Pascucci;
            _dumitru = tank.Epic.Dumitru;
            _tamadaYoshio = tank.Epic.TamadaYoshio;
            _billotte = tank.Epic.Billotte;
            _brunoPietro = tank.Epic.BrunoPietro;
            _tarczay = tank.Epic.Tarczay;
            _kolobanov = tank.Epic.Kolobanov;
            _fadin = tank.Epic.Fadin;
            _heroesOfRaseiniai = tank.Special.heroesOfRassenay;
            //TODO: fix DeLanglade medal load
            _deLanglade = tank.Epic.DeLanglade;
        }

        public int Boelter
        {
            get { return _boelter; }
            set { _boelter = value; }
        }

        public int RadleyWalters
        {
            get { return _radleyWalters; }
            set { _radleyWalters = value; }
        }

        public int LafayettePool
        {
            get { return _lafayettePool; }
            set { _lafayettePool = value; }
        }

        public int Orlik
        {
            get { return _orlik; }
            set { _orlik = value; }
        }

        public int Oskin
        {
            get { return _oskin; }
            set { _oskin = value; }
        }

        public int Lehvaslaiho
        {
            get { return _lehvaslaiho; }
            set { _lehvaslaiho = value; }
        }

        public int Nikolas
        {
            get { return _nikolas; }
            set { _nikolas = value; }
        }

        public int Halonen
        {
            get { return _halonen; }
            set { _halonen = value; }
        }

        public int Burda
        {
            get { return _burda; }
            set { _burda = value; }
        }

        public int Pascucci
        {
            get { return _pascucci; }
            set { _pascucci = value; }
        }

        public int Dumitru
        {
            get { return _dumitru; }
            set { _dumitru = value; }
        }

        public int TamadaYoshio
        {
            get { return _tamadaYoshio; }
            set { _tamadaYoshio = value; }
        }

        public int Billotte
        {
            get { return _billotte; }
            set { _billotte = value; }
        }

        public int BrunoPietro
        {
            get { return _brunoPietro; }
            set { _brunoPietro = value; }
        }

        public int Tarczay
        {
            get { return _tarczay; }
            set { _tarczay = value; }
        }

        public int Kolobanov
        {
            get { return _kolobanov; }
            set { _kolobanov = value; }
        }

        public int Fadin
        {
            get { return _fadin; }
            set { _fadin = value; }
        }

        public int HeroesOfRaseiniai
        {
            get { return _heroesOfRaseiniai; }
            set { _heroesOfRaseiniai = value; }
        }

        public int DeLanglade
        {
            get { return _deLanglade; }
            set { _deLanglade = value; }
        }
    }
}
