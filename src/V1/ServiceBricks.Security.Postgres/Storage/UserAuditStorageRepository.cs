using Microsoft.Extensions.Logging;
using ServiceBricks.Security.EntityFrameworkCore;

namespace ServiceBricks.Security.Postgres
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
            SecurityPostgresContext context) : base(loggerFactory, context)
        {
        }
    }
}