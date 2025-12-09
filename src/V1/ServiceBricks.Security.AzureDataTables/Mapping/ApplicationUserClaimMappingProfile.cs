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
                    d.StorageKey =
                        s.UserId.ToString() +
                        StorageAzureDataTablesConstants.STORAGEKEY_DELIMITER +
                        s.Key.ToString();
                    d.UserStorageKey = s.UserId.ToString();
                });

            registry.Register<UserClaimDto, ApplicationUserClaim>(
                (s, d) =>
                {
                    if (!string.IsNullOrEmpty(s.StorageKey))
                    {
                        string[] split = s.StorageKey.Split(StorageAzureDataTablesConstants.STORAGEKEY_DELIMITER);
                        if (split.Length >= 1)
                        {
                            Guid tempUser;
                            if (Guid.TryParse(split[0], out tempUser))
                                d.UserId = tempUser;
                        }
                        if (split.Length >= 2)
                        {
                            Guid tempKey;
                            if (Guid.TryParse(split[1], out tempKey))
                                d.Key = tempKey;
                        }
                    }

                    d.ClaimType = s.ClaimType;
                    d.ClaimValue = s.ClaimValue;
                    Guid tempUserId;
                    if (Guid.TryParse(s.UserStorageKey, out tempUserId))
                        d.UserId = tempUserId;
                    d.PartitionKey = d.UserId.ToString();
                    d.RowKey = d.Key.ToString();
                });
        }
    }
}