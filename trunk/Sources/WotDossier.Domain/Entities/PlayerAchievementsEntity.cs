using System;
using System.Collections.Generic;
using WotDossier.Common;

namespace WotDossier.Domain.Entities
{
	/// <summary>
	/// Object representation for table 'PlayerAchievements'.
	/// </summary>
	[Serializable]
	public class PlayerAchievementsEntity : EntityBase
	{	
		#region Property names
		
		public static readonly string PropWarrior = TypeHelper<PlayerAchievementsEntity>.PropertyName(v => v.Warrior);
		public static readonly string PropSniper = TypeHelper<PlayerAchievementsEntity>.PropertyName(v => v.Sniper);
		public static readonly string PropInvader = TypeHelper<PlayerAchievementsEntity>.PropertyName(v => v.Invader);
		public static readonly string PropDefender = TypeHelper<PlayerAchievementsEntity>.PropertyName(v => v.Defender);
		public static readonly string PropSteelWall = TypeHelper<PlayerAchievementsEntity>.PropertyName(v => v.SteelWall);
		public static readonly string PropConfederate = TypeHelper<PlayerAchievementsEntity>.PropertyName(v => v.Confederate);
		public static readonly string PropScout = TypeHelper<PlayerAchievementsEntity>.PropertyName(v => v.Scout);
		public static readonly string PropPatrolDuty = TypeHelper<PlayerAchievementsEntity>.PropertyName(v => v.PatrolDuty);
		public static readonly string PropHeroesOfRaseiniai = TypeHelper<PlayerAchievementsEntity>.PropertyName(v => v.HeroesOfRaseiniai);
		public static readonly string PropLafayettePool = TypeHelper<PlayerAchievementsEntity>.PropertyName(v => v.LafayettePool);
		public static readonly string PropRadleyWalters = TypeHelper<PlayerAchievementsEntity>.PropertyName(v => v.RadleyWalters);
		public static readonly string PropCrucialContribution = TypeHelper<PlayerAchievementsEntity>.PropertyName(v => v.CrucialContribution);
		public static readonly string PropBrothersInArms = TypeHelper<PlayerAchievementsEntity>.PropertyName(v => v.BrothersInArms);
		public static readonly string PropKolobanov = TypeHelper<PlayerAchievementsEntity>.PropertyName(v => v.Kolobanov);
		public static readonly string PropNikolas = TypeHelper<PlayerAchievementsEntity>.PropertyName(v => v.Nikolas);
		public static readonly string PropOrlik = TypeHelper<PlayerAchievementsEntity>.PropertyName(v => v.Orlik);
		public static readonly string PropOskin = TypeHelper<PlayerAchievementsEntity>.PropertyName(v => v.Oskin);
		public static readonly string PropHalonen = TypeHelper<PlayerAchievementsEntity>.PropertyName(v => v.Halonen);
		public static readonly string PropLehvaslaiho = TypeHelper<PlayerAchievementsEntity>.PropertyName(v => v.Lehvaslaiho);
		public static readonly string PropDeLanglade = TypeHelper<PlayerAchievementsEntity>.PropertyName(v => v.DeLanglade);
		public static readonly string PropBurda = TypeHelper<PlayerAchievementsEntity>.PropertyName(v => v.Burda);
		public static readonly string PropDumitru = TypeHelper<PlayerAchievementsEntity>.PropertyName(v => v.Dumitru);
		public static readonly string PropPascucci = TypeHelper<PlayerAchievementsEntity>.PropertyName(v => v.Pascucci);
		public static readonly string PropTamadaYoshio = TypeHelper<PlayerAchievementsEntity>.PropertyName(v => v.TamadaYoshio);
		public static readonly string PropBoelter = TypeHelper<PlayerAchievementsEntity>.PropertyName(v => v.Boelter);
		public static readonly string PropFadin = TypeHelper<PlayerAchievementsEntity>.PropertyName(v => v.Fadin);
		public static readonly string PropTarczay = TypeHelper<PlayerAchievementsEntity>.PropertyName(v => v.Tarczay);
		public static readonly string PropBrunoPietro = TypeHelper<PlayerAchievementsEntity>.PropertyName(v => v.BrunoPietro);
		public static readonly string PropBillotte = TypeHelper<PlayerAchievementsEntity>.PropertyName(v => v.Billotte);
		public static readonly string PropSurvivor = TypeHelper<PlayerAchievementsEntity>.PropertyName(v => v.Survivor);
		public static readonly string PropKamikaze = TypeHelper<PlayerAchievementsEntity>.PropertyName(v => v.Kamikaze);
		public static readonly string PropInvincible = TypeHelper<PlayerAchievementsEntity>.PropertyName(v => v.Invincible);
		public static readonly string PropRaider = TypeHelper<PlayerAchievementsEntity>.PropertyName(v => v.Raider);
		public static readonly string PropBombardier = TypeHelper<PlayerAchievementsEntity>.PropertyName(v => v.Bombardier);
		public static readonly string PropReaper = TypeHelper<PlayerAchievementsEntity>.PropertyName(v => v.Reaper);
		public static readonly string PropMouseTrap = TypeHelper<PlayerAchievementsEntity>.PropertyName(v => v.MouseTrap);
		public static readonly string PropPattonValley = TypeHelper<PlayerAchievementsEntity>.PropertyName(v => v.PattonValley);
		public static readonly string PropHunter = TypeHelper<PlayerAchievementsEntity>.PropertyName(v => v.Hunter);
		public static readonly string PropSinai = TypeHelper<PlayerAchievementsEntity>.PropertyName(v => v.Sinai);
		public static readonly string PropMasterGunnerLongest = TypeHelper<PlayerAchievementsEntity>.PropertyName(v => v.MasterGunnerLongest);
		public static readonly string PropSharpshooterLongest = TypeHelper<PlayerAchievementsEntity>.PropertyName(v => v.SharpshooterLongest);
		public static readonly string PropJager = TypeHelper<PlayerAchievementsEntity>.PropertyName(v => v.Jager);
		public static readonly string PropCoolHeaded = TypeHelper<PlayerAchievementsEntity>.PropertyName(v => v.CoolHeaded);
		public static readonly string PropSpartan = TypeHelper<PlayerAchievementsEntity>.PropertyName(v => v.Spartan);
		public static readonly string PropLuckyDevil = TypeHelper<PlayerAchievementsEntity>.PropertyName(v => v.LuckyDevil);
		
