namespace ServiceBricks.Security
{
    /// <summary>
    /// This is a process when a user logs in.
    /// </summary>
    public partial class UserLoginProcess : DomainProcess
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

        public virtual string Email { get; set; }
        public virtual string Password { get; set; }

        public virtual bool RememberMe { get; set; }

        public virtual ApplicationSigninResult ApplicationSigninResult { get; set; }

        public virtual UserDto User { get; set; }
    }
}