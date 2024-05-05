using Microsoft.AspNetCore.Identity;
using ServiceBricks.Storage.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ServiceBricks.Security.Cosmos
{
    /// <summary>
    /// This associated a role to a claim.
    /// </summary>
    public partial class ApplicationRoleClaim : IdentityRoleClaim<Guid>, IEntityFrameworkCoreDomainObject<ApplicationRoleClaim>
    {
        public virtual Guid Key { get; set; }

        public virtual IQueryable<ApplicationRoleClaim> DomainGetIQueryableDefaults(IQueryable<ApplicationRoleClaim> query)
        {
            return query;
        }

        public virtual Expression<Func<ApplicationRoleClaim, bool>> DomainGetItemFilter(ApplicationRoleClaim obj)
        {
            return x => x.Key == obj.Key;
        }
    }
}