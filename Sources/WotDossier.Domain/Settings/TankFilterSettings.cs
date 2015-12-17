namespace WotDossier.Domain
{
    /// <summary>
    /// Provides the setting for tank filtering.
    /// </summary>
    public class TankFilterSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TankFilterSettings" /> class.
        /// </summary>
        public TankFilterSettings()
        {
            UKSelected = true;
            FranceSelected = true;
            ChinaSelected = true;
            GermanySelected = true;
            USSelected = true;
            USSRSelected = true;
            JPSelected = true;
            CZSelected = true;
            LTSelected = true;
            MTSelected = true;
            HTSelected = true;
            TDSelected = true;
            SPGSelected = true;
            Level1Selected = true;
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

        /// <summary>
        /// Gets or sets a value indicating whether [level10 selected].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [level10 selected]; otherwise, <c>false</c>.
        /// </value>
        public bool Level10Selected { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [level9 selected].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [level9 selected]; otherwise, <c>false</c>.
        /// </value>
        public bool Level9Selected { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [level8 selected].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [level8 selected]; otherwise, <c>false</c>.
        /// </value>
        public bool Level8Selected { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [level7 selected].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [level7 selected]; otherwise, <c>false</c>.
        /// </value>
        public bool Level7Selected { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [level6 selected].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [level6 selected]; otherwise, <c>false</c>.
        /// </value>
        public bool Level6Selected { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [level5 selected].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [level5 selected]; otherwise, <c>false</c>.
        /// </value>
        public bool Level5Selected { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [level4 selected].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [level4 selected]; otherwise, <c>false</c>.
        /// </value>
        public bool Level4Selected { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [level3 selected].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [level3 selected]; otherwise, <c>false</c>.
        /// </value>
        public bool Level3Selected { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [level2 selected].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [level2 selected]; otherwise, <c>false</c>.
        /// </value>
        public bool Level2Selected { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [level1 selected].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [level1 selected]; otherwise, <c>false</c>.
        /// </value>
        public bool Level1Selected { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [SPG selected].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [SPG selected]; otherwise, <c>false</c>.
        /// </value>
        public bool SPGSelected { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [TD selected].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [TD selected]; otherwise, <c>false</c>.
        /// </value>
        public bool TDSelected { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [HT selected].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [HT selected]; otherwise, <c>false</c>.
        /// </value>
        public bool HTSelected { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [MT selected].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [MT selected]; otherwise, <c>false</c>.
        /// </value>
        public bool MTSelected { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [LT selected].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [LT selected]; otherwise, <c>false</c>.
        /// </value>
        public bool LTSelected { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [USSR selected].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [USSR selected]; otherwise, <c>false</c>.
        /// </value>
        public bool USSRSelected { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [germany selected].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [germany selected]; otherwise, <c>false</c>.
        /// </value>
        public bool GermanySelected { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [US selected].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [US selected]; otherwise, <c>false</c>.
        /// </value>
        public bool USSelected { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [china selected].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [china selected]; otherwise, <c>false</c>.
        /// </value>
        public bool ChinaSelected { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [france selected].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [france selected]; otherwise, <c>false</c>.
        /// </value>
        public bool FranceSelected { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [UK selected].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [UK selected]; otherwise, <c>false</c>.
        /// </value>
        public bool UKSelected { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [JP selected].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [JP selected]; otherwise, <c>false</c>.
        /// </value>
        public bool JPSelected { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [cz selected].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [cz selected]; otherwise, <c>false</c>.
        /// </value>
        public bool CZSelected { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is premium.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is premium; otherwise, <c>false</c>.
        /// </value>
        public bool IsPremium { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is favorite.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is favorite; otherwise, <c>false</c>.
        /// </value>
        public bool IsFavorite { get; set; }
    }
}