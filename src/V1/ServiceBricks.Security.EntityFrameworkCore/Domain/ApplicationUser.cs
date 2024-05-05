using Microsoft.AspNetCore.Identity;
using ServiceBricks.Storage.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ServiceBricks.Security.EntityFrameworkCore
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

        public virtual DateTimeOffset CreateDate { get; set; }
        public virtual DateTimeOffset UpdateDate { get; set; }
        public List<ApplicationUserRole> ApplicationUserRoles { get; set; }

        public virtual IQueryable<ApplicationUser> DomainGetIQueryableDefaults(IQueryable<ApplicationUser> query)
        {
            return query;
        }

        public virtual Expression<Func<ApplicationUser, bool>> DomainGetItemFilter(ApplicationUser obj)
        {
            return x => x.Id == obj.Id;
        }
    }
}