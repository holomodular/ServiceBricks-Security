using Microsoft.AspNetCore.Identity;
using ServiceBricks.Storage.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ServiceBricks.Security.EntityFrameworkCore
{
    /// <summary>
    /// This is a claim for a user.
    /// </summary>
    public partial class ApplicationUserClaim : IdentityUserClaim<Guid>, IEntityFrameworkCoreDomainObject<ApplicationUserClaim>
    {
        public virtual IQueryable<ApplicationUserClaim> DomainGetIQueryableDefaults(IQueryable<ApplicationUserClaim> query)
        {
            return query;
        }

        public virtual Expression<Func<ApplicationUserClaim, bool>> DomainGetItemFilter(ApplicationUserClaim obj)
        {
            return x => x.Id == obj.Id;
        }
    }
}