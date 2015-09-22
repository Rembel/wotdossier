namespace WotDossier.Domain
{
    public enum ClanBattleType
    {
        /// <summary>
        /// бой за провинцию
        /// </summary>
        for_province,

        /// <summary>
        /// встречный бой
        /// </summary>
        meeting_engagement,
        
        /// <summary>
        /// бой за высадку.
        /// </summary>
        landing
    }

    public enum StrongholBattleType
    {
        /// <summary>
        /// Defense
        /// </summary>
        defense,

        /// <summary>
        /// Attack
        /// </summary>
        attack
    }
}
