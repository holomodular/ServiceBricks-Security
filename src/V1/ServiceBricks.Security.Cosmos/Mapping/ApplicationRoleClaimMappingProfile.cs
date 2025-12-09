namespace ServiceBricks.Security.Cosmos
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
                    d.RoleStorageKey = s.RoleId.ToString();
                    d.StorageKey = s.Key.ToString();
                });

            registry.Register<RoleClaimDto, ApplicationRoleClaim>(
                (s, d) =>
                {
                    d.ClaimType = s.ClaimType;
                    d.ClaimValue = s.ClaimValue;
                    Guid tempRoleId;
                    if (Guid.TryParse(s.RoleStorageKey, out tempRoleId))
                        d.RoleId = tempRoleId;
                    Guid tempKey;
                    if (Guid.TryParse(s.StorageKey, out tempKey))
                        d.Key = tempKey;
                });
        }
    }
}