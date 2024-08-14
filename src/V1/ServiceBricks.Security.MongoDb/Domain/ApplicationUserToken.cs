using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ServiceBricks.Storage.MongoDb;
using System.Linq.Expressions;

namespace ServiceBricks.Security.MongoDb
{
    /// <summary>
    /// This is a token for a user.
    /// </summary>
    public partial class ApplicationUserToken : IdentityUserToken<string>, IMongoDbDomainObject<ApplicationUserToken>
    {
        /// <summary>
        /// Internal primary key.
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public virtual string Key { get; set; }

        /// <summary>
        /// Provide an expression that will filter an object based on its primary key.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual Expression<Func<ApplicationUserToken, bool>> DomainGetItemFilter(ApplicationUserToken obj)
        {
            return x => x.Key == obj.Key;
        }
    }
}