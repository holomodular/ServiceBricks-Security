using Azure;
using Azure.Data.Tables;
using Microsoft.AspNetCore.Identity;
using ServiceBricks.Storage.AzureDataTables;

namespace ServiceBricks.Security.AzureDataTables
{
    /// <summary>
    /// This is a user in the application.
    /// </summary>
    public partial class ApplicationUser : IdentityUser<Guid>, IAzureDataTablesDomainObject<ApplicationUser>, IDpCreateDate, IDpUpdateDate
    {
        public ApplicationUser()
        {
            RowKey = string.Empty;
        }

        public virtual string PartitionKey { get; set; }
        public virtual string RowKey { get; set; }
        public virtual DateTimeOffset? Timestamp { get; set; }
        public virtual ETag ETag { get; set; }

        public virtual DateTimeOffset CreateDate { get; set; }
        public virtual DateTimeOffset UpdateDate { get; set; }
    }
}