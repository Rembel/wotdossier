using System;
using WotDossier.Domain.Interfaces;

namespace WotDossier.Domain.Entities
{
    /// <summary>
    /// Object representation for table 'PlayerAchievements'.
    /// </summary>
	[Serializable]
    public class PlayerAchievementsEntity : EntityBase, IRandomBattlesAchievements, IClanBattlesAchievements, IFortAchievements
	{	
		/// <summary>
		/// Gets/Sets the field "Warrior".
		/// </summary>
		public virtual int Warrior	{get; set; }
		
		/// <summary>
		/// Gets/Sets the field "Sniper".
		/// </summary>
		public virtual int Sniper	{get; set; }

        /// <summary>
        /// Gets/Sets the field "Sniper2".
        /// </summary>
        public virtual int Sniper2	{get; set; }

        /// <summary>
        /// Gets/Sets the field "MainGun".
        /// </summary>
		public virtual int MainGun	{get; set; }
		
		/// <summary>
		/// Gets/Sets the field "Invader".
		/// </summary>
		public virtual int Invader	{get; set; }
		
		/// <summary>
		/// Gets/Sets the field "Defender".
		/// </summary>
		public virtual int Defender	{get; set; }
		
		/// <summary>
		/// Gets/Sets the field "SteelWall".
		/// </summary>
		public virtual int SteelWall	{get; set; }
		
		/// <summary>
		/// Gets/Sets the field "Confederate".
		/// </summary>
		public virtual int Confederate	{get; set; }
		
		/// <summary>
		/// Gets/Sets the field "Scout".
		/// </summary>
		public virtual int Scout	{get; set; }
		
		/// <summary>
		/// Gets/Sets the field "PatrolDuty".
		/// </summary>
		public virtual int PatrolDuty	{get; set; }
		
		/// <summary>
        /// Gets/Sets the field "HeroesOfRassenay".
		/// </summary>
		public virtual int HeroesOfRassenay	{get; set; }
		
		/// <summary>
		/// Gets/Sets the field "LafayettePool".
		/// </summary>
		public virtual int LafayettePool	{get; set; }
		
		/// <summary>
		/// Gets/Sets the field "RadleyWalters".
		/// </summary>
		public virtual int RadleyWalters	{get; set; }
		
		/// <summary>
		/// Gets/Sets the field "CrucialContribution".
		/// </summary>
		public virtual int CrucialContribution	{get; set; }
		
		/// <summary>
		/// Gets/Sets the field "BrothersInArms".
		/// </summary>
		public virtual int BrothersInArms	{get; set; }
		
		/// <summary>
		/// Gets/Sets the field "Kolobanov".
		/// </summary>
		public virtual int Kolobanov	{get; set; }
		
		/// <summary>
		/// Gets/Sets the field "Nikolas".
		/// </summary>
		public virtual int Nikolas	{get; set; }
		
		/// <summary>
		/// Gets/Sets the field "Orlik".
		/// </summary>
		public virtual int Orlik	{get; set; }
		
		/// <summary>
		/// Gets/Sets the field "Oskin".
		/// </summary>
		public virtual int Oskin	{get; set; }
		
		/// <summary>
		/// Gets/Sets the field "Halonen".
		/// </summary>
		public virtual int Halonen	{get; set; }
		
		/// <summary>
		/// Gets/Sets the field "Lehvaslaiho".
		/// </summary>
		public virtual int Lehvaslaiho	{get; set; }
		
		/// <summary>
		/// Gets/Sets the field "DeLanglade".
		/// </summary>
		public virtual int DeLanglade	{get; set; }
		
		/// <summary>
		/// Gets/Sets the field "Burda".
		/// </summary>
		public virtual int Burda	{get; set; }
		
		/// <summary>
		/// Gets/Sets the field "Dumitru".
		/// </summary>
		public virtual int Dumitru	{get; set; }
		
		/// <summary>
		/// Gets/Sets the field "Pascucci".
		/// </summary>
		public virtual int Pascucci	{get; set; }
		
		/// <summary>
		/// Gets/Sets the field "TamadaYoshio".
		/// </summary>
		public virtual int TamadaYoshio	{get; set; }
		
