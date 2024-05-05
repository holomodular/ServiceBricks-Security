namespace ServiceBricks.Security
{
    /// <summary>
    /// This is an event when a user logs in.
    /// </summary>
    public class UserLoginProcess : DomainProcess
    {
        public UserLoginProcess(
            string email,
            string password,
            bool rememberMe)
        {
            Email = email;
            Password = password;
            RememberMe = rememberMe;
        }

        public string Email { get; set; }
        public string Password { get; set; }

        public bool RememberMe { get; set; }

        public ApplicationSigninResult ApplicationSigninResult { get; set; }
    }
}