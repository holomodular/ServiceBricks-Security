using Azure;
using Microsoft.AspNetCore.Identity;
using ServiceBricks.Storage.AzureDataTables;

namespace ServiceBricks.Security.AzureDataTables
{
    /// <summary>
    /// This is a claim for a user.
    /// </summary>
    public partial class ApplicationUserClaim : IdentityUserClaim<Guid>, IAzureDataTablesDomainObject<ApplicationUserClaim>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ApplicationUserClaim()
        {
            PartitionKey = string.Empty;
            RowKey = string.Empty;
        }

        /// <summary>
        /// Internal primary key.
        /// </summary>
        public virtual Guid Key { get; set; }

        /// <summary>
        ///     The partition key is a unique identifier for the partition within a given table
        ///     and forms the first part of an entity's primary key.
        /// </summary>
        public virtual string PartitionKey { get; set; }

        /// <summary>
        ///     The row key is a unique identifier for an entity within a given partition. Together
        ///     the Azure.Data.Tables.ITableEntity.PartitionKey and RowKey uniquely identify
        ///     every entity within a table.
        /// </summary>
        public virtual string RowKey { get; set; }

        /// <summary>
        ///     The Timestamp property is a DateTime value that is maintained on the server side
        ///     to record the time an entity was last modified. The Table service uses the Timestamp
        ///     property internally to provide optimistic concurrency. The value of Timestamp
        ///     is a monotonically increasing value, meaning that each time the entity is modified,
        ///     the value of Timestamp increases for that entity. This property should not be
        ///     set on insert or update operations (the value will be ignored).
        /// </summary>
        public virtual DateTimeOffset? Timestamp { get; set; }

        /// <summary>
        /// Gets or sets the entity's ETag.
        /// </summary>
        public virtual ETag ETag { get; set; }
    }
}