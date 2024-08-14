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
        /// <summary>
        /// Internal primary key.
        /// </summary>
        public virtual Guid Key { get; set; }

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
            return x => x.Key == obj.Key;
        }
    }
}