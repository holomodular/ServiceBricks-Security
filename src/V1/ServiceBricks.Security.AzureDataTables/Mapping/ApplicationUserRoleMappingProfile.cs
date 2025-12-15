using ServiceBricks.Storage.AzureDataTables;

namespace ServiceBricks.Security.AzureDataTables
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
                        StorageAzureDataTablesConstants.STORAGEKEY_DELIMITER +
                        s.RoleId.ToString();                    
                });

            registry.Register<UserRoleDto, ApplicationUserRole>(
                (s, d) =>
                {
                    if (Guid.TryParse(s.UserStorageKey, out var tempUserStorageKey))
                        d.UserId = tempUserStorageKey;
                    if (Guid.TryParse(s.RoleStorageKey, out var tempRoleStorageKey))
                        d.RoleId = tempRoleStorageKey;

                    if (!string.IsNullOrEmpty(s.StorageKey))
                    {
                        string[] split = s.StorageKey.Split(StorageAzureDataTablesConstants.STORAGEKEY_DELIMITER);
                        if (split.Length >= 1)
                        {                     
                            d.PartitionKey = split[0];
                            if (Guid.TryParse(split[0], out var tempUserId))
                                d.UserId = tempUserId;
                        }
                        if (split.Length >= 2)
                        {
                            d.RowKey = split[1];
                            if (Guid.TryParse(split[1], out var tempRoleId))
                                d.RoleId = tempRoleId;
                        }
                    }                    
                });
        }
    }
}