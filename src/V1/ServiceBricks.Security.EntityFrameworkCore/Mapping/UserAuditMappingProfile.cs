using AutoMapper;

using ServiceBricks.Storage.EntityFrameworkCore;

namespace ServiceBricks.Security.EntityFrameworkCore
{
    /// <summary>
    /// This is an automapper profile for the UserSecurity domain object.
    /// </summary>
    public partial class UserAuditMappingProfile : Profile
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public UserAuditMappingProfile()
        {
            CreateMap<UserAuditDto, UserAudit>()
                .ForMember(x => x.CreateDate, y => y.Ignore())
                .ForMember(x => x.Key, y => y.MapFrom<KeyResolver>())
                .ForMember(x => x.UserId, y => y.MapFrom<UserIdResolver>());

            CreateMap<UserAudit, UserAuditDto>()
                .ForMember(x => x.StorageKey, y => y.MapFrom(z => z.Key))
                .ForMember(x => x.UserStorageKey, y => y.MapFrom(z => z.UserId));
        }

        /// <summary>
        /// Resolve the key.
        /// </summary>
        public class KeyResolver : IValueResolver<DataTransferObject, object, long>
        {
            /// <summary>
            /// Resolve the key.
            /// </summary>
            /// <param name="source"></param>
            /// <param name="destination"></param>
            /// <param name="sourceMember"></param>
            /// <param name="context"></param>
            /// <returns></returns>
            public long Resolve(DataTransferObject source, object destination, long sourceMember, ResolutionContext context)
            {
                if (string.IsNullOrEmpty(source.StorageKey))
                    return 0;

                long tempKey;
                if (long.TryParse(source.StorageKey, out tempKey))
                    return tempKey;
                return 0;
            }
        }

        /// <summary>
        /// Resolve the user id.
        /// </summary>
        public class UserIdResolver : IValueResolver<UserAuditDto, object, Guid>
        {
            /// <summary>
            /// Resolve the user id.
            /// </summary>
            /// <param name="source"></param>
            /// <param name="destination"></param>
            /// <param name="sourceMember"></param>
            /// <param name="context"></param>
            /// <returns></returns>
            public Guid Resolve(UserAuditDto source, object destination, Guid sourceMember, ResolutionContext context)
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
    }
}