namespace ServiceBricks.Security.MongoDb
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
                    d.StorageKey = s.Key;
                    d.UserStorageKey = s.UserId;
                });

            registry.Register<ApplicationUserClaim, ApplicationIdentityUserClaim>(
                (s, d) =>
                {
                    d.ClaimType = s.ClaimType;
                    d.ClaimValue = s.ClaimValue;
                    d.Key = s.Key;
                    d.UserId = s.UserId;
                });

            registry.Register<ApplicationIdentityUserClaim, UserClaimDto>(
                (s, d) =>
                {
                    d.ClaimType = s.ClaimType;
                    d.ClaimValue = s.ClaimValue;
                    d.StorageKey = s.Key;
                    d.UserStorageKey = s.UserId;
                });

            registry.Register<UserClaimDto, ApplicationUserClaim>(
                (s, d) =>
                {
                    d.ClaimType = s.ClaimType;
                    d.ClaimValue = s.ClaimValue;
                    d.UserId = s.UserStorageKey;
                    d.Key = s.StorageKey;
                });

            registry.Register<ApplicationIdentityUserClaim, ApplicationUserClaim>(
                (s, d) =>
                {
                    d.ClaimType = s.ClaimType;
                    d.ClaimValue = s.ClaimValue;
                    d.Key = s.Key;
                    d.UserId = s.UserId;
                });

            registry.Register<UserClaimDto, ApplicationIdentityUserClaim>(
                (s, d) =>
                {
                    d.ClaimType = s.ClaimType;
                    d.ClaimValue = s.ClaimValue;
                    d.UserId = s.UserStorageKey;
                    d.Key = s.StorageKey;
                });
        }
    }
}