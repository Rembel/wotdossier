using System;
using System.Runtime.Serialization;
using WotDossier.Domain.Server;

namespace WotDossier.Domain.Entities
{
    /// <summary>
    ///     Object representation for table 'RandomBattlesStatistic'.
    /// </summary>
    [Serializable]
    [DataContract]
    public class RandomBattlesStatisticEntity : StatisticEntity
    {
        /// <summary>
        ///     Gets/Sets the <see cref="RandomBattlesAchievementsEntity" /> object.
        /// </summary>
        [DataMember(Name = "Achievements")]
        public virtual RandomBattlesAchievementsEntity AchievementsIdObject { get; set; }

        /// <summary>
        ///     Updates the ratings.
        /// </summary>
        /// <param name="ratings">The ratings.</param>
        public override void UpdateRatings(Ratings ratings)
        {
            if (ratings.global_rating == null)
            {
                return;
            }

            #region Ratings init

            //GR-->
            //Global Rating
            //RBR = (int) (ratings.global_rating.Value ?? 0);
            
            #endregion
        }
    }
}