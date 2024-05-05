using Microsoft.AspNetCore.Identity;
using ServiceBricks.Storage.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ServiceBricks.Security.EntityFrameworkCore
{
    /// <summary>
    /// This is an role.
    /// </summary>
    public partial class ApplicationRole : IdentityRole<Guid>, IEntityFrameworkCoreDomainObject<ApplicationRole>
    {
        public ApplicationRole()
        {
            ApplicationUserRoles = new List<ApplicationUserRole>();
        }

        public virtual List<ApplicationUserRole> ApplicationUserRoles { get; set; }

        public virtual IQueryable<ApplicationRole> DomainGetIQueryableDefaults(IQueryable<ApplicationRole> query)
        {
            return query;
        }

        public virtual Expression<Func<ApplicationRole, bool>> DomainGetItemFilter(ApplicationRole obj)
        {
            return x => x.Id == obj.Id;
        }
    }
}