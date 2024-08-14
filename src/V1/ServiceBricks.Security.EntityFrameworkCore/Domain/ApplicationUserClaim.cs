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
        /// <summary>
        /// Provide any defaults for the IQueryable object.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public virtual IQueryable<ApplicationUserClaim> DomainGetIQueryableDefaults(IQueryable<ApplicationUserClaim> query)
        {
            return query;
        }

        /// <summary>
        /// Provide an expression that will filter an object based on its primary key.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual Expression<Func<ApplicationUserClaim, bool>> DomainGetItemFilter(ApplicationUserClaim obj)
        {
            return x => x.Id == obj.Id;
        }
    }
}