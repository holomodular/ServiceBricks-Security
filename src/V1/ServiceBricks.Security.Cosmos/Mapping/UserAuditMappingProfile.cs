namespace ServiceBricks.Security.Cosmos
{
    /// <summary>
    /// This is a mapper profile for the AuditUser domain object.
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
                    d.UserStorageKey = s.UserId.ToString();
                    d.StorageKey = s.Key.ToString();
                });

            registry.Register<UserAuditDto, UserAudit>(
                (s, d) =>
                {
                    d.AuditType = s.AuditType;
                    //d.CreateDate ignore
                    d.Data = s.Data;
                    d.IPAddress = s.IPAddress;
                    d.RequestHeaders = s.RequestHeaders;                                                            
                    if (Guid.TryParse(s.UserStorageKey, out var tempUserId))
                        d.UserId = tempUserId;
                    if (Guid.TryParse(s.StorageKey, out var tempStorageKey))
                        d.Key = tempStorageKey;
                });
        }
    }
}