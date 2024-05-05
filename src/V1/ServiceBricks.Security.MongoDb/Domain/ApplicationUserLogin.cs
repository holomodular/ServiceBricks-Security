using Microsoft.AspNetCore.Identity;
using ServiceBricks.Storage.MongoDb;
using System.Linq;
using System.Linq.Expressions;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ServiceBricks.Security.MongoDb
{
    /// <summary>
    /// This is an external login for a user.
    /// </summary>
    public partial class ApplicationUserLogin : IdentityUserLogin<string>, IMongoDbDomainObject<ApplicationUserLogin>
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public virtual string Key { get; set; }

        public virtual Expression<Func<ApplicationUserLogin, bool>> DomainGetItemFilter(ApplicationUserLogin obj)
        {
            return x => x.Key == obj.Key;
        }
    }
}