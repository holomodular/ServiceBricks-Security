using Azure;
using Azure.Data.Tables;
using Microsoft.AspNetCore.Identity;
using ServiceBricks.Storage.AzureDataTables;

namespace ServiceBricks.Security.AzureDataTables
{
    /// <summary>
    /// This is a token for a user.
    /// </summary>
    public partial class ApplicationUserToken : IdentityUserToken<Guid>, IAzureDataTablesDomainObject<ApplicationUserToken>
    {
        public virtual string PartitionKey { get; set; }
        public virtual string RowKey { get; set; }
        public virtual DateTimeOffset? Timestamp { get; set; }
        public virtual ETag ETag { get; set; }
    }
}