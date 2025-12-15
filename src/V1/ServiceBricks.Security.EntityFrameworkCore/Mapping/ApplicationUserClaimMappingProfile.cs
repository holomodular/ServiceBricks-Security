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
                    d.UserStorageKey = s.UserId.ToString();
                    d.StorageKey = s.Id.ToString();
                });

            registry.Register<UserClaimDto, ApplicationUserClaim>(
                (s, d) =>
                {
                    d.ClaimType = s.ClaimType;
                    d.ClaimValue = s.ClaimValue;                                        
                    if (Guid.TryParse(s.UserStorageKey, out var tempUserId))
                        d.UserId = tempUserId;
                    if (int.TryParse(s.StorageKey, out var tempId))
                        d.Id = tempId;
                });
        }
    }
}