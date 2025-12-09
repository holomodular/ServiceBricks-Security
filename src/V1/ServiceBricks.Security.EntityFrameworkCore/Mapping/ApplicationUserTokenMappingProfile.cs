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
                    d.StorageKey =
                        s.UserId.ToString() +
                        StorageEntityFrameworkCoreConstants.STORAGEKEY_DELIMITER +
                        s.LoginProvider +
                        StorageEntityFrameworkCoreConstants.STORAGEKEY_DELIMITER +
                        s.Name;
                    d.UserStorageKey = s.UserId.ToString();
                    d.Value = s.Value;
                });

            registry.Register<UserTokenDto, ApplicationUserToken>(
                (s, d) =>
                {
                    if (!string.IsNullOrEmpty(s.StorageKey))
                    {
                        string[] split = s.StorageKey.Split(StorageEntityFrameworkCoreConstants.STORAGEKEY_DELIMITER);
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
                            d.LoginProvider = split[1];
                        else
                            d.LoginProvider = string.Empty;
                        if (split.Length >= 3)
                            d.Name = split[2];
                        else
                            d.Name = string.Empty;
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
                });
        }
    }
}