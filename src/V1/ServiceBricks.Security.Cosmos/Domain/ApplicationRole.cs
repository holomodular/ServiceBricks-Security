using Microsoft.AspNetCore.Identity;
using ServiceBricks.Storage.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ServiceBricks.Security.Cosmos
{
    /// <summary>
    /// This is an role.
    /// </summary>
    public partial class ApplicationRole : IdentityRole<Guid>, IEntityFrameworkCoreDomainObject<ApplicationRole>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public ApplicationRole()
        {
            ApplicationUserRoles = new List<ApplicationUserRole>();
        }

        /// <summary>
        /// The list of user roles.
        /// </summary>
        public virtual List<ApplicationUserRole> ApplicationUserRoles { get; set; }

        /// <summary>
        /// Provide any defaults for the IQueryable object.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public virtual IQueryable<ApplicationRole> DomainGetIQueryableDefaults(IQueryable<ApplicationRole> query)
        {
            return query;
        }

        /// <summary>
        /// Provide an expression that will filter an object based on its primary key.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual Expression<Func<ApplicationRole, bool>> DomainGetItemFilter(ApplicationRole obj)
        {
            return x => x.Id == obj.Id;
        }
    }
}