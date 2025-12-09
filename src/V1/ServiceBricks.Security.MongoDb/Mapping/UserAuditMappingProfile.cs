namespace ServiceBricks.Security.MongoDb
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
                    d.StorageKey = s.Key;
                    d.UserStorageKey = s.UserId;
                });

            registry.Register<UserAuditDto, UserAudit>(
                (s, d) =>
                {
                    d.AuditType = s.AuditType;
                    //d.CreateDate ignore
                    d.Data = s.Data;
                    d.IPAddress = s.IPAddress;
                    d.RequestHeaders = s.RequestHeaders;
                    d.Key = s.StorageKey;
                    d.UserId = s.UserStorageKey;
                });
        }
    }
}