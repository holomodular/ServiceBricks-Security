using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ServiceBricks.Storage.MongoDb;
using System.Linq.Expressions;

namespace ServiceBricks.Security.MongoDb
{
    /// <summary>
    /// This associates a user to a role.
    /// </summary>
    public partial class ApplicationUserRole : IdentityUserRole<string>, IMongoDbDomainObject<ApplicationUserRole>
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
        public virtual Expression<Func<ApplicationUserRole, bool>> DomainGetItemFilter(ApplicationUserRole obj)
        {
            return x => x.Key == obj.Key;
        }
    }
}