using ServiceBricks.Storage.AzureDataTables;

namespace ServiceBricks.Security.AzureDataTables
{
    /// <summary>
    /// This is a mapper profile for the ApplicationUserToken domain object.
    /// </summary>
    public partial class ApplicationUserTokenMappingProfile
    {
        /// <summary>
        /// Register the mapping
        /// </summary>
        public static void Register(IMapperRegistry registry)
        {
            registry.Register<ApplicationUserToken, UserTokenDto>(
                (s, d) =>
                {
                    d.LoginProvider = s.LoginProvider;
                    d.Name = s.Name;                    
                    d.UserStorageKey = s.UserId.ToString();
                    d.Value = s.Value;
                    d.StorageKey =
                        s.UserId.ToString() +
                        StorageAzureDataTablesConstants.STORAGEKEY_DELIMITER +
                        s.LoginProvider +
                        StorageAzureDataTablesConstants.KEY_DELIMITER +
                        s.Name;

                });

            registry.Register<UserTokenDto, ApplicationUserToken>(
                (s, d) =>
                {
                    d.LoginProvider = s.LoginProvider;
                    d.Name = s.Name;
                    d.Value = s.Value;
                    if(Guid.TryParse(s.UserStorageKey, out var tempUserStorageKey))
                        d.UserId = tempUserStorageKey;

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
                            string[] splitRowKey = split[1].Split(StorageAzureDataTablesConstants.KEY_DELIMITER);
                            if (splitRowKey.Length >= 1)
                                d.LoginProvider = splitRowKey[0];
                            if (splitRowKey.Length >= 2)
                                d.Name = splitRowKey[1];
                        }
                        
                    }                    
                });
        }
    }
}