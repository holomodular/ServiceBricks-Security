using Microsoft.AspNetCore.Identity;

namespace ServiceBricks.Security
{
    public class ApplicationSigninResult
    {
        public SignInResult SignInResult { get; set; }
        public ApplicationUserDto User { get; set; }
        public bool EmailNotConfirmed { get; set; }
    }
}