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
                        s.PartitionKey +
                        StorageAzureDataTablesConstants.STORAGEKEY_DELIMITER +
                        s.RowKey;
                });

            registry.Register<RoleClaimDto, ApplicationRoleClaim>(
                (s, d) =>
                {
                    if (!string.IsNullOrEmpty(s.StorageKey))
                    {
                        string[] split = s.StorageKey.Split(StorageAzureDataTablesConstants.STORAGEKEY_DELIMITER);
                        if (split.Length >= 1)
                            d.PartitionKey = split[0];
                        else
                            d.PartitionKey = string.Empty;
                        if (split.Length >= 2)
                            d.RowKey = split[1];
                        else
                            d.RowKey = string.Empty;
                    }
                    d.ClaimType = s.ClaimType;
                    d.ClaimValue = s.ClaimValue;
                    Guid tempRoleId;
                    if (Guid.TryParse(s.RoleStorageKey, out tempRoleId))
                        d.RoleId = tempRoleId;
                });
        }
    }
}