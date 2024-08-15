using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ServiceBricks.Security.MongoDb
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
        /// <param name="configuration"></param>
        public UserAuditStorageRepository(
            ILoggerFactory loggerFactory,
            IConfiguration configuration)
            : base(loggerFactory, configuration)
        {
        }
    }
}