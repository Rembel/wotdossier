using System.Dynamic;
using WotDossier.Applications.ViewModel;
using WotDossier.Domain;
using WotDossier.Domain.Entities;
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
            AutoMapper.Mapper.CreateMap<ExpandoObject, Map>();
            AutoMapper.Mapper.CreateMap<FavoritePlayerEntity, ListItem<int>>()
                .ForMember(x => x.Id, opt => opt.MapFrom(x => x.Id))
                .ForMember(x => x.Value, opt => opt.MapFrom(x => x.Name));
        }

        public static void Map<TSource, TTarget>(TSource source, TTarget target)
        {
            AutoMapper.Mapper.Map(source, target);
        }

        public static void Map<T>(T source, T target)
        {
            AutoMapper.Mapper.Map(source, target);
        }

        public static TDestination Map<TSource, TDestination>(TSource source)
        {
           return AutoMapper.Mapper.Map<TDestination>(source);
        }
    }
}
