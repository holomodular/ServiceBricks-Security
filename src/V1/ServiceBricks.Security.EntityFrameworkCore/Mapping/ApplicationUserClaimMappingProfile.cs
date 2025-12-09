namespace ServiceBricks.Security.EntityFrameworkCore
{
    /// <summary>
    /// This is an automapper profile for the ApplicationUserClaim domain object.
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
                    d.StorageKey = s.Id.ToString();
                    d.UserStorageKey = s.UserId.ToString();
                });

            registry.Register<UserClaimDto, ApplicationUserClaim>(
                (s, d) =>
                {
                    d.ClaimType = s.ClaimType;
                    d.ClaimValue = s.ClaimValue;
                    int tempId;
                    if (int.TryParse(s.StorageKey, out tempId))
                        d.Id = tempId;
                    Guid tempUserId;
                    if (Guid.TryParse(s.UserStorageKey, out tempUserId))
                        d.UserId = tempUserId;
                });
        }
    }
}