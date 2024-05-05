using Microsoft.AspNetCore.Identity;
using ServiceBricks.Storage.MongoDb;
using System.Linq;
using System.Linq.Expressions;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ServiceBricks.Security.MongoDb
{
    /// <summary>
    /// This associated a role to a claim.
    /// </summary>
    public partial class ApplicationIdentityRoleClaim : IdentityRoleClaim<string>
    {
        public virtual string Key { get; set; }
    }
}