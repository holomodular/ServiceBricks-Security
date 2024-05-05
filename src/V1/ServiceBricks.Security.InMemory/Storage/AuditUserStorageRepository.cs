using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServiceBricks.Security.EntityFrameworkCore;

namespace ServiceBricks.Security.InMemory
{
    /// <summary>
    /// This is a storage repository for the AuditUser domain object.
    /// </summary>
    public class AuditUserStorageRepository : SecurityStorageRepository<AuditUser>, IAuditUserStorageRepository
    {
        public AuditUserStorageRepository(
            ILoggerFactory loggerFactory,
            SecurityInMemoryContext context) : base(loggerFactory, context)
        {
        }
    }
}