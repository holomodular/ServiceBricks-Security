using AutoMapper;

using ServiceBricks.Storage.EntityFrameworkCore;

namespace ServiceBricks.Security.EntityFrameworkCore
{
    /// <summary>
    /// This is an automapper profile for the UserSecurity domain object.
    /// </summary>
    public class AuditUserMappingProfile : Profile
    {
        public AuditUserMappingProfile()
        {
            CreateMap<AuditUserDto, AuditUser>()
                .ForMember(x => x.CreateDate, y => y.Ignore())
                .ForMember(x => x.Key, y => y.MapFrom<KeyResolver>())
                .ForMember(x => x.UserId, y => y.MapFrom<UserIdResolver>());

            CreateMap<AuditUser, AuditUserDto>()
                .ForMember(x => x.StorageKey, y => y.MapFrom(z => z.Key))
                .ForMember(x => x.UserStorageKey, y => y.MapFrom(z => z.UserId));
        }

        public class KeyResolver : IValueResolver<DataTransferObject, object, long>
        {
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

        public class UserIdResolver : IValueResolver<AuditUserDto, object, Guid>
        {
            public Guid Resolve(AuditUserDto source, object destination, Guid sourceMember, ResolutionContext context)
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