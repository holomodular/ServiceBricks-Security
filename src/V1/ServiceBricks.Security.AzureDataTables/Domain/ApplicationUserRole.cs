using Azure;
using Azure.Data.Tables;
using Microsoft.AspNetCore.Identity;
using ServiceBricks.Storage.AzureDataTables;

namespace ServiceBricks.Security.AzureDataTables
{
    /// <summary>
    /// This associates a user to a role.
    /// </summary>
    public partial class ApplicationUserRole : IdentityUserRole<Guid>, IAzureDataTablesDomainObject<ApplicationUserRole>
    {
        //public ApplicationUser User { get; set; }

        //public ApplicationRole Role { get; set; }

        public virtual string PartitionKey { get; set; }
        public virtual string RowKey { get; set; }
        public virtual DateTimeOffset? Timestamp { get; set; }
        public virtual ETag ETag { get; set; }
    }
}