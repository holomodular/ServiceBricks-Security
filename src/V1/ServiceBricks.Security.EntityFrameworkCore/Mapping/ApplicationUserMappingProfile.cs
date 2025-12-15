namespace ServiceBricks.Security.EntityFrameworkCore
{
    /// <summary>
    /// This is an automapper profile for the ApplicationUser domain object.
    /// </summary>
    public partial class ApplicationUserMappingProfile
    {
        /// <summary>
        /// Register the mapping
        /// </summary>
        public static void Register(IMapperRegistry registry)
        {
            registry.Register<ApplicationUser, UserDto>(
                (s, d) =>
                {
                    d.AccessFailedCount = s.AccessFailedCount;
                    d.ConcurrencyStamp = s.ConcurrencyStamp;
                    d.CreateDate = s.CreateDate;
                    d.Email = s.Email;
                    d.EmailConfirmed = s.EmailConfirmed;
                    d.LockoutEnabled = s.LockoutEnabled;
                    d.LockoutEnd = s.LockoutEnd;
                    d.NormalizedEmail = s.NormalizedEmail;
                    d.NormalizedUserName = s.NormalizedUserName;
                    d.PasswordHash = s.PasswordHash;
                    d.PhoneNumber = s.PhoneNumber;
                    d.PhoneNumberConfirmed = s.PhoneNumberConfirmed;
                    d.SecurityStamp = s.SecurityStamp;                    
                    d.TwoFactorEnabled = s.TwoFactorEnabled;
                    d.UpdateDate = s.UpdateDate;
                    d.UserName = s.UserName;
                    d.StorageKey = s.Id.ToString();
                });

            registry.Register<UserDto, ApplicationUser>(
                (s, d) =>
                {
                    d.AccessFailedCount = s.AccessFailedCount;
                    d.ConcurrencyStamp = s.ConcurrencyStamp;
                    //d.CreateDate ignore
                    d.Email = s.Email;
                    d.EmailConfirmed = s.EmailConfirmed;
                    d.LockoutEnabled = s.LockoutEnabled;
                    d.LockoutEnd = s.LockoutEnd;
                    d.NormalizedEmail = s.NormalizedEmail;
                    d.NormalizedUserName = s.NormalizedUserName;
                    d.PasswordHash = s.PasswordHash;
                    d.PhoneNumber = s.PhoneNumber;
                    d.PhoneNumberConfirmed = s.PhoneNumberConfirmed;
                    d.SecurityStamp = s.SecurityStamp;                    
                    d.TwoFactorEnabled = s.TwoFactorEnabled;
                    d.UpdateDate = s.UpdateDate;
                    d.UserName = s.UserName;
                    if (Guid.TryParse(s.StorageKey, out var tempId))
                        d.Id = tempId;
                });
        }
    }
}