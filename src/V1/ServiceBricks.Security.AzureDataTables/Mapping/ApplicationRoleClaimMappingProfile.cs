using ServiceBricks.Storage.AzureDataTables;

namespace ServiceBricks.Security.AzureDataTables
{
    /// <summary>
    /// This is a omapper profile for the ApplicationRoleClaim domain object.
    /// </summary>
    public partial class ApplicationRoleClaimMappingProfile
    {
        /// <summary>
        /// Register the mapping
        /// </summary>
        public static void Register(IMapperRegistry registry)
        {
            registry.Register<ApplicationRoleClaim, RoleClaimDto>(
                (s, d) =>
                {
                    d.ClaimType = s.ClaimType;
                    d.ClaimValue = s.ClaimValue;                    
                    d.RoleStorageKey = s.RoleId.ToString();
                    d.StorageKey =
                        s.RoleId.ToString() +
                        StorageAzureDataTablesConstants.STORAGEKEY_DELIMITER +
                        s.Key.ToString();
                });

            registry.Register<RoleClaimDto, ApplicationRoleClaim>(
                (s, d) =>
                {                    
                    d.ClaimType = s.ClaimType;
                    d.ClaimValue = s.ClaimValue;                    
                    if (Guid.TryParse(s.RoleStorageKey, out var tempRoleId))
                        d.RoleId = tempRoleId;                    
                    if (!string.IsNullOrEmpty(s.StorageKey))
                    {
                        string[] split = s.StorageKey.Split(StorageAzureDataTablesConstants.STORAGEKEY_DELIMITER);
                        if (split.Length >= 1)
                        {
                            d.PartitionKey = split[0];
                            if (Guid.TryParse(split[0], out var tempRoleId2))
                                d.RoleId = tempRoleId2;
                        }
                        if (split.Length >= 2)
                        {
                            d.RowKey = split[1];
                            if (Guid.TryParse(split[1], out var tempKey))
                                d.Key = tempKey;
                        }
                    }
                });
        }
    }
}