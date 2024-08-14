using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ServiceBricks.Storage.MongoDb;
using System.Linq.Expressions;

namespace ServiceBricks.Security.MongoDb
{
    /// <summary>
    /// This associated a role to a claim.
    /// </summary>
    public partial class ApplicationRoleClaim : IMongoDbDomainObject<ApplicationRoleClaim>
    {
        /// <summary>
        /// Internal primary key.
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public virtual string Key { get; set; }

        //
        // Summary:
        //     Gets or sets the of the primary key of the role associated with this claim.
        public virtual string RoleId { get; set; }

        //
        // Summary:
        //     Gets or sets the claim type for this claim.
        public virtual string ClaimType { get; set; }

        //
        // Summary:
        //     Gets or sets the claim value for this claim.
        public virtual string ClaimValue { get; set; }

        /// <summary>
        /// Provide an expression that will filter an object based on its primary key.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual Expression<Func<ApplicationRoleClaim, bool>> DomainGetItemFilter(ApplicationRoleClaim obj)
        {
            return x => x.Key == obj.Key;
        }
    }
}