using Microsoft.AspNetCore.Identity;
using ServiceBricks.Storage.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ServiceBricks.Security.EntityFrameworkCore
{
    /// <summary>
    /// This associated a role to a claim.
    /// </summary>
    public partial class ApplicationRoleClaim : IdentityRoleClaim<Guid>, IEntityFrameworkCoreDomainObject<ApplicationRoleClaim>
    {
        /// <summary>
        /// Provide any defaults for the IQueryable object.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public virtual IQueryable<ApplicationRoleClaim> DomainGetIQueryableDefaults(IQueryable<ApplicationRoleClaim> query)
        {
            return query;
        }

        /// <summary>
        /// Provide an expression that will filter an object based on its primary key.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual Expression<Func<ApplicationRoleClaim, bool>> DomainGetItemFilter(ApplicationRoleClaim obj)
        {
            return x => x.Id == obj.Id;
        }
    }
}