using ServiceBricks.Storage.EntityFrameworkCore;

namespace ServiceBricks.Security.EntityFrameworkCore
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
                        StorageEntityFrameworkCoreConstants.STORAGEKEY_DELIMITER +
                        s.ProviderKey;                    
                });

            registry.Register<UserLoginDto, ApplicationUserLogin>(
                (s, d) =>
                {                    
                    d.LoginProvider = s.LoginProvider;
                    d.ProviderKey = s.ProviderKey;                    
                    d.ProviderDisplayName = s.ProviderDisplayName;
                    if (Guid.TryParse(s.UserStorageKey, out var tempUserId))
                        d.UserId = tempUserId;

                    if (!string.IsNullOrEmpty(s.StorageKey))
                    {
                        string[] split = s.StorageKey.Split(StorageEntityFrameworkCoreConstants.STORAGEKEY_DELIMITER);
                        if (split.Length >= 1)
                            d.LoginProvider = split[0];
                        if (split.Length >= 2)
                            d.ProviderKey = split[1];
                    }

                });
        }
    }
}