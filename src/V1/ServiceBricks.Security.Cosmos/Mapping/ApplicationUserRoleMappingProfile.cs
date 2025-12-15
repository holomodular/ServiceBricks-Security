using ServiceBricks.Storage.EntityFrameworkCore;

namespace ServiceBricks.Security.Cosmos
{
    /// <summary>
    /// This is a mapper profile for the ApplicationUserRole domain object.
    /// </summary>
    public partial class ApplicationUserRoleMappingProfile
    {
        /// <summary>
        /// Register the mapping
        /// </summary>
        public static void Register(IMapperRegistry registry)
        {
            registry.Register<ApplicationUserRole, UserRoleDto>(
                (s, d) =>
                {
                    d.RoleStorageKey = s.RoleId.ToString();                    
                    d.UserStorageKey = s.UserId.ToString();
                    d.StorageKey =
                        s.UserId.ToString() +
                        StorageEntityFrameworkCoreConstants.STORAGEKEY_DELIMITER +
                        s.RoleId.ToString();
                });

            registry.Register<UserRoleDto, ApplicationUserRole>(
                (s, d) =>
                {                    
                    if (Guid.TryParse(s.RoleStorageKey, out var tempRoleId))
                        d.RoleId = tempRoleId;
                    if (Guid.TryParse(s.UserStorageKey, out var tempUserId))
                        d.UserId = tempUserId;

                    if (!string.IsNullOrEmpty(s.StorageKey))
                    {
                        string[] split = s.StorageKey.Split(StorageEntityFrameworkCoreConstants.STORAGEKEY_DELIMITER);
                        if (split.Length >= 1)
                        {
                            if (Guid.TryParse(split[0], out var tempUserId2))
                                d.UserId = tempUserId2;
                        }
                        if (split.Length >= 2)
                        {
                            if (Guid.TryParse(split[1], out var tempRoleId2))
                                d.RoleId = tempRoleId2;
                        }
                    }

                });
        }
    }
}