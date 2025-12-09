namespace ServiceBricks.Security.MongoDb
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
                    d.StorageKey = s.Key;
                    d.UserStorageKey = s.UserId;
                });

            registry.Register<UserLoginDto, ApplicationUserLogin>(
                (s, d) =>
                {
                    d.LoginProvider = s.LoginProvider;
                    d.ProviderKey = s.ProviderKey;
                    d.ProviderDisplayName = s.ProviderDisplayName;
                    d.UserId = s.UserStorageKey;
                    d.Key = s.StorageKey;
                });
        }
    }
}