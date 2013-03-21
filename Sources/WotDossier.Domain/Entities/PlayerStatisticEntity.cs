using System;
using System.Collections.Generic;
using WotDossier.Common;
using WotDossier.Domain.Player;
using System.Linq;

namespace WotDossier.Domain.Entities
{
	/// <summary>
	/// Object representation for table 'PlayerStatistic'.
	/// </summary>
	[Serializable]
	public class PlayerStatisticEntity : EntityBase
	{	
		#region Property names
		
		public static readonly string PropUpdated = TypeHelper<PlayerStatisticEntity>.PropertyName(v => v.Updated);
		public static readonly string PropWins = TypeHelper<PlayerStatisticEntity>.PropertyName(v => v.Wins);
		public static readonly string PropLosses = TypeHelper<PlayerStatisticEntity>.PropertyName(v => v.Losses);
		public static readonly string PropSurvivedBattles = TypeHelper<PlayerStatisticEntity>.PropertyName(v => v.SurvivedBattles);
		public static readonly string PropXp = TypeHelper<PlayerStatisticEntity>.PropertyName(v => v.Xp);
		public static readonly string PropBattleAvgXp = TypeHelper<PlayerStatisticEntity>.PropertyName(v => v.BattleAvgXp);
		public static readonly string PropMaxXp = TypeHelper<PlayerStatisticEntity>.PropertyName(v => v.MaxXp);
		public static readonly string PropFrags = TypeHelper<PlayerStatisticEntity>.PropertyName(v => v.Frags);
		public static readonly string PropSpotted = TypeHelper<PlayerStatisticEntity>.PropertyName(v => v.Spotted);
		public static readonly string PropHitsPercents = TypeHelper<PlayerStatisticEntity>.PropertyName(v => v.HitsPercents);
		public static readonly string PropDamageDealt = TypeHelper<PlayerStatisticEntity>.PropertyName(v => v.DamageDealt);
		public static readonly string PropCapturePoints = TypeHelper<PlayerStatisticEntity>.PropertyName(v => v.CapturePoints);
		public static readonly string PropDroppedCapturePoints = TypeHelper<PlayerStatisticEntity>.PropertyName(v => v.DroppedCapturePoints);
		public static readonly string PropBattlesCount = TypeHelper<PlayerStatisticEntity>.PropertyName(v => v.BattlesCount);
		public static readonly string PropRatingIntegratedValue = TypeHelper<PlayerStatisticEntity>.PropertyName(v => v.RatingIntegratedValue);
		public static readonly string PropRatingIntegratedPlace = TypeHelper<PlayerStatisticEntity>.PropertyName(v => v.RatingIntegratedPlace);
		public static readonly string PropRatingBattleAvgPerformanceValue = TypeHelper<PlayerStatisticEntity>.PropertyName(v => v.RatingBattleAvgPerformanceValue);
		public static readonly string PropRatingBattleAvgPerformancePlace = TypeHelper<PlayerStatisticEntity>.PropertyName(v => v.RatingBattleAvgPerformancePlace);
		public static readonly string PropRatingBattleAvgXpValue = TypeHelper<PlayerStatisticEntity>.PropertyName(v => v.RatingBattleAvgXpValue);
		public static readonly string PropRatingBattleAvgXpPlace = TypeHelper<PlayerStatisticEntity>.PropertyName(v => v.RatingBattleAvgXpPlace);
		public static readonly string PropRatingBattleWinsValue = TypeHelper<PlayerStatisticEntity>.PropertyName(v => v.RatingBattleWinsValue);
		public static readonly string PropRatingBattleWinsPlace = TypeHelper<PlayerStatisticEntity>.PropertyName(v => v.RatingBattleWinsPlace);
		public static readonly string PropRatingBattlesValue = TypeHelper<PlayerStatisticEntity>.PropertyName(v => v.RatingBattlesValue);
		public static readonly string PropRatingBattlesPlace = TypeHelper<PlayerStatisticEntity>.PropertyName(v => v.RatingBattlesPlace);
		public static readonly string PropRatingCapturedPointsValue = TypeHelper<PlayerStatisticEntity>.PropertyName(v => v.RatingCapturedPointsValue);
		public static readonly string PropRatingCapturedPointsPlace = TypeHelper<PlayerStatisticEntity>.PropertyName(v => v.RatingCapturedPointsPlace);
		public static readonly string PropRatingDamageDealtValue = TypeHelper<PlayerStatisticEntity>.PropertyName(v => v.RatingDamageDealtValue);
		public static readonly string PropRatingDamageDealtPlace = TypeHelper<PlayerStatisticEntity>.PropertyName(v => v.RatingDamageDealtPlace);
		public static readonly string PropRatingDroppedPointsValue = TypeHelper<PlayerStatisticEntity>.PropertyName(v => v.RatingDroppedPointsValue);
		public static readonly string PropRatingDroppedPointsPlace = TypeHelper<PlayerStatisticEntity>.PropertyName(v => v.RatingDroppedPointsPlace);
		public static readonly string PropRatingFragsValue = TypeHelper<PlayerStatisticEntity>.PropertyName(v => v.RatingFragsValue);
		public static readonly string PropRatingFragsPlace = TypeHelper<PlayerStatisticEntity>.PropertyName(v => v.RatingFragsPlace);
		public static readonly string PropRatingSpottedValue = TypeHelper<PlayerStatisticEntity>.PropertyName(v => v.RatingSpottedValue);
		public static readonly string PropRatingSpottedPlace = TypeHelper<PlayerStatisticEntity>.PropertyName(v => v.RatingSpottedPlace);
		public static readonly string PropRatingXpValue = TypeHelper<PlayerStatisticEntity>.PropertyName(v => v.RatingXpValue);
		public static readonly string PropRatingXpPlace = TypeHelper<PlayerStatisticEntity>.PropertyName(v => v.RatingXpPlace);
		public static readonly string PropPlayerId = TypeHelper<PlayerStatisticEntity>.PropertyName(v => v.PlayerId);
		
