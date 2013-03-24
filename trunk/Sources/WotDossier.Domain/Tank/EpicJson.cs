namespace WotDossier.Domain.Tank
{
    public class EpicJson {
        private int _billotte;
        private int _brothersInArms;
        private int _brunoPietro;
        private int _burda;
        private int _crucialContribution;
        private int _deLanglade;
        private int _dumitru;
        private int _fadin;
        private int _halonen;
        private int _kolobanov;
        private int _lafayettePool;
        private int _lehvaslaiho;
        private int _nikolas;
        private int _orlik;
        private int _oskin;
        private int _pascucci;
        private int _radleyWalters;
        private int _tarczay;
        private int _wittmann;
        private int _tamadaYoshio;
        private int _boelter;

        public int medalBillotte;
        public int medalBrothersInArms;
        public int medalBrunoPietro;
        public int medalBurda;
        public int medalCrucialContribution;
        public int medalDumitru;
        public int medalFadin;
        public int medalHalonen;
        public int medalKolobanov;
        public int medalLafayettePool;
        public int medalLehvaslaiho;
        public int medalNikolas;
        public int medalOrlik;
        public int medalOskin;
        public int medalPascucci;
        public int medalRadleyWalters;
        public int medalTarczay;
        public int medalWittmann;
        public int medalDeLanglade;
        public int medalTamadaYoshio;

        public int Billotte
        {
            get { return GetActualMedalValue(_billotte, medalBillotte); }
            set { _billotte = value; }
        }

        public int BrothersInArms
        {
            get { return GetActualMedalValue(_brothersInArms, medalBrothersInArms); }
            set { _brothersInArms = value; }
        }

        public int BrunoPietro
        {
            get { return GetActualMedalValue(_brunoPietro, medalBrunoPietro); }
            set { _brunoPietro = value; }
        }

        public int Burda
        {
            get { return GetActualMedalValue(_burda, medalBurda); }
            set { _burda = value; }
        }

        public int CrucialContribution
        {
            get { return GetActualMedalValue(_crucialContribution, medalCrucialContribution); }
            set { _crucialContribution = value; }
        }

        public int Dumitru
        {
            get { return GetActualMedalValue(_dumitru, medalDumitru); }
            set { _dumitru = value; }
        }

        public int Fadin
        {
            get { return GetActualMedalValue(_fadin, medalFadin); }
            set { _fadin = value; }
        }

        public int Halonen
        {
            get { return GetActualMedalValue(_halonen, medalHalonen); }
            set { _halonen = value; }
        }

        public int Kolobanov
        {
            get { return GetActualMedalValue(_kolobanov, medalKolobanov); }
            set { _kolobanov = value; }
        }

        public int LafayettePool
        {
            get { return GetActualMedalValue(_lafayettePool, medalLafayettePool); }
            set { _lafayettePool = value; }
        }

        public int Lehvaslaiho
        {
            get { return GetActualMedalValue(_lehvaslaiho, medalLehvaslaiho); }
            set { _lehvaslaiho = value; }
        }

        public int Nikolas
        {
            get { return GetActualMedalValue(_nikolas, medalNikolas); }
            set { _nikolas = value; }
        }

        public int Orlik
        {
            get { return GetActualMedalValue(_orlik, medalOrlik); }
            set { _orlik = value; }
        }

        public int Oskin
        {
            get { return GetActualMedalValue(_oskin, medalOskin); }
            set { _oskin = value; }
        }

        public int Pascucci
        {
            get { return GetActualMedalValue(_pascucci, medalPascucci); }
            set { _pascucci = value; }
        }

        public int RadleyWalters
        {
            get { return GetActualMedalValue(_radleyWalters, medalRadleyWalters); }
            set { _radleyWalters = value; }
        }

        public int Tarczay
        {
            get { return GetActualMedalValue(_tarczay, medalTarczay); }
            set { _tarczay = value; }
        }

        public int Wittmann
        {
            get { return GetActualMedalValue(_wittmann, medalWittmann); }
            set { _wittmann = value; }
        }

        public int DeLanglade
        {
            get { return GetActualMedalValue(_deLanglade, medalDeLanglade); }
            set { _deLanglade = value; }
        }

        public int TamadaYoshio
        {
            get { return GetActualMedalValue(_tamadaYoshio, medalTamadaYoshio); }
            set { _tamadaYoshio = value; }
        }

        public int Boelter
        {
            get { return _boelter; }
            set { _boelter = value; }
        }

        private int GetActualMedalValue(int oldFieldValue, int newFieldValue)
        {
            return oldFieldValue == 0 ? newFieldValue : oldFieldValue;
        }
    }
}