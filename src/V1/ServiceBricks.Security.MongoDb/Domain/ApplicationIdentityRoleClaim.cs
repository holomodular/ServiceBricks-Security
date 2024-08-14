using Microsoft.AspNetCore.Identity;

namespace ServiceBricks.Security.MongoDb
{
    /// <summary>
    /// This associated a role to a claim.
    /// </summary>
    public partial class ApplicationIdentityRoleClaim : IdentityRoleClaim<string>
    {
        /// <summary>
        /// Internal primary key.
        /// </summary>
        public virtual string Key { get; set; }
    }
}