using System;
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
        public static readonly string PropAvgLevel = TypeHelper<PlayerStatisticEntity>.PropertyName(v => v.AvgLevel);
        public static readonly string PropPlayerId = TypeHelper<PlayerStatisticEntity>.PropertyName(v => v.PlayerId);
		public static readonly string PropAchievementsId = TypeHelper<PlayerStatisticEntity>.PropertyName(v => v.AchievementsId);

        #endregion

        /// <summary>
        /// Gets/Sets the field "Updated".
        /// </summary>
        public virtual DateTime Updated { get; set; }

        /// <summary>
        /// Gets/Sets the field "Wins".
        /// </summary>
        public virtual int Wins { get; set; }

        /// <summary>
        /// Gets/Sets the field "Losses".
        /// </summary>
        public virtual int Losses { get; set; }

        /// <summary>
        /// Gets/Sets the field "SurvivedBattles".
        /// </summary>
        public virtual int SurvivedBattles { get; set; }

        /// <summary>
        /// Gets/Sets the field "Xp".
        /// </summary>
        public virtual int Xp { get; set; }

        /// <summary>
        /// Gets/Sets the field "BattleAvgXp".
        /// </summary>
        public virtual double BattleAvgXp { get; set; }

        /// <summary>
        /// Gets/Sets the field "MaxXp".
        /// </summary>
        public virtual int MaxXp { get; set; }

        /// <summary>
        /// Gets/Sets the field "Frags".
        /// </summary>
        public virtual int Frags { get; set; }

        /// <summary>
        /// Gets/Sets the field "Spotted".
        /// </summary>
        public virtual int Spotted { get; set; }

        /// <summary>
        /// Gets/Sets the field "HitsPercents".
        /// </summary>
        public virtual double HitsPercents { get; set; }

        /// <summary>
        /// Gets/Sets the field "DamageDealt".
        /// </summary>
        public virtual int DamageDealt { get; set; }

        /// <summary>
        /// Gets/Sets the field "DamageTaken".
        /// </summary>
        public virtual int DamageTaken { get; set; }

        /// <summary>
        /// Gets/Sets the field "CapturePoints".
        /// </summary>
        public virtual int CapturePoints { get; set; }

        /// <summary>
        /// Gets/Sets the field "DroppedCapturePoints".
        /// </summary>
        public virtual int DroppedCapturePoints { get; set; }

        /// <summary>
        /// Gets/Sets the field "BattlesCount".
        /// </summary>
        public virtual int BattlesCount { get; set; }

        /// <summary>
        /// Gets/Sets the field "Rating_IntegratedValue".
        /// </summary>
        public virtual int RatingIntegratedValue { get; set; }

        /// <summary>
        /// Gets/Sets the field "Rating_IntegratedPlace".
        /// </summary>
        public virtual int RatingIntegratedPlace { get; set; }

        /// <summary>
        /// Gets/Sets the field "Rating_BattleAvgPerformanceValue".
        /// </summary>
        public virtual double RatingBattleAvgPerformanceValue { get; set; }

        /// <summary>
        /// Gets/Sets the field "Rating_BattleAvgPerformancePlace".
        /// </summary>
        public virtual int RatingBattleAvgPerformancePlace { get; set; }

        /// <summary>
        /// Gets/Sets the field "Rating_BattleAvgXpValue".
        /// </summary>
        public virtual double RatingBattleAvgXpValue { get; set; }

        /// <summary>
        /// Gets/Sets the field "Rating_BattleAvgXpPlace".
        /// </summary>
        public virtual int RatingBattleAvgXpPlace { get; set; }

        /// <summary>
        /// Gets/Sets the field "Rating_BattleWinsValue".
        /// </summary>
        public virtual int RatingBattleWinsValue { get; set; }

        /// <summary>
        /// Gets/Sets the field "Rating_BattleWinsPlace".
        /// </summary>
        public virtual int RatingBattleWinsPlace { get; set; }

        /// <summary>
        /// Gets/Sets the field "Rating_BattlesValue".
        /// </summary>
        public virtual int RatingBattlesValue { get; set; }

        /// <summary>
        /// Gets/Sets the field "Rating_BattlesPlace".
        /// </summary>
        public virtual int RatingBattlesPlace { get; set; }

        /// <summary>
        /// Gets/Sets the field "Rating_CapturedPointsValue".
        /// </summary>
        public virtual int RatingCapturedPointsValue { get; set; }

        /// <summary>
        /// Gets/Sets the field "Rating_CapturedPointsPlace".
        /// </summary>
        public virtual int RatingCapturedPointsPlace { get; set; }

        /// <summary>
        /// Gets/Sets the field "Rating_DamageDealtValue".
        /// </summary>
        public virtual int RatingDamageDealtValue { get; set; }

        /// <summary>
        /// Gets/Sets the field "Rating_DamageDealtPlace".
        /// </summary>
        public virtual int RatingDamageDealtPlace { get; set; }

        /// <summary>
        /// Gets/Sets the field "Rating_DroppedPointsValue".
        /// </summary>
        public virtual int RatingDroppedPointsValue { get; set; }

        /// <summary>
        /// Gets/Sets the field "Rating_DroppedPointsPlace".
        /// </summary>
        public virtual int RatingDroppedPointsPlace { get; set; }

        /// <summary>
        /// Gets/Sets the field "Rating_FragsValue".
        /// </summary>
        public virtual int RatingFragsValue { get; set; }

        /// <summary>
        /// Gets/Sets the field "Rating_FragsPlace".
        /// </summary>
        public virtual int RatingFragsPlace { get; set; }

        /// <summary>
        /// Gets/Sets the field "Rating_SpottedValue".
        /// </summary>
        public virtual int RatingSpottedValue { get; set; }

        /// <summary>
        /// Gets/Sets the field "Rating_SpottedPlace".
        /// </summary>
        public virtual int RatingSpottedPlace { get; set; }

        /// <summary>
        /// Gets/Sets the field "Rating_XpValue".
        /// </summary>
        public virtual int RatingXpValue { get; set; }

        /// <summary>
        /// Gets/Sets the field "Rating_XpPlace".
        /// </summary>
        public virtual int RatingXpPlace { get; set; }

        /// <summary>
        /// Gets/Sets the field "RatingHitsPercentsValue".
        /// </summary>
        public virtual double RatingHitsPercentsValue { get; set; }

        /// <summary>
        /// Gets/Sets the field "RatingHitsPercentsPlace".
        /// </summary>
        public virtual int RatingHitsPercentsPlace { get; set; }

        /// <summary>
        /// Gets/Sets the field "RatingMaxXpValue".
        /// </summary>
        public virtual int RatingMaxXpValue { get; set; }

        /// <summary>
        /// Gets/Sets the field "RatingMaxXpPlace".
        /// </summary>
        public virtual int RatingMaxXpPlace { get; set; }

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

				/// <summary>
		/// Gets/Sets the field "AchievementsId".
		/// </summary>
		public virtual int? AchievementsId { get; set; }
		
		/// <summary>
		/// Gets/Sets the <see cref="PlayerAchievementsEntity"/> object.
		/// </summary>
		public virtual PlayerAchievementsEntity AchievementsIdObject { get; set; }

        public virtual void Update(PlayerStatAdapter statAdapter)
        {
            #region CommonJson init

            BattlesCount = statAdapter.Battles_count;
            Wins = statAdapter.Wins;
            Losses = statAdapter.Losses;
            SurvivedBattles = statAdapter.Survived_battles;
            Xp = statAdapter.Xp;
            BattleAvgXp = statAdapter.Battle_avg_xp;
            MaxXp = statAdapter.Max_xp;
            Frags = statAdapter.Frags;
            Spotted = statAdapter.Spotted;
            HitsPercents = statAdapter.Hits_percents;
            DamageDealt = statAdapter.Damage_dealt;
            DamageTaken = statAdapter.Damage_taken;
            CapturePoints = statAdapter.Capture_points;
            DroppedCapturePoints = statAdapter.Dropped_capture_points;
            Updated = statAdapter.Updated;
            AvgLevel = statAdapter.AvgLevel;

            if (AchievementsIdObject == null)
            {
                AchievementsIdObject = new PlayerAchievementsEntity();
            }

            #region [ IRowBattleAwards ]

            AchievementsIdObject.Warrior = statAdapter.Warrior;
            AchievementsIdObject.Invader = statAdapter.Invader;
            AchievementsIdObject.Sniper = statAdapter.Sniper;
            AchievementsIdObject.Defender = statAdapter.Defender;
            AchievementsIdObject.SteelWall = statAdapter.SteelWall;
            AchievementsIdObject.Confederate = statAdapter.Confederate;
            AchievementsIdObject.Scout = statAdapter.Scout;
            AchievementsIdObject.PatrolDuty = statAdapter.PatrolDuty;
            AchievementsIdObject.BrothersInArms = statAdapter.BrothersInArms;
            AchievementsIdObject.CrucialContribution = statAdapter.CrucialContribution;
            AchievementsIdObject.CoolHeaded = statAdapter.CoolHeaded;
            AchievementsIdObject.LuckyDevil = statAdapter.LuckyDevil;
            AchievementsIdObject.Spartan = statAdapter.Spartan;

            #endregion

            #region [ IRowEpic ]

            AchievementsIdObject.Boelter = statAdapter.Boelter;
            AchievementsIdObject.RadleyWalters = statAdapter.RadleyWalters;
            AchievementsIdObject.LafayettePool = statAdapter.LafayettePool;
            AchievementsIdObject.Orlik = statAdapter.Orlik;
            AchievementsIdObject.Oskin = statAdapter.Oskin;
            AchievementsIdObject.Lehvaslaiho = statAdapter.Lehvaslaiho;
            AchievementsIdObject.Nikolas = statAdapter.Nikolas;
            AchievementsIdObject.Halonen = statAdapter.Halonen;
            AchievementsIdObject.Burda = statAdapter.Burda;
            AchievementsIdObject.Pascucci = statAdapter.Pascucci;
            AchievementsIdObject.Dumitru = statAdapter.Dumitru;
            AchievementsIdObject.TamadaYoshio = statAdapter.TamadaYoshio;
            AchievementsIdObject.Billotte = statAdapter.Billotte;
            AchievementsIdObject.BrunoPietro = statAdapter.BrunoPietro;
            AchievementsIdObject.Tarczay = statAdapter.Tarczay;
            AchievementsIdObject.Kolobanov = statAdapter.Kolobanov;
            AchievementsIdObject.Fadin = statAdapter.Fadin;
            AchievementsIdObject.HeroesOfRassenay = statAdapter.HeroesOfRassenay;
            AchievementsIdObject.DeLanglade = statAdapter.DeLanglade;

            #endregion

            #region [ IRowMedals]

            AchievementsIdObject.Kay = statAdapter.Kay;
            AchievementsIdObject.Carius = statAdapter.Carius;
            AchievementsIdObject.Knispel = statAdapter.Knispel;
            AchievementsIdObject.Poppel = statAdapter.Poppel;
            AchievementsIdObject.Abrams = statAdapter.Abrams;
            AchievementsIdObject.Leclerk = statAdapter.Leclerk;
            AchievementsIdObject.Lavrinenko = statAdapter.Lavrinenko;
            AchievementsIdObject.Ekins = statAdapter.Ekins;

            #endregion

            #region [ IRowSeries ]

            AchievementsIdObject.SharpshooterLongest = statAdapter.SharpshooterLongest;
            AchievementsIdObject.MasterGunnerLongest = statAdapter.MasterGunnerLongest;

            #endregion

            #region [ IRowSpecialAwards ]

            AchievementsIdObject.Kamikaze = statAdapter.Kamikaze;
            AchievementsIdObject.Raider = statAdapter.Raider;
            AchievementsIdObject.Bombardier = statAdapter.Bombardier;
            AchievementsIdObject.Reaper = statAdapter.Reaper;
            AchievementsIdObject.Invincible = statAdapter.Invincible;
            AchievementsIdObject.Survivor = statAdapter.Survivor;
            AchievementsIdObject.MouseTrap = statAdapter.MouseTrap;
            AchievementsIdObject.Hunter = statAdapter.Hunter;
            AchievementsIdObject.Sinai = statAdapter.Sinai;
            AchievementsIdObject.PattonValley = statAdapter.PattonValley;
            AchievementsIdObject.Ranger = statAdapter.Ranger;

            #endregion

            #endregion
        }

        public virtual void UpdateRatings(Ratings ratings)
        {
            #region Ratings init

            //GR-->
            //Global Rating
            //RatingIntegratedValue = ratings.Integrated_rating.Value;
            //RatingIntegratedPlace = ratings.Integrated_rating.Rank ?? 0;
            //W/B-->
            //Victories/Battles
            //wins_ratio 	Процент побед wins_ratio.rank 	numeric 	
            RatingBattleAvgPerformanceValue = ratings.wins_ratio.Value ?? 0;
            RatingBattleAvgPerformancePlace = ratings.wins_ratio.Rank ?? 0;
            //E/B-->
            //Average Experience per BattleJson
            //xp_avg 	Средний опыт за бой xp_avg.rank 	numeric 	
            RatingBattleAvgXpValue = ratings.xp_avg.Value ?? 0;
            RatingBattleAvgXpPlace = ratings.xp_avg.Rank ?? 0;
            //WIN-->
            //Victories
            //RatingBattleWinsValue = ratings.Battle_wins.Value;
            //RatingBattleWinsPlace = ratings.Battle_wins.Rank ?? 0;
            //GPL-->
            //Battles Participated
            //battles_count 	Количество проведённых боёв battles_count.rank 	numeric 	
            RatingBattlesValue = (int) (ratings.battles_count.Value ?? 0);
            RatingBattlesPlace = ratings.battles_count.Rank ?? 0;
            //CPT-->
            //Capture Points
            //RatingCapturedPointsValue = ratings.Ctf_points.Value;
            //RatingCapturedPointsPlace = ratings.Ctf_points.Rank ?? 0;
            //DMG-->
            //Damage Caused
            //damage_dealt 	Общий нанесённый урон damage_dealt.rank 	numeric 	
            RatingDamageDealtValue = (int) (ratings.damage_dealt.Value ?? 0);
            RatingDamageDealtPlace = ratings.damage_dealt.Rank ?? 0;
            //DPT-->
            //Defense Points
            //RatingDroppedPointsValue = ratings.Dropped_ctf_points.Value;
            //RatingDroppedPointsPlace = ratings.Dropped_ctf_points.Rank ?? 0;
            //FRG-->
            //Targets Destroyed
            //frags_count 	Количество уничтоженной техники frags_count.rank 	numeric 	
            RatingFragsValue = (int) (ratings.frags_count.Value ?? 0);
            RatingFragsPlace = ratings.frags_count.Rank ?? 0;
            //SPT-->
            //Targets Detected
            //spotted_count 	Количество обнаруженной техники spotted_count.rank 	numeric 	
            RatingSpottedValue = (int) (ratings.spotted_count.Value ?? 0);
            RatingSpottedPlace = ratings.spotted_count.Rank ?? 0;
            //EXP-->
            //Total Experience
            //xp_amount 	Общий опыт xp_amount.rank 	numeric 	
            RatingXpValue = (int) (ratings.xp_amount.Value ?? 0);
            RatingXpPlace = ratings.xp_amount.Rank ?? 0;

            RatingMaxXpValue = (int) (ratings.xp_max.Value ?? 0);
            RatingMaxXpPlace = ratings.xp_max.Rank ?? 0;

            RatingHitsPercentsValue = ratings.hits_ratio.Value ?? 0;
            RatingHitsPercentsPlace = ratings.hits_ratio.Rank ?? 0;

            /*
            damage_avg 	Средний нанесённый урон за бой damage_avg.rank 	numeric 	
            frags_avg 	Среднее количество уничтоженной техники за бой frags_avg.rank 	numeric 	
            hits_ratio 	Процент попаданий hits_ratio.rank 	numeric 	
            spotted_avg 	Среднее количество обнаруженной техники за бой spotted_avg.rank 	numeric 	
            survived_ratio 	Процент выживаемости survived_ratio.rank 	numeric 	
            xp_max 	Максимальный опыт за бой xp_max.rank 	numeric 	
             */

            #endregion
        }
    }
}

