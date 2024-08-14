using Microsoft.AspNetCore.Identity;

namespace ServiceBricks.Security
{
    /// <summary>
    /// Result from login attempt
    /// </summary>
    public partial class ApplicationSigninResult
    {
        /// <summary>
        /// Sign in result
        /// </summary>
        public SignInResult SignInResult { get; set; }

        /// <summary>
        /// The user that was signed in
        /// </summary>
        public ApplicationUserDto User { get; set; }

        /// <summary>
        /// Determine if email is confirmed
        /// </summary>
        public bool EmailNotConfirmed { get; set; }
    }
}