using AutoMapper;

using ServiceBricks.Storage.MongoDb;

namespace ServiceBricks.Security.MongoDb
{
    /// <summary>
    /// This is an automapper profile for the ApplicationUserLogin domain object.
    /// </summary>
    public class ApplicationUserLoginMappingProfile : Profile
    {
        public ApplicationUserLoginMappingProfile()
        {
            CreateMap<ApplicationUserLoginDto, ApplicationUserLogin>()
                .ForMember(x => x.Key, y => y.MapFrom(z => z.StorageKey))
                .ForMember(x => x.UserId, y => y.MapFrom(z => z.UserStorageKey));

            CreateMap<ApplicationUserLogin, ApplicationUserLoginDto>()
                .ForMember(x => x.StorageKey, y => y.MapFrom(z => z.Key))
                .ForMember(x => x.UserStorageKey, y => y.MapFrom(z => z.UserId));
        }
    }
}