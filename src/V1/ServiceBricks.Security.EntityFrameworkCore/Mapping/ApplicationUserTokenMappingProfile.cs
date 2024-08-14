using AutoMapper;

using ServiceBricks.Storage.EntityFrameworkCore;

namespace ServiceBricks.Security.EntityFrameworkCore
{
    /// <summary>
    /// This is an automapper profile for the ApplicationUserToken domain object.
    /// </summary>
    public partial class ApplicationUserTokenMappingProfile : Profile
    {
        /// <summary>
        /// Constructor.
        /// </summary>
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

        /// <summary>
        /// Resolve the user id.
        /// </summary>
        public class UserIdResolver : IValueResolver<ApplicationUserTokenDto, object, Guid>
        {
            /// <summary>
            /// Resolve the user id.
            /// </summary>
            /// <param name="source"></param>
            /// <param name="destination"></param>
            /// <param name="sourceMember"></param>
            /// <param name="context"></param>
            /// <returns></returns>
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

        /// <summary>
        /// Resolve the login provider.
        /// </summary>
        public class LoginProviderResolver : IValueResolver<ApplicationUserTokenDto, object, string>
        {
            /// <summary>
            /// Resolve the login provider.
            /// </summary>
            /// <param name="source"></param>
            /// <param name="destination"></param>
            /// <param name="sourceMember"></param>
            /// <param name="context"></param>
            /// <returns></returns>
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

        /// <summary>
        /// Resolve the name.
        /// </summary>
        public class NameResolver : IValueResolver<ApplicationUserTokenDto, object, string>
        {
            /// <summary>
            /// Resolve the name.
            /// </summary>
            /// <param name="source"></param>
            /// <param name="destination"></param>
            /// <param name="sourceMember"></param>
            /// <param name="context"></param>
            /// <returns></returns>
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