		/// <summary>
		/// Gets/Sets the field "Boelter".
		/// </summary>
		public virtual int Boelter	{get; set; }
		
		/// <summary>
		/// Gets/Sets the field "Fadin".
		/// </summary>
		public virtual int Fadin	{get; set; }
		
		/// <summary>
		/// Gets/Sets the field "Tarczay".
		/// </summary>
		public virtual int Tarczay	{get; set; }
		
		/// <summary>
		/// Gets/Sets the field "BrunoPietro".
		/// </summary>
		public virtual int BrunoPietro	{get; set; }
		
		/// <summary>
		/// Gets/Sets the field "Billotte".
		/// </summary>
		public virtual int Billotte	{get; set; }
		
		/// <summary>
		/// Gets/Sets the field "Survivor".
		/// </summary>
		public virtual int Survivor	{get; set; }
		
		/// <summary>
		/// Gets/Sets the field "Kamikaze".
		/// </summary>
		public virtual int Kamikaze	{get; set; }
		
		/// <summary>
		/// Gets/Sets the field "Invincible".
		/// </summary>
		public virtual int Invincible	{get; set; }
		
		/// <summary>
		/// Gets/Sets the field "Raider".
		/// </summary>
		public virtual int Raider	{get; set; }
		
		/// <summary>
		/// Gets/Sets the field "Bombardier".
		/// </summary>
		public virtual int Bombardier	{get; set; }
		
		/// <summary>
		/// Gets/Sets the field "Reaper".
		/// </summary>
		public virtual int Reaper	{get; set; }
		
		/// <summary>
		/// Gets/Sets the field "MouseTrap".
		/// </summary>
		public virtual int MouseTrap	{get; set; }
		
		/// <summary>
		/// Gets/Sets the field "PattonValley".
		/// </summary>
		public virtual int PattonValley	{get; set; }
		
		/// <summary>
		/// Gets/Sets the field "Hunter".
		/// </summary>
		public virtual int Hunter	{get; set; }
		
		/// <summary>
		/// Gets/Sets the field "Sinai".
		/// </summary>
		public virtual int Sinai	{get; set; }
		
		/// <summary>
		/// Gets/Sets the field "MasterGunnerLongest".
		/// </summary>
		public virtual int MasterGunnerLongest	{get; set; }
		
		/// <summary>
		/// Gets/Sets the field "SharpshooterLongest".
		/// </summary>
		public virtual int SharpshooterLongest	{get; set; }
		
		/// <summary>
		/// Gets/Sets the field "Jager".
		/// </summary>
        public virtual int Huntsman { get; set; }
		
		/// <summary>
		/// Gets/Sets the field "IronMan".
		/// </summary>
		public virtual int IronMan	{get; set; }
		
		/// <summary>
		/// Gets/Sets the field "Sturdy".
		/// </summary>
		public virtual int Sturdy	{get; set; }

        /// <summary>
        /// Gets/Sets the field "LuckyDevil".
        /// </summary>
        public virtual int LuckyDevil { get; set; }
		
		/// <summary>
        /// Gets/Sets the field "Kay".
		/// </summary>
        public virtual int Kay { get; set; }

        /// <summary>
        /// Gets/Sets the field "Carius".
        /// </summary>
        public virtual int Carius { get; set; }

        /// <summary>
        /// Gets/Sets the field "Knispel".
        /// </summary>
        public virtual int Knispel { get; set; }

        /// <summary>
        /// Gets/Sets the field "Poppel".
        /// </summary>
        public virtual int Poppel { get; set; }

        /// <summary>
        /// Gets/Sets the field "Abrams".
        /// </summary>
        public virtual int Abrams { get; set; }

        /// <summary>
        /// Gets/Sets the field "Leclerk".
        /// </summary>
        public virtual int Leclerk { get; set; }

        /// <summary>
        /// Gets/Sets the field "Lavrinenko".
        /// </summary>
        public virtual int Lavrinenko { get; set; }

        /// <summary>
        /// Gets/Sets the field "Ekins".
        /// </summary>
        public virtual int Ekins { get; set; }