		#endregion

		/// <summary>
		/// Gets/Sets the field "Updated".
		/// </summary>
		public virtual DateTime Updated	{get; set; }
		
		/// <summary>
		/// Gets/Sets the field "Wins".
		/// </summary>
		public virtual int Wins	{get; set; }
		
		/// <summary>
		/// Gets/Sets the field "Losses".
		/// </summary>
		public virtual int Losses	{get; set; }
		
		/// <summary>
		/// Gets/Sets the field "SurvivedBattles".
		/// </summary>
		public virtual int SurvivedBattles	{get; set; }
		
		/// <summary>
		/// Gets/Sets the field "Xp".
		/// </summary>
		public virtual int Xp	{get; set; }
		
		/// <summary>
		/// Gets/Sets the field "BattleAvgXp".
		/// </summary>
		public virtual int BattleAvgXp	{get; set; }
		
		/// <summary>
		/// Gets/Sets the field "MaxXp".
		/// </summary>
		public virtual int MaxXp	{get; set; }
		
		/// <summary>
		/// Gets/Sets the field "Frags".
		/// </summary>
		public virtual int Frags	{get; set; }
		
		/// <summary>
		/// Gets/Sets the field "Spotted".
		/// </summary>
		public virtual int Spotted	{get; set; }
		
		/// <summary>
		/// Gets/Sets the field "HitsPercents".
		/// </summary>
		public virtual int HitsPercents	{get; set; }
		
		/// <summary>
		/// Gets/Sets the field "DamageDealt".
		/// </summary>
		public virtual int DamageDealt	{get; set; }
		
		/// <summary>
		/// Gets/Sets the field "CapturePoints".
		/// </summary>
		public virtual int CapturePoints	{get; set; }
		
		/// <summary>
		/// Gets/Sets the field "DroppedCapturePoints".
		/// </summary>
		public virtual int DroppedCapturePoints	{get; set; }
		
