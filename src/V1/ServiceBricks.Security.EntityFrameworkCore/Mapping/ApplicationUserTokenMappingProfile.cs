using ServiceBricks.Storage.EntityFrameworkCore;

namespace ServiceBricks.Security.EntityFrameworkCore
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
                        StorageEntityFrameworkCoreConstants.STORAGEKEY_DELIMITER +
                        s.LoginProvider +
                        StorageEntityFrameworkCoreConstants.STORAGEKEY_DELIMITER +
                        s.Name;
                });

            registry.Register<UserTokenDto, ApplicationUserToken>(
                (s, d) =>
                {

                    d.LoginProvider = s.LoginProvider;
                    d.Name = s.Name;
                    if (Guid.TryParse(s.UserStorageKey, out var tempUserId))
                        d.UserId = tempUserId;                                        
                    d.Value = s.Value;

                    if (!string.IsNullOrEmpty(s.StorageKey))
                    {
                        string[] split = s.StorageKey.Split(StorageEntityFrameworkCoreConstants.STORAGEKEY_DELIMITER);
                        if (split.Length >= 1)
                        {
                            if (Guid.TryParse(split[0], out var tempUserId2))
                                d.UserId = tempUserId2;
                        }
                        if (split.Length >= 2)
                            d.LoginProvider = split[1];
                        if (split.Length >= 3)
                            d.Name = split[2];
                    }

                });
        }
    }
}