using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ServiceBricks.Security.MongoDb
{
    /// <summary>
    /// This is a storage repository for the AuditUser domain object.
    /// </summary>
    public class AuditUserStorageRepository : SecurityStorageRepository<AuditUser>, IAuditUserStorageRepository
    {
        public AuditUserStorageRepository(
            ILoggerFactory loggerFactory,
            IConfiguration configuration)
            : base(loggerFactory, configuration)
        {
        }
    }
}