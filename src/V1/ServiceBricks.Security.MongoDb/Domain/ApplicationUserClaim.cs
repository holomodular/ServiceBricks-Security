using Microsoft.AspNetCore.Identity;
using ServiceBricks.Storage.MongoDb;
using System.Linq;
using System.Linq.Expressions;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ServiceBricks.Security.MongoDb
{
    /// <summary>
    /// This is a claim for a user.
    /// </summary>
    public partial class ApplicationUserClaim : IMongoDbDomainObject<ApplicationUserClaim>
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public virtual string Key { get; set; }

        //
        // Summary:
        //     Gets or sets the primary key of the user associated with this claim.
        public virtual string UserId { get; set; }

        //
        // Summary:
        //     Gets or sets the claim type for this claim.
        public virtual string ClaimType { get; set; }

        //
        // Summary:
        //     Gets or sets the claim value for this claim.
        public virtual string ClaimValue { get; set; }

        public virtual Expression<Func<ApplicationUserClaim, bool>> DomainGetItemFilter(ApplicationUserClaim obj)
        {
            return x => x.Key == obj.Key;
        }
    }
}