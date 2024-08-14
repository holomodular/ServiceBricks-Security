using Microsoft.Extensions.Logging;

namespace ServiceBricks.Security.Cosmos
{
    /// <summary>
    /// This is a storage repository for the AuditUser domain object.
    /// </summary>
    public partial class AuditUserStorageRepository : SecurityStorageRepository<AuditUser>, IAuditUserStorageRepository
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="loggerFactory"></param>
        /// <param name="context"></param>
        public AuditUserStorageRepository(
            ILoggerFactory loggerFactory,
            SecurityCosmosContext context) : base(loggerFactory, context)
        {
        }
    }
}