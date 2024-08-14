using Microsoft.AspNetCore.Identity;

namespace ServiceBricks.Security.MongoDb
{
    /// <summary>
    /// This is a claim for a user.
    /// </summary>
    public partial class ApplicationIdentityUserClaim : IdentityUserClaim<string>
    {
        /// <summary>
        /// Internal primary key.
        /// </summary>
        public virtual string Key { get; set; }
    }
}