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
                    d.StorageKey =
                        s.UserId.ToString() +
                        StorageEntityFrameworkCoreConstants.STORAGEKEY_DELIMITER +
                        s.RoleId.ToString();
                    d.UserStorageKey = s.UserId.ToString();
                });

            registry.Register<UserRoleDto, ApplicationUserRole>(
                (s, d) =>
                {
                    if (!string.IsNullOrEmpty(s.StorageKey))
                    {
                        string[] split = s.StorageKey.Split(StorageEntityFrameworkCoreConstants.STORAGEKEY_DELIMITER);
                        if (split.Length >= 1)
                        {
                            Guid tempUserId;
                            if (Guid.TryParse(split[0], out tempUserId))
                                d.UserId = tempUserId;
                            else
                                d.UserId = Guid.Empty;
                        }
                        else
                            d.UserId = Guid.Empty;
                        if (split.Length >= 2)
                        {
                            Guid tempRoleId;
                            if (Guid.TryParse(split[1], out tempRoleId))
                                d.RoleId = tempRoleId;
                            else
                                d.RoleId = Guid.Empty;
                        }
                        else
                            d.RoleId = Guid.Empty;
                    }
                    else
                    {
                        Guid tempRoleId;
                        if (Guid.TryParse(s.RoleStorageKey, out tempRoleId))
                            d.RoleId = tempRoleId;
                        else
                            d.RoleId = Guid.Empty;

                        Guid tempUserId;
                        if (Guid.TryParse(s.UserStorageKey, out tempUserId))
                            d.UserId = tempUserId;
                        else
                            d.UserId = Guid.Empty;
                    }
                });
        }
    }
}