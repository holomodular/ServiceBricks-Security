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

        public string Email { get; set; }
        public string Password { get; set; }
        public string Code { get; set; }
    }
}