		/// <summary>
		/// Gets/Sets the field "BattlesCount".
		/// </summary>
		public virtual int BattlesCount	{get; set; }
		
		/// <summary>
		/// Gets/Sets the field "Rating_IntegratedValue".
		/// </summary>
		public virtual int RatingIntegratedValue	{get; set; }
		
		/// <summary>
		/// Gets/Sets the field "Rating_IntegratedPlace".
		/// </summary>
		public virtual int RatingIntegratedPlace	{get; set; }
		
		/// <summary>
		/// Gets/Sets the field "Rating_BattleAvgPerformanceValue".
		/// </summary>
		public virtual int RatingBattleAvgPerformanceValue	{get; set; }
		
		/// <summary>
		/// Gets/Sets the field "Rating_BattleAvgPerformancePlace".
		/// </summary>
		public virtual int RatingBattleAvgPerformancePlace	{get; set; }
		
		/// <summary>
		/// Gets/Sets the field "Rating_BattleAvgXpValue".
		/// </summary>
		public virtual int RatingBattleAvgXpValue	{get; set; }
		
		/// <summary>
		/// Gets/Sets the field "Rating_BattleAvgXpPlace".
		/// </summary>
		public virtual int RatingBattleAvgXpPlace	{get; set; }
		
		/// <summary>
		/// Gets/Sets the field "Rating_BattleWinsValue".
		/// </summary>
		public virtual int RatingBattleWinsValue	{get; set; }
		
		/// <summary>
		/// Gets/Sets the field "Rating_BattleWinsPlace".
		/// </summary>
		public virtual int RatingBattleWinsPlace	{get; set; }
		
		/// <summary>
		/// Gets/Sets the field "Rating_BattlesValue".
		/// </summary>
		public virtual int RatingBattlesValue	{get; set; }
		
		/// <summary>
		/// Gets/Sets the field "Rating_BattlesPlace".
		/// </summary>
		public virtual int RatingBattlesPlace	{get; set; }
		
		/// <summary>
		/// Gets/Sets the field "Rating_CapturedPointsValue".
		/// </summary>
		public virtual int RatingCapturedPointsValue	{get; set; }
		
		/// <summary>
		/// Gets/Sets the field "Rating_CapturedPointsPlace".
		/// </summary>
		public virtual int RatingCapturedPointsPlace	{get; set; }
		
		/// <summary>
		/// Gets/Sets the field "Rating_DamageDealtValue".
		/// </summary>
		public virtual int RatingDamageDealtValue	{get; set; }
		
		/// <summary>
		/// Gets/Sets the field "Rating_DamageDealtPlace".
		/// </summary>
		public virtual int RatingDamageDealtPlace	{get; set; }
		
		/// <summary>
		/// Gets/Sets the field "Rating_DroppedPointsValue".
		/// </summary>
		public virtual int RatingDroppedPointsValue	{get; set; }
		
		/// <summary>
		/// Gets/Sets the field "Rating_DroppedPointsPlace".
		/// </summary>
		public virtual int RatingDroppedPointsPlace	{get; set; }
		
		/// <summary>
		/// Gets/Sets the field "Rating_FragsValue".
		/// </summary>
		public virtual int RatingFragsValue	{get; set; }
		
		/// <summary>
		/// Gets/Sets the field "Rating_FragsPlace".
		/// </summary>
		public virtual int RatingFragsPlace	{get; set; }
		
		/// <summary>
		/// Gets/Sets the field "Rating_SpottedValue".
		/// </summary>
		public virtual int RatingSpottedValue	{get; set; }
		
		/// <summary>
		/// Gets/Sets the field "Rating_SpottedPlace".
		/// </summary>
		public virtual int RatingSpottedPlace	{get; set; }
		
		/// <summary>
		/// Gets/Sets the field "Rating_XpValue".
		/// </summary>
		public virtual int RatingXpValue	{get; set; }
		
		/// <summary>
		/// Gets/Sets the field "Rating_XpPlace".
		/// </summary>
		public virtual int RatingXpPlace	{get; set; }
		
