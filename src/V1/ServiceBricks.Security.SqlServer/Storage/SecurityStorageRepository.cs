using Microsoft.Extensions.Logging;
using ServiceBricks.Storage.EntityFrameworkCore;

namespace ServiceBricks.Security.SqlServer
{
    /// <summary>
    /// This is a storage repository for the Security module.
    /// </summary>
    /// <typeparam name="TDomain"></typeparam>
    public class SecurityStorageRepository<TDomain> : EntityFrameworkCoreStorageRepository<TDomain>
        where TDomain : class, IEntityFrameworkCoreDomainObject<TDomain>, new()
    {
        public SecurityStorageRepository(ILoggerFactory logFactory, SecuritySqlServerContext context)
            : base(logFactory)
        {
            Context = context;
            DbSet = context.Set<TDomain>();
        }
    }
}