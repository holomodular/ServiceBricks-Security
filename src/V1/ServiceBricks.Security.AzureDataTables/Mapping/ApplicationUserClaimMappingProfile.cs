using Azure.Messaging.ServiceBus;
using ServiceBricks.Storage.AzureDataTables;

namespace ServiceBricks.Security.AzureDataTables
{
    /// <summary>
    /// This is a mapper profile for the ApplicationUserClaim domain object.
    /// </summary>
    public partial class ApplicationUserClaimMappingProfile
    {
        /// <summary>
        /// Register the mapping
        /// </summary>
        public static void Register(IMapperRegistry registry)
        {
            registry.Register<ApplicationUserClaim, UserClaimDto>(
                (s, d) =>
                {
                    d.ClaimType = s.ClaimType;
                    d.ClaimValue = s.ClaimValue;
                    d.UserStorageKey = s.UserId.ToString();
                    d.StorageKey =
                        s.UserId.ToString() +
                        StorageAzureDataTablesConstants.STORAGEKEY_DELIMITER +
                        s.Key.ToString();
                    
                });

            registry.Register<UserClaimDto, ApplicationUserClaim>(
                (s, d) =>
                {                    
                    d.ClaimType = s.ClaimType;
                    d.ClaimValue = s.ClaimValue;
                    Guid tempUserId;
                    if (Guid.TryParse(s.UserStorageKey, out tempUserId))
                        d.UserId = tempUserId;

                    if (!string.IsNullOrEmpty(s.StorageKey))
                    {
                        string[] split = s.StorageKey.Split(StorageAzureDataTablesConstants.STORAGEKEY_DELIMITER);
                        if (split.Length >= 1)
                        {                            
                            d.PartitionKey = split[0];
                            if (Guid.TryParse(split[0], out var tempUser))
                                d.UserId = tempUser;                                                            
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