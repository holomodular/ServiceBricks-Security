using ServiceBricks.Storage.AzureDataTables;

namespace ServiceBricks.Security.AzureDataTables
{
    /// <summary>
    /// This is user audit information.
    /// </summary>
    public partial class AuditUser : AzureDataTablesDomainObject<AuditUser>, IDpCreateDate
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
        /// The user agent.
        /// </summary>
        public string UserAgent { get; set; }

        /// <summary>
        /// The audit name
        /// </summary>
        public string AuditName { get; set; }

        /// <summary>
        /// The audit data.
        /// </summary>
        public string Data { get; set; }
    }
}