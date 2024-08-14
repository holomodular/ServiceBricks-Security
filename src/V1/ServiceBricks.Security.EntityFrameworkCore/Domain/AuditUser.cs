using ServiceBricks.Storage.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ServiceBricks.Security.EntityFrameworkCore
{
    /// <summary>
    /// This is user audit information.
    /// </summary>
    public partial class AuditUser : EntityFrameworkCoreDomainObject<AuditUser>, IDpCreateDate
    {
        /// <summary>
        /// Internal key.
        /// </summary>
        public long Key { get; set; }

        /// <summary>
        /// The create date.
        /// </summary>
        public DateTimeOffset CreateDate { get; set; }

        /// <summary>
        /// The user ID.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// The IP address.
        /// </summary>
        public string IPAddress { get; set; }

        /// <summary>
        /// The user agent.
        /// </summary>
        public string UserAgent { get; set; }

        /// <summary>
        /// The audit name.
        /// </summary>
        public string AuditName { get; set; }

        /// <summary>
        /// The audit data.
        /// </summary>
        public string Data { get; set; }

        /// <summary>
        /// Provide any defaults for the IQueryable object.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public override IQueryable<AuditUser> DomainGetIQueryableDefaults(IQueryable<AuditUser> query)
        {
            return query.OrderByDescending(x => x.CreateDate);
        }

        /// <summary>
        /// Provide an expression that will filter an object based on its primary key.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override Expression<Func<AuditUser, bool>> DomainGetItemFilter(AuditUser obj)
        {
            return x => x.Key == obj.Key;
        }
    }
}