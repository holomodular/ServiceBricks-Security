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
    public partial class ApplicationIdentityUserClaim : IdentityUserClaim<string>
    {
        public virtual string Key { get; set; }
    }
}