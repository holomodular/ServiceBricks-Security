using ServiceBricks.Storage.AzureDataTables;

namespace ServiceBricks.Security.AzureDataTables
{
    /// <summary>
    /// This is user audit information.
    /// </summary>
    public partial class UserAudit : AzureDataTablesDomainObject<UserAudit>, IDpCreateDate
    {
        /// <summary>
        /// Internal primary key.
        /// </summary>
        public Guid Key { get; set; }

        /// <summary>
        /// The create date.
        /// </summary>
        public DateTimeOffset CreateDate { get; set; }

        /// <summary>
        /// The user id.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// The IP address.
        /// </summary>
        public string IPAddress { get; set; }

        /// <summary>
        /// The request headers.
        /// </summary>
        public string RequestHeaders { get; set; }

        /// <summary>
        /// The audit name
        /// </summary>
        public string AuditType { get; set; }

        /// <summary>
        /// The audit data.
        /// </summary>
        public string Data { get; set; }
    }
}