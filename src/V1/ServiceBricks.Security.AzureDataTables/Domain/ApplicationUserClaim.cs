using Azure;
using Azure.Data.Tables;
using Microsoft.AspNetCore.Identity;
using ServiceBricks.Storage.AzureDataTables;

namespace ServiceBricks.Security.AzureDataTables
{
    /// <summary>
    /// This is a claim for a user.
    /// </summary>
    public partial class ApplicationUserClaim : IdentityUserClaim<Guid>, IAzureDataTablesDomainObject<ApplicationUserClaim>
    {
        public virtual Guid Key { get; set; }
        public virtual string PartitionKey { get; set; }
        public virtual string RowKey { get; set; }
        public virtual DateTimeOffset? Timestamp { get; set; }
        public virtual ETag ETag { get; set; }
    }
}