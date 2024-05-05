using Microsoft.Extensions.Logging;
using ServiceBricks.Storage.EntityFrameworkCore;

namespace ServiceBricks.Security.Postgres
{
    /// <summary>
    /// This is a storage repository for the Security module.
    /// </summary>
    /// <typeparam name="TDomain"></typeparam>
    public class SecurityStorageRepository<TDomain> : EntityFrameworkCoreStorageRepository<TDomain>
        where TDomain : class, IEntityFrameworkCoreDomainObject<TDomain>, new()
    {
        public SecurityStorageRepository(ILoggerFactory logFactory, SecurityPostgresContext context)
            : base(logFactory)
        {
            Context = context;
            DbSet = context.Set<TDomain>();
        }
    }
}