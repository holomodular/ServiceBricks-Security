namespace ServiceBricks.Security.MongoDb
{
    /// <summary>
    /// This is a mapper profile for the ApplicationUserRole domain object.
    /// </summary>
    public partial class ApplicationUserRoleMappingProfile
    {
        /// <summary>
        /// Register the mapping
        /// </summary>
        public static void Register(IMapperRegistry registry)
        {
            registry.Register<ApplicationUserRole, UserRoleDto>(
                (s, d) =>
                {
                    d.RoleStorageKey = s.RoleId;
                    d.StorageKey = s.Key;
                    d.UserStorageKey = s.UserId;
                });

            registry.Register<UserRoleDto, ApplicationUserRole>(
                (s, d) =>
                {
                    d.Key = s.StorageKey;
                    d.RoleId = s.RoleStorageKey;
                    d.UserId = s.UserStorageKey;
                });
        }
    }
}