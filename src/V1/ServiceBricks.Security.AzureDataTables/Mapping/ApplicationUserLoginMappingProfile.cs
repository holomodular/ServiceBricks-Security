using ServiceBricks.Storage.AzureDataTables;

namespace ServiceBricks.Security.AzureDataTables
{
    /// <summary>
    /// This is a mapper profile for the ApplicationUserLogin domain object.
    /// </summary>
    public partial class ApplicationUserLoginMappingProfile
    {
        /// <summary>
        /// Register the mapping
        /// </summary>
        public static void Register(IMapperRegistry registry)
        {
            registry.Register<ApplicationUserLogin, UserLoginDto>(
                (s, d) =>
                {
                    d.LoginProvider = s.LoginProvider;
                    d.ProviderDisplayName = s.ProviderDisplayName;
                    d.ProviderKey = s.ProviderKey;                    
                    d.UserStorageKey = s.UserId.ToString();
                    d.StorageKey =
                        s.LoginProvider +
                        StorageAzureDataTablesConstants.STORAGEKEY_DELIMITER +
                        s.ProviderKey;
                });

            registry.Register<UserLoginDto, ApplicationUserLogin>(
                (s, d) =>
                {
                    d.ProviderDisplayName = s.ProviderDisplayName;
                    d.LoginProvider = s.LoginProvider;
                    d.ProviderKey = s.ProviderKey;
                    if (Guid.TryParse(s.UserStorageKey, out var tempUserId))
                        d.UserId = tempUserId;
                    
                    if (!string.IsNullOrEmpty(s.StorageKey))
                    {
                        string[] split = s.StorageKey.Split(StorageAzureDataTablesConstants.STORAGEKEY_DELIMITER);
                        if (split.Length >= 1)
                        {
                            d.PartitionKey = split[0];
                            d.LoginProvider = split[0];                            
                        }
                        if (split.Length >= 2)
                        {
                            d.RowKey = split[1];
                            d.ProviderKey = split[1];                            
                        }
                    }                    
                });
        }
    }
}