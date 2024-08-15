using Microsoft.Extensions.Logging;
using ServiceBricks.Security.EntityFrameworkCore;

namespace ServiceBricks.Security.SqlServer
{
    /// <summary>
    /// This is a storage repository for the UserAudit domain object.
    /// </summary>
    public partial class UserAuditStorageRepository : SecurityStorageRepository<UserAudit>, IUserAuditStorageRepository
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="loggerFactory"></param>
        /// <param name="context"></param>
        public UserAuditStorageRepository(
            ILoggerFactory loggerFactory,
            SecuritySqlServerContext context) : base(loggerFactory, context)
        {
        }
    }
}