		#endregion

		/// <summary>
		/// Gets/Sets the field "Warrior".
		/// </summary>
		public virtual int Warrior	{get; set; }
		
		/// <summary>
		/// Gets/Sets the field "Sniper".
		/// </summary>
		public virtual int Sniper	{get; set; }
		
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
		/// Gets/Sets the field "HeroesOfRaseiniai".
		/// </summary>
		public virtual int HeroesOfRaseiniai	{get; set; }
		
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
		public virtual int Jager	{get; set; }
		
		/// <summary>
		/// Gets/Sets the field "CoolHeaded".
		/// </summary>
		public virtual int CoolHeaded	{get; set; }
		
		/// <summary>
		/// Gets/Sets the field "Spartan".
		/// </summary>
		public virtual int Spartan	{get; set; }
		
		/// <summary>
		/// Gets/Sets the field "LuckyDevil".
		/// </summary>
		public virtual int LuckyDevil	{get; set; }
		
		#region Collections
		
		private IList<PlayerStatisticEntity> _playerStatisticEntities;
		/// <summary>
		/// Gets/Sets the <see cref="PlayerStatisticEntity"/> collection.
		/// </summary>
        public virtual IList<PlayerStatisticEntity> PlayerStatisticEntities
        {
            get
            {
                return _playerStatisticEntities ?? (_playerStatisticEntities = new List<PlayerStatisticEntity>());
            }
            set { _playerStatisticEntities = value; }
        }
		
		#endregion Collections
		
	}
}

