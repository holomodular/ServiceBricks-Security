namespace ServiceBricks.Security.Cosmos
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
                    d.StorageKey = s.Key.ToString();
                    d.UserStorageKey = s.UserId.ToString();
                });

            registry.Register<UserLoginDto, ApplicationUserLogin>(
                (s, d) =>
                {
                    d.LoginProvider = s.LoginProvider;
                    d.ProviderDisplayName = s.ProviderDisplayName;
                    d.ProviderKey = s.ProviderKey;
                    if (Guid.TryParse(s.StorageKey, out var tempKey))
                        d.Key = tempKey;
                    if (Guid.TryParse(s.UserStorageKey, out var tempUser))
                        d.UserId = tempUser;
                });
        }
    }
}