using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ServiceBricks.Storage.MongoDb;
using System.Linq.Expressions;

namespace ServiceBricks.Security.MongoDb
{
    /// <summary>
    /// This is an external login for a user.
    /// </summary>
    public partial class ApplicationUserLogin : IdentityUserLogin<string>, IMongoDbDomainObject<ApplicationUserLogin>
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
        public virtual Expression<Func<ApplicationUserLogin, bool>> DomainGetItemFilter(ApplicationUserLogin obj)
        {
            return x => x.Key == obj.Key;
        }
    }
}