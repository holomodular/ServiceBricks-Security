using Azure;
using Azure.Data.Tables;
using Microsoft.AspNetCore.Identity;
using ServiceBricks.Storage.AzureDataTables;

namespace ServiceBricks.Security.AzureDataTables
{
    /// <summary>
    /// This associated a role to a claim.
    /// </summary>
    public partial class ApplicationRoleClaim : IdentityRoleClaim<Guid>, IAzureDataTablesDomainObject<ApplicationRoleClaim>
    {
        public virtual string PartitionKey { get; set; }
        public virtual string RowKey { get; set; }
        public virtual DateTimeOffset? Timestamp { get; set; }
        public virtual ETag ETag { get; set; }

        public virtual Guid Key { get; set; }
    }
}