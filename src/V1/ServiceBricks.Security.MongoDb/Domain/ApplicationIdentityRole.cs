using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Linq.Expressions;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ServiceBricks.Security.MongoDb
{
    /// <summary>
    /// This is an role.
    /// </summary>
    public partial class ApplicationIdentityRole : IdentityRole<string>
    {
    }
}