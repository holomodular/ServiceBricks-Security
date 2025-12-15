namespace ServiceBricks.Security.MongoDb
{
    /// <summary>
    /// This is a mapper profile for the ApplicationRoleClaim domain object.
    /// </summary>
    public partial class ApplicationRoleClaimMappingProfile
    {
        /// <summary>
        /// Register the mapping
        /// </summary>
        public static void Register(IMapperRegistry registry)
        {
            registry.Register<ApplicationRoleClaim, RoleClaimDto>(
                (s, d) =>
                {
                    d.ClaimType = s.ClaimType;
                    d.ClaimValue = s.ClaimValue;
                    d.RoleStorageKey = s.RoleId;
                    d.StorageKey = s.Key;
                });

            registry.Register<ApplicationRoleClaim, ApplicationIdentityRoleClaim>(
                (s, d) =>
                {
                    d.ClaimType = s.ClaimType;
                    d.ClaimValue = s.ClaimValue;
                    d.RoleId = s.RoleId;
                    if (int.TryParse(s.Key, out var tempId))
                        d.Id = tempId;
                });

            registry.Register<ApplicationIdentityRoleClaim, RoleClaimDto>(
                (s, d) =>
                {
                    d.ClaimType = s.ClaimType;
                    d.ClaimValue = s.ClaimValue;
                    d.RoleStorageKey = s.RoleId;
                    d.StorageKey = s.Id.ToString();
                });

            registry.Register<ApplicationRoleClaim, RoleClaimDto>(
                (s, d) =>
                {
                    d.ClaimType = s.ClaimType;
                    d.ClaimValue = s.ClaimValue;
                    d.RoleStorageKey = s.RoleId;
                    d.StorageKey = s.Key;
                });

            registry.Register<RoleClaimDto, ApplicationRoleClaim>(
                (s, d) =>
                {
                    d.ClaimType = s.ClaimType;
                    d.ClaimValue = s.ClaimValue;
                    d.RoleId = s.RoleStorageKey;
                    d.Key = s.StorageKey;
                });

            registry.Register<RoleClaimDto, ApplicationIdentityRoleClaim>(
                (s, d) =>
                {
                    d.ClaimType = s.ClaimType;
                    d.ClaimValue = s.ClaimValue;
                    d.RoleId = s.RoleStorageKey;
                    d.Key = s.StorageKey;
                });

            registry.Register<ApplicationIdentityRoleClaim, ApplicationRoleClaim>(
                (s, d) =>
                {
                    d.ClaimType = s.ClaimType;
                    d.ClaimValue = s.ClaimValue;
                    d.RoleId = s.RoleId;
                    d.Key = s.Key;
                });
        }
    }
}