		/// <summary>
		/// Gets/Sets the field "PlayerId".
		/// </summary>
		public virtual int PlayerId { get; set; }
		
		/// <summary>
		/// Gets/Sets the <see cref="PlayerEntity"/> object.
		/// </summary>
		public virtual PlayerEntity PlayerIdObject { get; set; }

        /// <summary>
        /// Gets/Sets the AvgLevel object.
        /// </summary>
        public virtual double AvgLevel { get; set; }

	    public virtual void Update(PlayerStat stat)
	    {
            #region Common init

            BattlesCount = stat.data.summary.Battles_count;
            Wins = stat.data.summary.Wins;
            Losses = stat.data.summary.Losses;
            SurvivedBattles = stat.data.summary.Survived_battles;
            Xp = stat.data.experience.Xp;
            BattleAvgXp = stat.data.experience.Battle_avg_xp;
            MaxXp = stat.data.experience.Max_xp;
            Frags = stat.data.battles.Frags;
            Spotted = stat.data.battles.Spotted;
            HitsPercents = stat.data.battles.Hits_percents;
            DamageDealt = stat.data.battles.Damage_dealt;
            CapturePoints = stat.data.battles.Capture_points;
            DroppedCapturePoints = stat.data.battles.Dropped_capture_points;
            Updated = Utils.UnixDateToDateTime((long)stat.data.updated_at);

            #endregion

            #region Ratings init

            //GR-->
            //Global Rating
            RatingIntegratedValue = stat.data.ratings.Integrated_rating.Value;
            RatingIntegratedPlace = stat.data.ratings.Integrated_rating.Place ?? 0;
            //W/B-->
            //Victories/Battles
            RatingBattleAvgPerformanceValue = stat.data.ratings.Battle_avg_performance.Value;
            RatingBattleAvgPerformancePlace = stat.data.ratings.Battle_avg_performance.Place ?? 0;
            //E/B-->
            //Average Experience per Battle
            RatingBattleAvgXpValue = stat.data.ratings.Battle_avg_xp.Value;
            RatingBattleAvgXpPlace = stat.data.ratings.Battle_avg_xp.Place ?? 0;
            //WIN-->
            //Victories
            RatingBattleWinsValue = stat.data.ratings.Battle_wins.Value;
            RatingBattleWinsPlace = stat.data.ratings.Battle_wins.Place ?? 0;
            //GPL-->
            //Battles Participated
            RatingBattlesValue = stat.data.ratings.Battles.Value;
            RatingBattlesPlace = stat.data.ratings.Battles.Place ?? 0;
            //CPT-->
            //Capture Points
            RatingCapturedPointsValue = stat.data.ratings.Ctf_points.Value;
            RatingCapturedPointsPlace = stat.data.ratings.Ctf_points.Place ?? 0;
            //DMG-->
            //Damage Caused
            RatingDamageDealtValue = stat.data.ratings.Damage_dealt.Value;
            RatingDamageDealtPlace = stat.data.ratings.Damage_dealt.Place ?? 0;
            //DPT-->
            //Defense Points
            RatingDroppedPointsValue = stat.data.ratings.Dropped_ctf_points.Value;
            RatingDroppedPointsPlace = stat.data.ratings.Dropped_ctf_points.Place ?? 0;
            //FRG-->
            //Targets Destroyed
            RatingFragsValue = stat.data.ratings.Frags.Value;
            RatingFragsPlace = stat.data.ratings.Frags.Place ?? 0;
            //SPT-->
            //Targets Detected
            RatingSpottedValue = stat.data.ratings.Spotted.Value;
            RatingSpottedPlace = stat.data.ratings.Spotted.Place ?? 0;
            //EXP-->
            //Total Experience
            RatingXpValue = stat.data.ratings.Xp.Value;
            RatingXpPlace = stat.data.ratings.Xp.Place ?? 0;

            #endregion

	        AvgLevel = stat.data.vehicles.Sum(x => x.level*x.battle_count)/(double) stat.data.summary.Battles_count;
	    }
	}
}

