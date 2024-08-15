using AutoMapper;

using ServiceBricks.Storage.EntityFrameworkCore;

namespace ServiceBricks.Security.EntityFrameworkCore
{
    /// <summary>
    /// This is an automapper profile for the ApplicationUserLogin domain object.
    /// </summary>
    public partial class ApplicationUserLoginMappingProfile : Profile
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public ApplicationUserLoginMappingProfile()
        {
            CreateMap<UserLoginDto, ApplicationUserLogin>()
                .ForMember(x => x.LoginProvider, y => y.MapFrom<LoginProviderResolver>())
                .ForMember(x => x.ProviderKey, y => y.MapFrom<ProviderKeyResolver>())
                .ForMember(x => x.UserId, y => y.MapFrom<UserIdResolver>());

            CreateMap<ApplicationUserLogin, UserLoginDto>()
                .ForMember(x => x.StorageKey, y => y.MapFrom(z =>
                    z.LoginProvider +
                    StorageEntityFrameworkCoreConstants.STORAGEKEY_DELIMITER +
                    z.ProviderKey))
                .ForMember(x => x.UserStorageKey, y => y.MapFrom(z => z.UserId));
        }

        /// <summary>
        /// Resolve the login provider.
        /// </summary>
        public class LoginProviderResolver : IValueResolver<UserLoginDto, object, string>
        {
            /// <summary>
            /// Resolve the login provider.
            /// </summary>
            /// <param name="source"></param>
            /// <param name="destination"></param>
            /// <param name="sourceMember"></param>
            /// <param name="context"></param>
            /// <returns></returns>
            public string Resolve(UserLoginDto source, object destination, string sourceMember, ResolutionContext context)
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

        /// <summary>
        /// Resolve the provider key.
        /// </summary>
        public class ProviderKeyResolver : IValueResolver<UserLoginDto, object, string>
        {
            /// <summary>
            /// Resolve the provider key.
            /// </summary>
            /// <param name="source"></param>
            /// <param name="destination"></param>
            /// <param name="sourceMember"></param>
            /// <param name="context"></param>
            /// <returns></returns>
            public string Resolve(UserLoginDto source, object destination, string sourceMember, ResolutionContext context)
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

        /// <summary>
        /// Resolve the user id.
        /// </summary>
        public class UserIdResolver : IValueResolver<UserLoginDto, object, Guid>
        {
            /// <summary>
            /// Resolve the user id.
            /// </summary>
            /// <param name="source"></param>
            /// <param name="destination"></param>
            /// <param name="sourceMember"></param>
            /// <param name="context"></param>
            /// <returns></returns>
            public Guid Resolve(UserLoginDto source, object destination, Guid sourceMember, ResolutionContext context)
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