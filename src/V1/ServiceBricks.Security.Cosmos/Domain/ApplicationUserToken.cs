using Microsoft.AspNetCore.Identity;
using ServiceBricks.Storage.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ServiceBricks.Security.Cosmos
{
    /// <summary>
    /// This is a token for a user.
    /// </summary>
    public partial class ApplicationUserToken : IdentityUserToken<Guid>, IEntityFrameworkCoreDomainObject<ApplicationUserToken>
    {
        public virtual Guid Key { get; set; }

        public virtual IQueryable<ApplicationUserToken> DomainGetIQueryableDefaults(IQueryable<ApplicationUserToken> query)
        {
            return query;
        }

        public virtual Expression<Func<ApplicationUserToken, bool>> DomainGetItemFilter(ApplicationUserToken obj)
        {
            return x => x.Key == obj.Key;
        }
    }
}