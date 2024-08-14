using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ServiceBricks.Storage.MongoDb;
using System.Linq.Expressions;

namespace ServiceBricks.Security.MongoDb
{
    /// <summary>
    /// This is an role.
    /// </summary>
    public partial class ApplicationRole : MongoDbDomainObject<ApplicationRole>
    {
        /// <summary>
        /// Internal primary key.
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public virtual string Id { get; set; }

        /// <summary>
        /// THe name of the role.
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// The normalized name of the role.
        /// </summary>
        public virtual string NormalizedName { get; set; }

        /// <summary>
        /// A random value that should change whenever a role is persisted to the store
        /// </summary>
        public virtual string ConcurrencyStamp { get; set; }

        /// <summary>
        /// Provide an expression that will filter an object based on its primary key.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override Expression<Func<ApplicationRole, bool>> DomainGetItemFilter(ApplicationRole obj)
        {
            return x => x.Id == obj.Id;
        }
    }
}