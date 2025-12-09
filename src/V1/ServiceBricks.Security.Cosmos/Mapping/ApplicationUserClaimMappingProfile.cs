namespace ServiceBricks.Security.Cosmos
{
    /// <summary>
    /// This is a mapper profile for the ApplicationUserClaim domain object.
    /// </summary>
    public partial class ApplicationUserClaimMappingProfile
    {
        /// <summary>
        /// Register the mapping
        /// </summary>
        public static void Register(IMapperRegistry registry)
        {
            registry.Register<ApplicationUserClaim, UserClaimDto>(
                (s, d) =>
                {
                    d.ClaimType = s.ClaimType;
                    d.ClaimValue = s.ClaimValue;
                    d.StorageKey = s.Key.ToString();
                    d.UserStorageKey = s.UserId.ToString();
                });

            registry.Register<UserClaimDto, ApplicationUserClaim>(
                (s, d) =>
                {
                    d.ClaimType = s.ClaimType;
                    d.ClaimValue = s.ClaimValue;
                    Guid tempKey;
                    if (Guid.TryParse(s.StorageKey, out tempKey))
                        d.Key = tempKey;
                    Guid tempUserId;
                    if (Guid.TryParse(s.UserStorageKey, out tempUserId))
                        d.UserId = tempUserId;
                });
        }
    }
}