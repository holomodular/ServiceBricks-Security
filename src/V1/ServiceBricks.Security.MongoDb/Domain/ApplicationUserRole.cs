using Microsoft.AspNetCore.Identity;
using ServiceBricks.Storage.MongoDb;
using System.Linq;
using System.Linq.Expressions;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ServiceBricks.Security.MongoDb
{
    /// <summary>
    /// This associates a user to a role.
    /// </summary>
    public partial class ApplicationUserRole : IdentityUserRole<string>, IMongoDbDomainObject<ApplicationUserRole>
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public virtual string Key { get; set; }

        public virtual Expression<Func<ApplicationUserRole, bool>> DomainGetItemFilter(ApplicationUserRole obj)
        {
            return x => x.Key == obj.Key;
        }
    }
}