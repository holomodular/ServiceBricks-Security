using ServiceBricks.Storage.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ServiceBricks.Security.EntityFrameworkCore
{
    /// <summary>
    /// This is user audit information.
    /// </summary>
    public partial class UserAudit : EntityFrameworkCoreDomainObject<UserAudit>, IDpCreateDate
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
        /// The request headers.
        /// </summary>
        public string RequestHeaders { get; set; }

        /// <summary>
        /// The audit name.
        /// </summary>
        public string AuditType { get; set; }

        /// <summary>
        /// The audit data.
        /// </summary>
        public string Data { get; set; }

        /// <summary>
        /// Provide any defaults for the IQueryable object.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public override IQueryable<UserAudit> DomainGetIQueryableDefaults(IQueryable<UserAudit> query)
        {
            return query.OrderByDescending(x => x.CreateDate);
        }

        /// <summary>
        /// Provide an expression that will filter an object based on its primary key.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override Expression<Func<UserAudit, bool>> DomainGetItemFilter(UserAudit obj)
        {
            return x => x.Key == obj.Key;
        }
    }
}