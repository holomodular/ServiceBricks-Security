using AutoMapper;

using ServiceBricks.Storage.MongoDb;

namespace ServiceBricks.Security.MongoDb
{
    /// <summary>
    /// This is an automapper profile for the ApplicationUserToken domain object.
    /// </summary>
    public class ApplicationUserTokenMappingProfile : Profile
    {
        public ApplicationUserTokenMappingProfile()
        {
            CreateMap<ApplicationUserTokenDto, ApplicationUserToken>()
                .ForMember(x => x.Key, y => y.MapFrom(z => z.StorageKey))
                .ForMember(x => x.UserId, y => y.MapFrom(z => z.UserStorageKey));

            CreateMap<ApplicationUserToken, ApplicationUserTokenDto>()
                .ForMember(x => x.StorageKey, y => y.MapFrom(z => z.Key))
                .ForMember(x => x.UserStorageKey, y => y.MapFrom(z => z.UserId));
        }
    }
}