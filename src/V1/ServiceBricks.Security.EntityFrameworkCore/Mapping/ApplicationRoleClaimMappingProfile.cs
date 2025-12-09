namespace ServiceBricks.Security.EntityFrameworkCore
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
                    d.StorageKey = s.Id.ToString();
                });

            registry.Register<RoleClaimDto, ApplicationRoleClaim>(
                (s, d) =>
                {
                    d.ClaimType = s.ClaimType;
                    d.ClaimValue = s.ClaimValue;
                    Guid tempRoleId;
                    if (Guid.TryParse(s.RoleStorageKey, out tempRoleId))
                        d.RoleId = tempRoleId;
                    int tempId;
                    if (int.TryParse(s.StorageKey, out tempId))
                        d.Id = tempId;
                });
        }
    }
}