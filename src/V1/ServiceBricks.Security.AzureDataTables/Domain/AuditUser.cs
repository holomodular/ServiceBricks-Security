using Azure;
using Azure.Data.Tables;

using ServiceBricks.Storage.AzureDataTables;

namespace ServiceBricks.Security.AzureDataTables
{
    /// <summary>
    /// This is user audit information.
    /// </summary>
    public class AuditUser : AzureDataTablesDomainObject<AuditUser>, IDpCreateDate
    {
        public Guid Key { get; set; }
        public DateTimeOffset CreateDate { get; set; }
        public Guid UserId { get; set; }
        public string IPAddress { get; set; }
        public string UserAgent { get; set; }
        public string AuditName { get; set; }
        public string Data { get; set; }
    }
}