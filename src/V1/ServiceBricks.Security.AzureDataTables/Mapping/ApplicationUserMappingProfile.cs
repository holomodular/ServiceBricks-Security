namespace ServiceBricks.Security.AzureDataTables
{
    /// <summary>
    /// This is a mapper profile for the ApplicationUser domain object.
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
                    d.StorageKey = s.Id.ToString();
                    d.TwoFactorEnabled = s.TwoFactorEnabled;
                    d.UpdateDate = s.UpdateDate;
                    d.UserName = s.UserName;
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
                    Guid tempId;
                    if (Guid.TryParse(s.StorageKey, out tempId))
                        d.Id = tempId;
                    d.TwoFactorEnabled = s.TwoFactorEnabled;
                    d.UpdateDate = s.UpdateDate;
                    d.UserName = s.UserName;
                    d.PartitionKey = d.Id.ToString();
                    d.RowKey = string.Empty;
                });
        }
    }
}