namespace ServiceBricks.Security
{
    /// <summary>
    /// This is a process when a user resets their password with a code.
    /// </summary>
    public partial class UserPasswordResetProcess : DomainProcess
    {
        public UserPasswordResetProcess(string email, string password, string code)
        {
            Email = email;
            Password = password;
            Code = code;
        }

        public virtual string Email { get; set; }
        public virtual string Password { get; set; }
        public virtual string Code { get; set; }
    }
}