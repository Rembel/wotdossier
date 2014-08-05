using WotDossier.Domain.Interfaces;

namespace WotDossier.Applications.Logic
{
    public class Mapper
    {
        static Mapper()
        {
            AutoMapper.Mapper.CreateMap<IRandomBattlesAchievements, IRandomBattlesAchievements>();
            AutoMapper.Mapper.CreateMap<IHistoricalBattlesAchievements, IHistoricalBattlesAchievements>();
            AutoMapper.Mapper.CreateMap<ITeamBattlesAchievements, ITeamBattlesAchievements>();
            AutoMapper.Mapper.CreateMap<IFortAchievements, IFortAchievements>();
            AutoMapper.Mapper.CreateMap<IClanBattlesAchievements, IClanBattlesAchievements>();
        }

        public static void Map<TSource, TTarget>(TSource source, TTarget target)
        {
            AutoMapper.Mapper.Map(source, target);
        }

        public static void Map<T>(T source, T target)
        {
            AutoMapper.Mapper.Map(source, target);
        }
    }
}
