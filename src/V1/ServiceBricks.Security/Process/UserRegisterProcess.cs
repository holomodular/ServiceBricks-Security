namespace ServiceBricks.Security
{
    /// <summary>
    /// This is a process when a user registers.
    /// </summary>
    public partial class UserRegisterProcess : DomainProcess
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

        public virtual string Email { get; set; }
        public virtual string Password { get; set; }
        public virtual bool CreateEmail { get; set; }
        public virtual bool EmailConfirmed { get; set; }
    }
}