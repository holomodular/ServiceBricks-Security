using ServiceBricks.Storage.MongoDb;
using System.Linq;
using System.Linq.Expressions;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ServiceBricks.Security.MongoDb
{
    /// <summary>
    /// This is user audit information.
    /// </summary>
    public class AuditUser : MongoDbDomainObject<AuditUser>, IDpCreateDate
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public virtual string Key { get; set; }

        public DateTimeOffset CreateDate { get; set; }
        public string UserId { get; set; }
        public string IPAddress { get; set; }
        public string UserAgent { get; set; }
        public string AuditName { get; set; }
        public string Data { get; set; }

        public override Expression<Func<AuditUser, bool>> DomainGetItemFilter(AuditUser obj)
        {
            return x => x.Key == obj.Key;
        }
    }
}