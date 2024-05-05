using AutoMapper;
using ServiceBricks.Storage.EntityFrameworkCore;

namespace ServiceBricks.Security.Cosmos
{
    /// <summary>
    /// This is an automapper profile for the ApplicationUserRole domain object.
    /// </summary>
    public class ApplicationUserRoleMappingProfile : Profile
    {
        public ApplicationUserRoleMappingProfile()
        {
            CreateMap<ApplicationUserRoleDto, ApplicationUserRole>()
                .ForMember(x => x.UserId, y => y.MapFrom<UserIdResolver>())
                .ForMember(x => x.RoleId, y => y.MapFrom<RoleIdResolver>())
                .ForMember(x => x.User, y => y.Ignore())
                .ForMember(x => x.Role, y => y.Ignore());

            CreateMap<ApplicationUserRole, ApplicationUserRoleDto>()
                .ForMember(x => x.StorageKey, y => y.MapFrom(z => z.UserId +
                StorageEntityFrameworkCoreConstants.STORAGEKEY_DELIMITER +
                z.RoleId))
                .ForMember(x => x.UserStorageKey, y => y.MapFrom(z => z.UserId))
                .ForMember(x => x.RoleStorageKey, y => y.MapFrom(z => z.RoleId));
        }

        public class UserIdResolver : IValueResolver<ApplicationUserRoleDto, object, Guid>
        {
            public Guid Resolve(ApplicationUserRoleDto source, object destination, Guid sourceMember, ResolutionContext context)
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

        public class RoleIdResolver : IValueResolver<ApplicationUserRoleDto, object, Guid>
        {
            public Guid Resolve(ApplicationUserRoleDto source, object destination, Guid sourceMember, ResolutionContext context)
            {
                if (!string.IsNullOrEmpty(source.RoleStorageKey))
                {
                    Guid tempGuid;
                    if (Guid.TryParse(source.RoleStorageKey, out tempGuid))
                        return tempGuid;
                }
                if (string.IsNullOrEmpty(source.StorageKey))
                    return Guid.Empty;

                string[] split = source.StorageKey.Split(StorageEntityFrameworkCoreConstants.STORAGEKEY_DELIMITER);
                if (split.Length >= 2)
                {
                    Guid tempGuid;
                    if (Guid.TryParse(split[1], out tempGuid))
                        return tempGuid;
                }
                return Guid.Empty;
            }
        }
    }
}