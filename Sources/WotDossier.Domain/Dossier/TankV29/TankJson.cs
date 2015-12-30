using System.Collections.Generic;

namespace WotDossier.Domain.Dossier.TankV29
{
    public class TankJson29
    {
        private IList<IList<string>> _kills = new List<IList<string>>();
        private SpecialJson29 _special = new SpecialJson29();
        private MajorJson29 _major = new MajorJson29();
        private BattleJson29 _battle = new BattleJson29();
        private SeriesJson29 _series = new SeriesJson29();
        private EpicJson29 _epic = new EpicJson29();

        public BattleJson29 Battle
        {
            get { return _battle; }
            set { _battle = value; }
        }

        public ClanJson29 Clan { get; set; }

        public CommonJson29 Common { get; set; }
        
        public CompanyJson29 Company { get; set; }

        public EpicJson29 Epic
        {
            get { return _epic; }
            set { _epic = value; }
        }

        public SeriesJson29 Series
        {
            get { return _series; }
            set { _series = value; }
        }

        public MajorJson29 Major
        {
            get { return _major; }
            set { _major = value; }
        }

        public IList<IList<string>> Kills
        {
            get { return _kills; }
            set { _kills = value; }
        }

        public SpecialJson29 Special
        {
            get { return _special; }
            set { _special = value; }
        }

        public StatisticJson29 Tankdata { get; set; }

        private int _uniqueId = -1;
        public int UniqueId()
        {
            if (_uniqueId == -1)
            {
                _uniqueId = DossierUtils.ToUniqueId(Common.countryid, Common.tankid);
            }
            return _uniqueId;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0}", Common.tanktitle);
        }
    }
}