        /// <summary>
        /// Gets/Sets the field "MarksOnGun".
        /// </summary>
        public virtual int MarksOnGun { get; set; }

        /// <summary>
        /// Gets/Sets the field "MovingAvgDamage".
        /// </summary>
        public virtual int MovingAvgDamage { get; set; }

        /// <summary>
        /// Gets/Sets the field "MedalMonolith".
        /// </summary>
        public virtual int MedalMonolith { get; set; }

        /// <summary>
        /// Gets/Sets the field "MedalAntiSpgFire".
        /// </summary>
        public virtual int MedalAntiSpgFire { get; set; }

        /// <summary>
        /// Gets/Sets the field "MedalGore".
        /// </summary>
        public virtual int MedalGore { get; set; }

        /// <summary>
        /// Gets/Sets the field "MedalCoolBlood".
        /// </summary>
        public virtual int MedalCoolBlood { get; set; }

        /// <summary>
        /// Gets/Sets the field "MedalStark".
        /// </summary>
        public virtual int MedalStark { get; set; }

        /// <summary>
        /// Gets or sets the damage rating.
        /// </summary>
	    public virtual int DamageRating { get; set; }

        /// <summary>
        /// Gets or sets the battle hero.
        /// </summary>
	    public virtual int BattleHero { get; set; }
        
        /// <summary>
        /// Gets or sets the sharpshooter.
        /// </summary>
        public virtual int Sharpshooter { get; set; }
        
        /// <summary>
        /// Gets or sets the reaper longest.
        /// </summary>
        public virtual int ReaperLongest { get; set; }
        
        /// <summary>
        /// Gets or sets the reaper progress.
        /// </summary>
        public virtual int ReaperProgress { get; set; }
        
        /// <summary>
        /// Gets or sets the sharpshooter progress.
        /// </summary>
        public virtual int SharpshooterProgress { get; set; }
        
        /// <summary>
        /// Gets or sets the master gunner progress.
        /// </summary>
        public virtual int MasterGunnerProgress { get; set; }
        
        /// <summary>
        /// Gets or sets the invincible longest.
        /// </summary>
        public virtual int InvincibleLongest { get; set; }
        
        /// <summary>
        /// Gets or sets the invincible progress.
        /// </summary>
        public virtual int InvincibleProgress { get; set; }
        
        /// <summary>
        /// Gets or sets the survivor longest.
        /// </summary>
        public virtual int SurvivorLongest { get; set; }

        /// <summary>
        /// Gets or sets the survivor progress.
        /// </summary>
        public virtual int SurvivorProgress { get; set; }

        /// <summary>
        /// Gets or sets the lumberjack.
        /// </summary>
        public virtual int Lumberjack { get; set; }

        /// <summary>
        /// Gets or sets the master gunner.
        /// </summary>
        public virtual int MasterGunner { get; set; }

        /// <summary>
        /// Gets or sets the alaric.
        /// </summary>
        public virtual int Alaric { get; set; }

        /// <summary>
        /// Gets or sets the mark of mastery.
        /// </summary>
        public virtual int MarkOfMastery { get; set; }

        #region 

        public virtual int Conqueror { get; set; }

        public virtual int FireAndSword { get; set; }

        public virtual int Crusher { get; set; }

        public virtual int CounterBlow { get; set; }

        public virtual int SoldierOfFortune { get; set; }

        public virtual int Kampfer { get; set; }

        public virtual int MedalRotmistrov { get; set; }

        public virtual int CapturedBasesInAttack { get; set; }

        public virtual int CapturedBasesInDefence { get; set; }

        #endregion


        public virtual int Impenetrable { get; set; }
        public virtual int MaxAimerSeries { get; set; }
        public virtual int ShootToKill { get; set; }
        public virtual int Fighter { get; set; }
        public virtual int Duelist { get; set; }
        public virtual int Demolition { get; set; }
        public virtual int Arsonist { get; set; }
        public virtual int Bonecrusher { get; set; }
        public virtual int Charmed { get; set; }
        public virtual int Even { get; set; }
	}
}

