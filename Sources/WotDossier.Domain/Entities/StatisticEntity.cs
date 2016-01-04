using System;
using System.Runtime.Serialization;
using WotDossier.Domain.Interfaces;
using WotDossier.Domain.Server;

namespace WotDossier.Domain.Entities
{
    /// <summary>
    ///     Base object representation for tables of type 'Statistic'.
    /// </summary>
    [Serializable]
    [DataContract]
    public abstract class StatisticEntity : EntityBase, IStatisticBase, IRevised
    {
        /// <summary>
        ///     Gets/Sets the field "PlayerId".
        /// </summary>
        [DataMember]
        public virtual int PlayerId { get; set; }

        [DataMember]
        public virtual Guid PlayerUId { get; set; }

        /// <summary>
        ///     Gets/Sets the field "BattleAvgXp".
        /// </summary>
        [DataMember]
        public virtual double BattleAvgXp { get; set; }

        /// <summary>
        ///     Gets or sets the mark of mastery.
        /// </summary>
        [DataMember]
        public virtual int MarkOfMastery { get; set; }

        /// <summary>
        ///     Gets/Sets the AvgLevel object.
        /// </summary>
        [DataMember]
        public virtual double AvgLevel { get; set; }

        /// <summary>
        ///     Gets or sets the RBR.
        /// </summary>
        [DataMember]
        public virtual double RBR { get; set; }

        /// <summary>
        ///     Gets or sets the W n8 rating.
        /// </summary>
        [DataMember]
        public virtual double WN8Rating { get; set; }

        /// <summary>
        ///     Gets or sets the performance rating.
        /// </summary>
        [DataMember]
        public virtual double PerformanceRating { get; set; }

        /// <summary>
        ///     Gets/Sets the <see cref="PlayerEntity" /> object.
        /// </summary>
        public virtual PlayerEntity PlayerIdObject { get; set; }

        /// <summary>
        ///     Gets/Sets the field "AchievementsId".
        /// </summary>
        [DataMember]
        public virtual int? AchievementsId { get; set; }

        [DataMember]
        public virtual Guid? AchievementsUId { get; set; }

        /// <summary>
        ///     Gets/Sets the field "Updated".
        /// </summary>
        [DataMember]
        public virtual DateTime Updated { get; set; }

        /// <summary>
        ///     Gets/Sets the field "BattlesCount".
        /// </summary>
        [DataMember]
        public virtual int BattlesCount { get; set; }

        /// <summary>
        ///     Gets/Sets the field "Wins".
        /// </summary>
        [DataMember]
        public virtual int Wins { get; set; }

        /// <summary>
        ///     Gets/Sets the field "Losses".
        /// </summary>
        [DataMember]
        public virtual int Losses { get; set; }

        /// <summary>
        ///     Gets/Sets the field "SurvivedBattles".
        /// </summary>
        [DataMember]
        public virtual int SurvivedBattles { get; set; }

        /// <summary>
        ///     Gets/Sets the field "Xp".
        /// </summary>
        [DataMember]
        public virtual int Xp { get; set; }

        /// <summary>
        ///     Gets/Sets the field "MaxXp".
        /// </summary>
        [DataMember]
        public virtual int MaxXp { get; set; }

        /// <summary>
        ///     Gets/Sets the field "Frags".
        /// </summary>
        [DataMember]
        public virtual int Frags { get; set; }

        /// <summary>
        ///     Gets or sets the maximum frags.
        /// </summary>
        [DataMember]
        public virtual int MaxFrags { get; set; }

        /// <summary>
        ///     Gets/Sets the field "Spotted".
        /// </summary>
        [DataMember]
        public virtual int Spotted { get; set; }

        /// <summary>
        ///     Gets/Sets the field "HitsPercents".
        /// </summary>
        [DataMember]
        public virtual double HitsPercents { get; set; }

        /// <summary>
        ///     Gets/Sets the field "DamageDealt".
        /// </summary>
        [DataMember]
        public virtual int DamageDealt { get; set; }

        /// <summary>
        ///     Gets/Sets the field "DamageTaken".
        /// </summary>
        [DataMember]
        public virtual int DamageTaken { get; set; }

        /// <summary>
        ///     Gets or sets the maximum damage.
        /// </summary>
        [DataMember]
        public virtual int MaxDamage { get; set; }

        /// <summary>
        ///     Gets/Sets the field "CapturePoints".
        /// </summary>
        [DataMember]
        public virtual int CapturePoints { get; set; }

        /// <summary>
        ///     Gets/Sets the field "DroppedCapturePoints".
        /// </summary>
        [DataMember]
        public virtual int DroppedCapturePoints { get; set; }
    }
}