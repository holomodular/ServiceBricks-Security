using Microsoft.AspNetCore.Identity;

namespace ServiceBricks.Security
{
    /// <summary>
    /// The module definition for the Security API Brick.
    /// </summary>
    public partial class SecurityModule : ServiceBricks.Module
    {
        /// <summary>
        /// Instance
        /// </summary>
        public static SecurityModule Instance = new SecurityModule();

        /// <summary>
        /// Constructor.
        /// </summary>
        public SecurityModule()
        {
            IdentityOptions = new Action<IdentityOptions>(options => new IdentityOptions());
        }

        /// <summary>
        /// Identity Options when starting
        /// </summary>
        public virtual Action<IdentityOptions> IdentityOptions { get; set; }
    }
}