using Microsoft.AspNetCore.Identity;

namespace ServiceBricks.Security.MongoDb
{
    /// <summary>
    /// This is a user in the application.
    /// </summary>
    public partial class ApplicationIdentityUser : IdentityUser<string>, IDpCreateDate, IDpUpdateDate
    {
        /// <summary>
        /// The create date of the user.
        /// </summary>
        public virtual DateTimeOffset CreateDate { get; set; }

        /// <summary>
        /// The update date of the user.
        /// </summary>
        public virtual DateTimeOffset UpdateDate { get; set; }
    }
}