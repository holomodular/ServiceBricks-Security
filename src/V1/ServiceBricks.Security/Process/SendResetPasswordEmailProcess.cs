namespace ServiceBricks.Security
{
    /// <summary>
    /// This is a process that sends an email.
    /// </summary>
    public partial class SendResetPasswordEmailProcess : DomainProcess
    {
        public SendResetPasswordEmailProcess(UserDto applicationUser, string callbackUrl)
        {
            ApplicationUser = applicationUser;
            CallbackUrl = callbackUrl;
        }

        public virtual UserDto ApplicationUser { get; set; }
        public virtual string CallbackUrl { get; set; }
    }
}