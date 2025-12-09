namespace ServiceBricks.Security.MongoDb
{
    /// <summary>
    /// This is a mapper profile for the ApplicationRole domain object.
    /// </summary>
    public partial class ApplicationRoleMappingProfile
    {
        /// <summary>
        /// Register the mapping
        /// </summary>
        public static void Register(IMapperRegistry registry)
        {
            registry.Register<ApplicationRole, RoleDto>(
                (s, d) =>
                {
                    d.ConcurrencyStamp = s.ConcurrencyStamp;
                    d.Name = s.Name;
                    d.NormalizedName = s.NormalizedName;
                    d.StorageKey = s.Id;
                });

            registry.Register<ApplicationRole, ApplicationIdentityRole>(
                (s, d) =>
                {
                    d.ConcurrencyStamp = s.ConcurrencyStamp;
                    d.Name = s.Name;
                    d.NormalizedName = s.NormalizedName;
                    d.Id = s.Id;
                });

            registry.Register<ApplicationIdentityRole, RoleDto>(
                (s, d) =>
                {
                    d.ConcurrencyStamp = s.ConcurrencyStamp;
                    d.Name = s.Name;
                    d.NormalizedName = s.NormalizedName;
                    d.StorageKey = s.Id;
                });

            registry.Register<RoleDto, ApplicationRole>(
                (s, d) =>
                {
                    d.ConcurrencyStamp = s.ConcurrencyStamp;
                    d.Name = s.Name;
                    d.NormalizedName = s.NormalizedName;
                    d.Id = s.StorageKey;
                });

            registry.Register<ApplicationIdentityRole, ApplicationRole>(
                (s, d) =>
                {
                    d.ConcurrencyStamp = s.ConcurrencyStamp;
                    d.Name = s.Name;
                    d.NormalizedName = s.NormalizedName;
                    d.Id = s.Id;
                });

            registry.Register<RoleDto, ApplicationIdentityRole>(
                (s, d) =>
                {
                    d.ConcurrencyStamp = s.ConcurrencyStamp;
                    d.Name = s.Name;
                    d.NormalizedName = s.NormalizedName;
                    d.Id = s.StorageKey;
                });
        }
    }
}