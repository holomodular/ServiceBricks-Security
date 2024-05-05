using Microsoft.AspNetCore.Identity;
using ServiceBricks.Storage.MongoDb;
using System.Linq;
using System.Linq.Expressions;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ServiceBricks.Security.MongoDb
{
    /// <summary>
    /// This is a token for a user.
    /// </summary>
    public partial class ApplicationUserToken : IdentityUserToken<string>, IMongoDbDomainObject<ApplicationUserToken>
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public virtual string Key { get; set; }

        public virtual Expression<Func<ApplicationUserToken, bool>> DomainGetItemFilter(ApplicationUserToken obj)
        {
            return x => x.Key == obj.Key;
        }
    }
}