namespace ServiceBricks.Security.EntityFrameworkCore
{
    /// <summary>
    /// This is an automapper profile for the UserSecurity domain object.
    /// </summary>
    public partial class UserAuditMappingProfile
    {
        /// <summary>
        /// Register the mapping
        /// </summary>
        public static void Register(IMapperRegistry registry)
        {
            registry.Register<UserAudit, UserAuditDto>(
                (s, d) =>
                {
                    d.AuditType = s.AuditType;
                    d.CreateDate = s.CreateDate;
                    d.Data = s.Data;
                    d.IPAddress = s.IPAddress;
                    d.RequestHeaders = s.RequestHeaders;
                    d.StorageKey = s.Key.ToString();
                    d.UserStorageKey = s.UserId.ToString();
                });

            registry.Register<UserAuditDto, UserAudit>(
                (s, d) =>
                {
                    d.AuditType = s.AuditType;
                    //d.CreateDate ignore
                    d.Data = s.Data;
                    d.IPAddress = s.IPAddress;
                    d.RequestHeaders = s.RequestHeaders;
                    long tempStorageKey;
                    if (long.TryParse(s.StorageKey, out tempStorageKey))
                        d.Key = tempStorageKey;
                    Guid tempUserId;
                    if (Guid.TryParse(s.UserStorageKey, out tempUserId))
                        d.UserId = tempUserId;
                });
        }
    }
}