using ServiceBricks.Storage.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ServiceBricks.Security.EntityFrameworkCore
{
    /// <summary>
    /// This is user audit information.
    /// </summary>
    public class AuditUser : EntityFrameworkCoreDomainObject<AuditUser>, IDpCreateDate
    {
        public long Key { get; set; }
        public DateTimeOffset CreateDate { get; set; }
        public Guid UserId { get; set; }
        public string IPAddress { get; set; }
        public string UserAgent { get; set; }
        public string AuditName { get; set; }
        public string Data { get; set; }

        public override IQueryable<AuditUser> DomainGetIQueryableDefaults(IQueryable<AuditUser> query)
        {
            return query.OrderByDescending(x => x.CreateDate);
        }

        public override Expression<Func<AuditUser, bool>> DomainGetItemFilter(AuditUser obj)
        {
            return x => x.Key == obj.Key;
        }
    }
}