using AutoMapper;

namespace ServiceBricks.Security.Cosmos
{
    /// <summary>
    /// This is an automapper profile for the ApplicationUserToken domain object.
    /// </summary>
    public class ApplicationUserTokenMappingProfile : Profile
    {
        public ApplicationUserTokenMappingProfile()
        {
            CreateMap<ApplicationUserTokenDto, ApplicationUserToken>()
                .ForMember(x => x.Key, y => y.MapFrom<KeyResolver>())
                .ForMember(x => x.UserId, y => y.MapFrom<UserIdResolver>());

            CreateMap<ApplicationUserToken, ApplicationUserTokenDto>()
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

        public class UserIdResolver : IValueResolver<ApplicationUserTokenDto, object, Guid>
        {
            public Guid Resolve(ApplicationUserTokenDto source, object destination, Guid sourceMember, ResolutionContext context)
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