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

        public string Provider { get; set; }
        public string Code { get; set; }
        public bool RememberMe { get; set; }
        public bool RememberBrowser { get; set; }

        public SignInResult SignInResult { get; set; }
    }
}