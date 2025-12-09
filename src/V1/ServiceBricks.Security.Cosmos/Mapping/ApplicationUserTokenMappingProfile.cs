namespace ServiceBricks.Security.Cosmos
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
                    d.StorageKey = s.Key.ToString();
                    d.UserStorageKey = s.UserId.ToString();
                    d.Value = s.Value;
                });

            registry.Register<UserTokenDto, ApplicationUserToken>(
                (s, d) =>
                {
                    d.LoginProvider = s.LoginProvider;
                    d.Name = s.Name;
                    Guid tempKey;
                    if (Guid.TryParse(s.StorageKey, out tempKey))
                        d.Key = tempKey;
                    Guid tempUser;
                    if (Guid.TryParse(s.UserStorageKey, out tempUser))
                        d.UserId = tempUser;
                    d.Value = s.Value;
                });
        }
    }
}