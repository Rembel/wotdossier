namespace WotDossier.Domain
{
    public class TankFilterSettings
    {
        public TankFilterSettings()
        {
            UKSelected = true;
            FranceSelected = true;
            ChinaSelected = true;
            GermanySelected = true;
            USSelected = true;
            USSRSelected = true;
            LTSelected = true;
            MTSelected = true;
            HTSelected = true;
            TDSelected = true;
            SPGSelected = true;
            Level2Selected = true;
            Level3Selected = true;
            Level4Selected = true;
            Level5Selected = true;
            Level6Selected = true;
            Level7Selected = true;
            Level8Selected = true;
            Level9Selected = true;
            Level10Selected = true;
        }

        public bool Level10Selected { get; set; }

        public bool Level9Selected { get; set; }

        public bool Level8Selected { get; set; }

        public bool Level7Selected { get; set; }

        public bool Level6Selected { get; set; }

        public bool Level5Selected { get; set; }

        public bool Level4Selected { get; set; }

        public bool Level3Selected { get; set; }

        public bool Level2Selected { get; set; }

        public bool Level1Selected { get; set; }

        public bool SPGSelected { get; set; }

        public bool TDSelected { get; set; }

        public bool HTSelected { get; set; }

        public bool MTSelected { get; set; }

        public bool LTSelected { get; set; }

        public bool USSRSelected { get; set; }

        public bool GermanySelected { get; set; }

        public bool USSelected { get; set; }

        public bool ChinaSelected { get; set; }

        public bool FranceSelected { get; set; }

        public bool UKSelected { get; set; }

        public bool IsPremium { get; set; }

        public bool IsFavorite { get; set; }
    }
}