using Azure;
using Azure.Data.Tables;
using Microsoft.AspNetCore.Identity;
using ServiceBricks.Storage.AzureDataTables;

namespace ServiceBricks.Security.AzureDataTables
{
    /// <summary>
    /// This is an role.
    /// </summary>
    public partial class ApplicationRole : IdentityRole<Guid>, IAzureDataTablesDomainObject<ApplicationRole>
    {
        public ApplicationRole()
        {
            RowKey = string.Empty;
        }

        public virtual string PartitionKey { get; set; }
        public virtual string RowKey { get; set; }
        public virtual DateTimeOffset? Timestamp { get; set; }
        public virtual ETag ETag { get; set; }
    }
}