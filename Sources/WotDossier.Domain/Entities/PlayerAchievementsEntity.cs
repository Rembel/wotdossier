using System;
using System.Runtime.Serialization;
using WotDossier.Domain.Interfaces;

namespace WotDossier.Domain.Entities
{
    /// <summary>
    ///     Object representation for table 'PlayerAchievements'.
    /// </summary>
    [Serializable]
    [DataContract]
    public class PlayerAchievementsEntity : EntityBase, IRandomBattlesAchievements, IClanBattlesAchievements,
        IFortAchievements
    {
        /// <summary>
        ///     Gets/Sets the field "Warrior".
        /// </summary>
        [DataMember]
        public virtual int Warrior { get; set; }

        /// <summary>
        ///     Gets/Sets the field "Sniper".
        /// </summary>
        [DataMember]
        public virtual int Sniper { get; set; }

        /// <summary>
        ///     Gets/Sets the field "Sniper2".
        /// </summary>
        [DataMember]
        public virtual int Sniper2 { get; set; }

        /// <summary>
        ///     Gets/Sets the field "MainGun".
        /// </summary>
        [DataMember]
        public virtual int MainGun { get; set; }

        /// <summary>
        ///     Gets/Sets the field "Invader".
        /// </summary>
        [DataMember]
        public virtual int Invader { get; set; }

        /// <summary>
        ///     Gets/Sets the field "Defender".
        /// </summary>
        [DataMember]
        public virtual int Defender { get; set; }

        /// <summary>
        ///     Gets/Sets the field "SteelWall".
        /// </summary>
        [DataMember]
        public virtual int SteelWall { get; set; }

        /// <summary>
        ///     Gets/Sets the field "Confederate".
        /// </summary>
        [DataMember]
        public virtual int Confederate { get; set; }

        /// <summary>
        ///     Gets/Sets the field "Scout".
        /// </summary>
        [DataMember]
        public virtual int Scout { get; set; }

        /// <summary>
        ///     Gets/Sets the field "PatrolDuty".
        /// </summary>
        [DataMember]
        public virtual int PatrolDuty { get; set; }

        /// <summary>
        ///     Gets/Sets the field "HeroesOfRassenay".
        /// </summary>
        [DataMember]
        public virtual int HeroesOfRassenay { get; set; }

        /// <summary>
        ///     Gets/Sets the field "LafayettePool".
        /// </summary>
        [DataMember]
        public virtual int LafayettePool { get; set; }

        /// <summary>
        ///     Gets/Sets the field "RadleyWalters".
        /// </summary>
        [DataMember]
        public virtual int RadleyWalters { get; set; }

        /// <summary>
        ///     Gets/Sets the field "CrucialContribution".
        /// </summary>
        [DataMember]
        public virtual int CrucialContribution { get; set; }

        /// <summary>
        ///     Gets/Sets the field "BrothersInArms".
        /// </summary>
        [DataMember]
        public virtual int BrothersInArms { get; set; }

        /// <summary>
        ///     Gets/Sets the field "Kolobanov".
        /// </summary>
        [DataMember]
        public virtual int Kolobanov { get; set; }

        /// <summary>
        ///     Gets/Sets the field "Nikolas".
        /// </summary>
        [DataMember]
        public virtual int Nikolas { get; set; }

        /// <summary>
        ///     Gets/Sets the field "Orlik".
        /// </summary>
        [DataMember]
        public virtual int Orlik { get; set; }

        /// <summary>
        ///     Gets/Sets the field "Oskin".
        /// </summary>
        [DataMember]
        public virtual int Oskin { get; set; }

        /// <summary>
        ///     Gets/Sets the field "Halonen".
        /// </summary>
        [DataMember]
        public virtual int Halonen { get; set; }

        /// <summary>
        ///     Gets/Sets the field "Lehvaslaiho".
        /// </summary>
        [DataMember]
        public virtual int Lehvaslaiho { get; set; }

        /// <summary>
        ///     Gets/Sets the field "DeLanglade".
        /// </summary>
        [DataMember]
        public virtual int DeLanglade { get; set; }

        /// <summary>
        ///     Gets/Sets the field "Burda".
        /// </summary>
        [DataMember]
        public virtual int Burda { get; set; }

        /// <summary>
        ///     Gets/Sets the field "Dumitru".
        /// </summary>
        [DataMember]
        public virtual int Dumitru { get; set; }

        /// <summary>
        ///     Gets/Sets the field "Pascucci".
        /// </summary>
        [DataMember]
        public virtual int Pascucci { get; set; }

        /// <summary>
        ///     Gets/Sets the field "TamadaYoshio".
        /// </summary>
        [DataMember]
        public virtual int TamadaYoshio { get; set; }

        /// <summary>
        ///     Gets/Sets the field "Boelter".
        /// </summary>
        [DataMember]
        public virtual int Boelter { get; set; }

        /// <summary>
        ///     Gets/Sets the field "Fadin".
        /// </summary>
        [DataMember]
        public virtual int Fadin { get; set; }

        /// <summary>
        ///     Gets/Sets the field "Tarczay".
        /// </summary>
        [DataMember]
        public virtual int Tarczay { get; set; }

        /// <summary>
        ///     Gets/Sets the field "BrunoPietro".
        /// </summary>
        [DataMember]
        public virtual int BrunoPietro { get; set; }

        /// <summary>
        ///     Gets/Sets the field "Billotte".
        /// </summary>
        [DataMember]
        public virtual int Billotte { get; set; }

        /// <summary>
        ///     Gets/Sets the field "Survivor".
        /// </summary>
        [DataMember]
        public virtual int Survivor { get; set; }

        /// <summary>
        ///     Gets/Sets the field "Kamikaze".
        /// </summary>
        [DataMember]
        public virtual int Kamikaze { get; set; }

        /// <summary>
        ///     Gets/Sets the field "Invincible".
        /// </summary>
        [DataMember]
        public virtual int Invincible { get; set; }

        /// <summary>
        ///     Gets/Sets the field "Raider".
        /// </summary>
        [DataMember]
        public virtual int Raider { get; set; }

        /// <summary>
        ///     Gets/Sets the field "Bombardier".
        /// </summary>
        [DataMember]
        public virtual int Bombardier { get; set; }

        /// <summary>
        ///     Gets/Sets the field "Reaper".
        /// </summary>
        [DataMember]
        public virtual int Reaper { get; set; }

        /// <summary>
        ///     Gets/Sets the field "MouseTrap".
        /// </summary>
        [DataMember]
        public virtual int MouseTrap { get; set; }

        /// <summary>
        ///     Gets/Sets the field "PattonValley".
        /// </summary>
        [DataMember]
        public virtual int PattonValley { get; set; }

        /// <summary>
        ///     Gets/Sets the field "Hunter".
        /// </summary>
        [DataMember]
        public virtual int Hunter { get; set; }

        /// <summary>
        ///     Gets/Sets the field "Sinai".
        /// </summary>
        [DataMember]
        public virtual int Sinai { get; set; }

        /// <summary>
        ///     Gets/Sets the field "MasterGunnerLongest".
        /// </summary>
        [DataMember]
        public virtual int MasterGunnerLongest { get; set; }

        /// <summary>
        ///     Gets/Sets the field "SharpshooterLongest".
        /// </summary>
        [DataMember]
        public virtual int SharpshooterLongest { get; set; }

        /// <summary>
        ///     Gets/Sets the field "Jager".
        /// </summary>
        [DataMember]
        public virtual int Huntsman { get; set; }

        /// <summary>
        ///     Gets/Sets the field "IronMan".
        /// </summary>
        [DataMember]
        public virtual int IronMan { get; set; }

        /// <summary>
        ///     Gets/Sets the field "Sturdy".
        /// </summary>
        [DataMember]
        public virtual int Sturdy { get; set; }

        /// <summary>
        ///     Gets/Sets the field "LuckyDevil".
        /// </summary>
        [DataMember]
        public virtual int LuckyDevil { get; set; }

        /// <summary>
        ///     Gets/Sets the field "Kay".
        /// </summary>
        [DataMember]
        public virtual int Kay { get; set; }

        /// <summary>
        ///     Gets/Sets the field "Carius".
        /// </summary>
        [DataMember]
        public virtual int Carius { get; set; }

        /// <summary>
        ///     Gets/Sets the field "Knispel".
        /// </summary>
        [DataMember]
        public virtual int Knispel { get; set; }

        /// <summary>
        ///     Gets/Sets the field "Poppel".
        /// </summary>
        [DataMember]
        public virtual int Poppel { get; set; }

        /// <summary>
        ///     Gets/Sets the field "Abrams".
        /// </summary>
        [DataMember]
        public virtual int Abrams { get; set; }

        /// <summary>
        ///     Gets/Sets the field "Leclerk".
        /// </summary>
        [DataMember]
        public virtual int Leclerk { get; set; }

        /// <summary>
        ///     Gets/Sets the field "Lavrinenko".
        /// </summary>
        [DataMember]
        public virtual int Lavrinenko { get; set; }

        /// <summary>
        ///     Gets/Sets the field "Ekins".
        /// </summary>
        [DataMember]
        public virtual int Ekins { get; set; }

        /// <summary>
        ///     Gets/Sets the field "MarksOnGun".
        /// </summary>
        [DataMember]
        public virtual int MarksOnGun { get; set; }

        /// <summary>
        ///     Gets/Sets the field "MovingAvgDamage".
        /// </summary>
        [DataMember]
        public virtual int MovingAvgDamage { get; set; }

        /// <summary>
        ///     Gets/Sets the field "MedalMonolith".
        /// </summary>
        [DataMember]
        public virtual int MedalMonolith { get; set; }

        /// <summary>
        ///     Gets/Sets the field "MedalAntiSpgFire".
        /// </summary>
        [DataMember]
        public virtual int MedalAntiSpgFire { get; set; }

        /// <summary>
        ///     Gets/Sets the field "MedalGore".
        /// </summary>
        [DataMember]
        public virtual int MedalGore { get; set; }

        /// <summary>
        ///     Gets/Sets the field "MedalCoolBlood".
        /// </summary>
        [DataMember]
        public virtual int MedalCoolBlood { get; set; }

        /// <summary>
        ///     Gets/Sets the field "MedalStark".
        /// </summary>
        [DataMember]
        public virtual int MedalStark { get; set; }

        /// <summary>
        ///     Gets or sets the damage rating.
        /// </summary>
        [DataMember]
        public virtual int DamageRating { get; set; }

        /// <summary>
        ///     Gets or sets the battle hero.
        /// </summary>
        [DataMember]
        public virtual int BattleHero { get; set; }

        /// <summary>
        ///     Gets or sets the sharpshooter.
        /// </summary>
        [DataMember]
        public virtual int Sharpshooter { get; set; }

        /// <summary>
        ///     Gets or sets the reaper longest.
        /// </summary>
        [DataMember]
        public virtual int ReaperLongest { get; set; }

        /// <summary>
        ///     Gets or sets the reaper progress.
        /// </summary>
        [DataMember]
        public virtual int ReaperProgress { get; set; }

        /// <summary>
        ///     Gets or sets the sharpshooter progress.
        /// </summary>
        [DataMember]
        public virtual int SharpshooterProgress { get; set; }

        /// <summary>
        ///     Gets or sets the master gunner progress.
        /// </summary>
        [DataMember]
        public virtual int MasterGunnerProgress { get; set; }

        /// <summary>
        ///     Gets or sets the invincible longest.
        /// </summary>
        [DataMember]
        public virtual int InvincibleLongest { get; set; }

        /// <summary>
        ///     Gets or sets the invincible progress.
        /// </summary>
        [DataMember]
        public virtual int InvincibleProgress { get; set; }

        /// <summary>
        ///     Gets or sets the survivor longest.
        /// </summary>
        [DataMember]
        public virtual int SurvivorLongest { get; set; }

        /// <summary>
        ///     Gets or sets the survivor progress.
        /// </summary>
        [DataMember]
        public virtual int SurvivorProgress { get; set; }

        /// <summary>
        ///     Gets or sets the lumberjack.
        /// </summary>
        [DataMember]
        public virtual int Lumberjack { get; set; }

        /// <summary>
        ///     Gets or sets the master gunner.
        /// </summary>
        [DataMember]
        public virtual int MasterGunner { get; set; }

        /// <summary>
        ///     Gets or sets the alaric.
        /// </summary>
        [DataMember]
        public virtual int Alaric { get; set; }

        /// <summary>
        ///     Gets or sets the mark of mastery.
        /// </summary>
        [DataMember]
        public virtual int MarkOfMastery { get; set; }

        [DataMember]
        public virtual int Impenetrable { get; set; }

        [DataMember]
        public virtual int MaxAimerSeries { get; set; }

        [DataMember]
        public virtual int ShootToKill { get; set; }

        [DataMember]
        public virtual int Fighter { get; set; }

        [DataMember]
        public virtual int Duelist { get; set; }

        [DataMember]
        public virtual int Demolition { get; set; }

        [DataMember]
        public virtual int Arsonist { get; set; }

        [DataMember]
        public virtual int Bonecrusher { get; set; }

        [DataMember]
        public virtual int Charmed { get; set; }

        [DataMember]
        public virtual int Even { get; set; }

        #region

        [DataMember]
        public virtual int MedalRotmistrov { get; set; }

        [DataMember]
        public virtual int Conqueror { get; set; }

        [DataMember]
        public virtual int FireAndSword { get; set; }

        [DataMember]
        public virtual int Crusher { get; set; }

        [DataMember]
        public virtual int CounterBlow { get; set; }

        [DataMember]
        public virtual int SoldierOfFortune { get; set; }

        [DataMember]
        public virtual int Kampfer { get; set; }

        [DataMember]
        public virtual int CapturedBasesInAttack { get; set; }

        [DataMember]
        public virtual int CapturedBasesInDefence { get; set; }

        #endregion
    }
}