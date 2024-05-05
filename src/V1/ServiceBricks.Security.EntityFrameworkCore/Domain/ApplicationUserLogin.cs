using Microsoft.AspNetCore.Identity;
using ServiceBricks.Storage.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ServiceBricks.Security.EntityFrameworkCore
{
    /// <summary>
    /// This is an external login for a user.
    /// </summary>
    public partial class ApplicationUserLogin : IdentityUserLogin<Guid>, IEntityFrameworkCoreDomainObject<ApplicationUserLogin>
    {
        public virtual IQueryable<ApplicationUserLogin> DomainGetIQueryableDefaults(IQueryable<ApplicationUserLogin> query)
        {
            return query;
        }

        public virtual Expression<Func<ApplicationUserLogin, bool>> DomainGetItemFilter(ApplicationUserLogin obj)
        {
            return x => x.LoginProvider == obj.LoginProvider && x.ProviderKey == obj.ProviderKey;
        }
    }
}