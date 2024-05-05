using AutoMapper;

namespace ServiceBricks.Security.Cosmos
{
    /// <summary>
    /// This is an automapper profile for the ApplicationUserLogin domain object.
    /// </summary>
    public class ApplicationUserLoginMappingProfile : Profile
    {
        public ApplicationUserLoginMappingProfile()
        {
            CreateMap<ApplicationUserLoginDto, ApplicationUserLogin>()
                .ForMember(x => x.UserId, y => y.MapFrom<UserIdResolver>())
                .ForMember(x => x.Key, y => y.MapFrom<KeyResolver>());

            CreateMap<ApplicationUserLogin, ApplicationUserLoginDto>()
                .ForMember(x => x.StorageKey, y => y.MapFrom(z => z.Key))
                .ForMember(x => x.UserStorageKey, y => y.MapFrom(z => z.UserId));
        }

        public class KeyResolver : IValueResolver<DataTransferObject, object, Guid>
        {
            public Guid Resolve(DataTransferObject source, object destination, Guid sourceMember, ResolutionContext context)
            {
                if (string.IsNullOrEmpty(source.StorageKey))
                    return Guid.Empty;

                Guid tempKey;
                if (Guid.TryParse(source.StorageKey, out tempKey))
                    return tempKey;
                return Guid.Empty;
            }
        }

        public class UserIdResolver : IValueResolver<ApplicationUserLoginDto, object, Guid>
        {
            public Guid Resolve(ApplicationUserLoginDto source, object destination, Guid sourceMember, ResolutionContext context)
            {
                if (string.IsNullOrEmpty(source.UserStorageKey))
                    return Guid.Empty;

                Guid tempKey;
                if (Guid.TryParse(source.UserStorageKey, out tempKey))
                    return tempKey;
                return Guid.Empty;
            }
        }
    }
}