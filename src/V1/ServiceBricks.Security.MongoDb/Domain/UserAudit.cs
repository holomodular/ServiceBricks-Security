using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ServiceBricks.Storage.MongoDb;
using System.Linq.Expressions;

namespace ServiceBricks.Security.MongoDb
{
    /// <summary>
    /// This is user audit information.
    /// </summary>
    public partial class UserAudit : MongoDbDomainObject<UserAudit>, IDpCreateDate
    {
        /// <summary>
        /// Internal primary key.
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public virtual string Key { get; set; }

        /// <summary>
        /// The create date of the audit.
        /// </summary>
        public DateTimeOffset CreateDate { get; set; }

        /// <summary>
        /// The user id of the user that was audited.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// The IP address of the user that was audited.
        /// </summary>
        public string IPAddress { get; set; }

        /// <summary>
        /// THe user agent of the user that was audited.
        /// </summary>
        public string RequestHeaders { get; set; }

        /// <summary>
        /// The name of the audit.
        /// </summary>
        public string AuditType { get; set; }

        /// <summary>
        /// The data of the audit.
        /// </summary>
        public string Data { get; set; }

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