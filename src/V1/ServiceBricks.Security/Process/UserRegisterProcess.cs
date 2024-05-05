namespace ServiceBricks.Security
{
    /// <summary>
    /// This is a process when a user registers.
    /// </summary>
    public class UserRegisterProcess : DomainProcess
    {
        public UserRegisterProcess(
            string email,
            string password,
            bool createEmail = true,
            bool emailConfirmed = false)
        {
            Email = email;
            Password = password;
            CreateEmail = createEmail;
            EmailConfirmed = emailConfirmed;
        }

        public string Email { get; set; }
        public string Password { get; set; }
        public bool CreateEmail { get; set; }
        public bool EmailConfirmed { get; set; }
    }
}