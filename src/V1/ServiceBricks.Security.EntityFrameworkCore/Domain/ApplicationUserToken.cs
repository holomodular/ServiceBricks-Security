using Microsoft.AspNetCore.Identity;
using ServiceBricks.Storage.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ServiceBricks.Security.EntityFrameworkCore
{
    /// <summary>
    /// This is a token for a user.
    /// </summary>
    public partial class ApplicationUserToken : IdentityUserToken<Guid>, IEntityFrameworkCoreDomainObject<ApplicationUserToken>
    {
        /// <summary>
        /// Provide any defaults for the IQueryable object.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public virtual IQueryable<ApplicationUserToken> DomainGetIQueryableDefaults(IQueryable<ApplicationUserToken> query)
        {
            return query;
        }

        /// <summary>
        /// Provide an expression that will filter an object based on its primary key.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual Expression<Func<ApplicationUserToken, bool>> DomainGetItemFilter(ApplicationUserToken obj)
        {
            return x => x.UserId == obj.UserId && x.LoginProvider == obj.LoginProvider && x.Name == obj.Name;
        }
    }
}