using AutoMapper;

using ServiceBricks.Storage.EntityFrameworkCore;

namespace ServiceBricks.Security.EntityFrameworkCore
{
    /// <summary>
    /// This is an automapper profile for the ApplicationUserLogin domain object.
    /// </summary>
    public class ApplicationUserLoginMappingProfile : Profile
    {
        public ApplicationUserLoginMappingProfile()
        {
            CreateMap<ApplicationUserLoginDto, ApplicationUserLogin>()
                .ForMember(x => x.LoginProvider, y => y.MapFrom<LoginProviderResolver>())
                .ForMember(x => x.ProviderKey, y => y.MapFrom<ProviderKeyResolver>())
                .ForMember(x => x.UserId, y => y.MapFrom<UserIdResolver>());

            CreateMap<ApplicationUserLogin, ApplicationUserLoginDto>()
                .ForMember(x => x.StorageKey, y => y.MapFrom(z =>
                    z.LoginProvider +
                    StorageEntityFrameworkCoreConstants.STORAGEKEY_DELIMITER +
                    z.ProviderKey))
                .ForMember(x => x.UserStorageKey, y => y.MapFrom(z => z.UserId));
        }

        public class LoginProviderResolver : IValueResolver<ApplicationUserLoginDto, object, string>
        {
            public string Resolve(ApplicationUserLoginDto source, object destination, string sourceMember, ResolutionContext context)
            {
                if (!string.IsNullOrEmpty(source.LoginProvider))
                    return source.LoginProvider;
                if (string.IsNullOrEmpty(source.StorageKey))
                    return string.Empty;

                string[] split = source.StorageKey.Split(StorageEntityFrameworkCoreConstants.STORAGEKEY_DELIMITER);
                if (split.Length >= 1)
                    return split[0];
                return string.Empty;
            }
        }

        public class ProviderKeyResolver : IValueResolver<ApplicationUserLoginDto, object, string>
        {
            public string Resolve(ApplicationUserLoginDto source, object destination, string sourceMember, ResolutionContext context)
            {
                if (!string.IsNullOrEmpty(source.ProviderKey))
                    return source.ProviderKey;
                if (string.IsNullOrEmpty(source.StorageKey))
                    return string.Empty;

                string[] split = source.StorageKey.Split(StorageEntityFrameworkCoreConstants.STORAGEKEY_DELIMITER);
                if (split.Length >= 2)
                    return split[1];
                return string.Empty;
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