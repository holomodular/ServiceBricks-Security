using Microsoft.AspNetCore.Identity;

namespace ServiceBricks.Security
{
    /// <summary>
    /// This is a process when a user verifies an MFA code.
    /// </summary>
    public partial class UserMfaVerifyProcess : DomainProcess
    {
        public UserMfaVerifyProcess(
            string provider,
            string code,
            bool rememberMe,
            bool rememberBrowser)
        {
            Provider = provider;
            Code = code;
            RememberMe = rememberMe;
            RememberBrowser = rememberBrowser;
        }

        public virtual string Provider { get; set; }
        public virtual string Code { get; set; }
        public virtual bool RememberMe { get; set; }
        public virtual bool RememberBrowser { get; set; }

        public virtual SignInResult SignInResult { get; set; }
    }
}