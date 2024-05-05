using Microsoft.AspNetCore.Identity;
using ServiceBricks.Storage.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;

namespace ServiceBricks.Security.Cosmos
{
    /// <summary>
    /// This associates a user to a role.
    /// </summary>
    public partial class ApplicationUserRole : IdentityUserRole<Guid>, IEntityFrameworkCoreDomainObject<ApplicationUserRole>
    {
        public virtual ApplicationUser User { get; set; }

        public virtual ApplicationRole Role { get; set; }

        public virtual IQueryable<ApplicationUserRole> DomainGetIQueryableDefaults(IQueryable<ApplicationUserRole> query)
        {
            return query;
        }

        public virtual Expression<Func<ApplicationUserRole, bool>> DomainGetItemFilter(ApplicationUserRole obj)
        {
            return x => x.UserId == obj.UserId && x.RoleId == obj.RoleId;
        }
    }
}