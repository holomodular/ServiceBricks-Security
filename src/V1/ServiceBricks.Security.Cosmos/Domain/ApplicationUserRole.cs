using Microsoft.AspNetCore.Identity;
using ServiceBricks.Storage.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ServiceBricks.Security.Cosmos
{
    /// <summary>
    /// This associates a user to a role.
    /// </summary>
    public partial class ApplicationUserRole : IdentityUserRole<Guid>, IEntityFrameworkCoreDomainObject<ApplicationUserRole>
    {
        /// <summary>
        /// The user.
        /// </summary>
        public virtual ApplicationUser User { get; set; }

        /// <summary>
        /// The role.
        /// </summary>
        public virtual ApplicationRole Role { get; set; }

        /// <summary>
        /// Provide any defaults for the IQueryable object.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public virtual IQueryable<ApplicationUserRole> DomainGetIQueryableDefaults(IQueryable<ApplicationUserRole> query)
        {
            return query;
        }

        /// <summary>
        /// Provide an expression that will filter an object based on its primary key.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual Expression<Func<ApplicationUserRole, bool>> DomainGetItemFilter(ApplicationUserRole obj)
        {
            return x => x.UserId == obj.UserId && x.RoleId == obj.RoleId;
        }
    }
}