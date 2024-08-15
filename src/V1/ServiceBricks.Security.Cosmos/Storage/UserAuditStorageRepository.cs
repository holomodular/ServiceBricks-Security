using Microsoft.Extensions.Logging;

namespace ServiceBricks.Security.Cosmos
{
    /// <summary>
    /// This is a storage repository for the AuditUser domain object.
    /// </summary>
    public partial class UserAuditStorageRepository : SecurityStorageRepository<UserAudit>, IUserAuditStorageRepository
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="loggerFactory"></param>
        /// <param name="context"></param>
        public UserAuditStorageRepository(
            ILoggerFactory loggerFactory,
            SecurityCosmosContext context) : base(loggerFactory, context)
        {
        }
    }
}