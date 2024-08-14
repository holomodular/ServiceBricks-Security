using Microsoft.AspNetCore.Identity;
using ServiceBricks.Storage.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ServiceBricks.Security.Cosmos
{
    /// <summary>
    /// This is a user in the application.
    /// </summary>
    public partial class ApplicationUser : IdentityUser<Guid>, IEntityFrameworkCoreDomainObject<ApplicationUser>, IDpCreateDate, IDpUpdateDate
    {
        public ApplicationUser()
        {
            ApplicationUserRoles = new List<ApplicationUserRole>();
        }

        /// <summary>
        /// The create date.
        /// </summary>
        public virtual DateTimeOffset CreateDate { get; set; }

        /// <summary>
        /// The update date.
        /// </summary>
        public virtual DateTimeOffset UpdateDate { get; set; }

        /// <summary>
        /// The list of user roles.
        /// </summary>
        public virtual List<ApplicationUserRole> ApplicationUserRoles { get; set; }

        /// <summary>
        /// Provide any defaults for the IQueryable object.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public virtual IQueryable<ApplicationUser> DomainGetIQueryableDefaults(IQueryable<ApplicationUser> query)
        {
            return query;
        }

        /// <summary>
        /// Provide an expression that will filter an object based on its primary key.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual Expression<Func<ApplicationUser, bool>> DomainGetItemFilter(ApplicationUser obj)
        {
            return x => x.Id == obj.Id;
        }
    }
}