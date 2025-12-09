namespace ServiceBricks.Security.MongoDb
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
                    d.StorageKey = s.Key;
                    d.UserStorageKey = s.UserId;
                    d.Value = s.Value;
                });

            registry.Register<UserTokenDto, ApplicationUserToken>(
                (s, d) =>
                {
                    d.LoginProvider = s.LoginProvider;
                    d.Name = s.Name;
                    d.Key = s.StorageKey;
                    d.UserId = s.UserStorageKey;
                    d.Value = s.Value;
                });
        }
    }
}