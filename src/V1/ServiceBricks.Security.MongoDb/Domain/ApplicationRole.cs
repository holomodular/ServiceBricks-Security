using Microsoft.AspNetCore.Identity;
using ServiceBricks.Storage.MongoDb;
using System.Linq;
using System.Linq.Expressions;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ServiceBricks.Security.MongoDb
{
    /// <summary>
    /// This is an role.
    /// </summary>
    public partial class ApplicationRole : MongoDbDomainObject<ApplicationRole>
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public virtual string Id { get; set; }

        //
        // Summary:
        //     Gets or sets the name for this role.
        public virtual string Name { get; set; }

        //
        // Summary:
        //     Gets or sets the normalized name for this role.
        public virtual string NormalizedName { get; set; }

        //
        // Summary:
        //     A random value that should change whenever a role is persisted to the store
        public virtual string ConcurrencyStamp { get; set; }

        public override Expression<Func<ApplicationRole, bool>> DomainGetItemFilter(ApplicationRole obj)
        {
            return x => x.Id == obj.Id;
        }
    }
}