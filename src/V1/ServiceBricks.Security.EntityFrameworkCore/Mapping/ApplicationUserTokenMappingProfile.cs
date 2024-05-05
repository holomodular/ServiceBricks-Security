using AutoMapper;

using ServiceBricks.Storage.EntityFrameworkCore;

namespace ServiceBricks.Security.EntityFrameworkCore
{
    /// <summary>
    /// This is an automapper profile for the ApplicationUserToken domain object.
    /// </summary>
    public class ApplicationUserTokenMappingProfile : Profile
    {
        public ApplicationUserTokenMappingProfile()
        {
            CreateMap<ApplicationUserTokenDto, ApplicationUserToken>()
                .ForMember(x => x.UserId, y => y.MapFrom<UserIdResolver>())
                .ForMember(x => x.LoginProvider, y => y.MapFrom<LoginProviderResolver>())
                .ForMember(x => x.Name, y => y.MapFrom<NameResolver>());

            CreateMap<ApplicationUserToken, ApplicationUserTokenDto>()
                .ForMember(x => x.StorageKey, y => y.MapFrom(z =>
                    z.UserId.ToString() +
                    StorageEntityFrameworkCoreConstants.STORAGEKEY_DELIMITER +
                    z.LoginProvider.ToString() +
                    StorageEntityFrameworkCoreConstants.STORAGEKEY_DELIMITER +
                    z.Name))
                .ForMember(x => x.UserStorageKey, y => y.MapFrom(z => z.UserId));
        }

        public class UserIdResolver : IValueResolver<ApplicationUserTokenDto, object, Guid>
        {
            public Guid Resolve(ApplicationUserTokenDto source, object destination, Guid sourceMember, ResolutionContext context)
            {
                if (!string.IsNullOrEmpty(source.UserStorageKey))
                {
                    Guid tempGuid;
                    if (Guid.TryParse(source.UserStorageKey, out tempGuid))
                        return tempGuid;
                }
                if (string.IsNullOrEmpty(source.StorageKey))
                    return Guid.Empty;

                string[] split = source.StorageKey.Split(StorageEntityFrameworkCoreConstants.STORAGEKEY_DELIMITER);
                if (split.Length >= 1)
                {
                    Guid tempGuid;
                    if (Guid.TryParse(split[0], out tempGuid))
                        return tempGuid;
                }
                return Guid.Empty;
            }
        }

        public class LoginProviderResolver : IValueResolver<ApplicationUserTokenDto, object, string>
        {
            public string Resolve(ApplicationUserTokenDto source, object destination, string sourceMember, ResolutionContext context)
            {
                if (!string.IsNullOrEmpty(source.LoginProvider))
                    return source.LoginProvider;
                if (string.IsNullOrEmpty(source.StorageKey))
                    return string.Empty;

                string[] split = source.StorageKey.Split(StorageEntityFrameworkCoreConstants.STORAGEKEY_DELIMITER);
                if (split.Length >= 2)
                    return split[1];
                return string.Empty;
            }
        }

        public class NameResolver : IValueResolver<ApplicationUserTokenDto, object, string>
        {
            public string Resolve(ApplicationUserTokenDto source, object destination, string sourceMember, ResolutionContext context)
            {
                if (!string.IsNullOrEmpty(source.Name))
                    return source.Name;
                if (string.IsNullOrEmpty(source.StorageKey))
                    return string.Empty;

                string[] split = source.StorageKey.Split(StorageEntityFrameworkCoreConstants.STORAGEKEY_DELIMITER);
                if (split.Length >= 3)
                    return split[2];
                return string.Empty;
            }
        }
    }
}