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
                    d.StorageKey =
                        s.UserId.ToString() +
                        StorageAzureDataTablesConstants.STORAGEKEY_DELIMITER +
                        s.LoginProvider +
                        StorageAzureDataTablesConstants.KEY_DELIMITER +
                        s.Name;
                    d.UserStorageKey = s.UserId.ToString();
                    d.Value = s.Value;
                });

            registry.Register<UserTokenDto, ApplicationUserToken>(
                (s, d) =>
                {
                    if (!string.IsNullOrEmpty(s.StorageKey))
                    {
                        string[] split = s.StorageKey.Split(StorageAzureDataTablesConstants.STORAGEKEY_DELIMITER);
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
                            string[] splitRowKey = split[1].Split(StorageAzureDataTablesConstants.KEY_DELIMITER);
                            if (splitRowKey.Length >= 1)
                                d.LoginProvider = splitRowKey[0];
                            else
                                d.LoginProvider = string.Empty;
                            if (splitRowKey.Length >= 2)
                                d.Name = splitRowKey[1];
                            else
                                d.Name = string.Empty;
                        }
                        else
                        {
                            d.LoginProvider = string.Empty;
                            d.Name = string.Empty;
                        }
                    }
                    else
                    {
                        d.LoginProvider = s.LoginProvider;
                        d.Name = s.Name;
                        if (!string.IsNullOrEmpty(s.UserStorageKey))
                        {
                            Guid tempUserId;
                            if (Guid.TryParse(s.UserStorageKey, out tempUserId))
                                d.UserId = tempUserId;
                            else
                                d.UserId = Guid.Empty;
                        }
                    }
                    d.Value = s.Value;
                    d.PartitionKey = d.UserId.ToString();
                    d.RowKey =
                        d.LoginProvider +
                        StorageAzureDataTablesConstants.KEY_DELIMITER +
                        d.Name;
                });
        }
    }
}