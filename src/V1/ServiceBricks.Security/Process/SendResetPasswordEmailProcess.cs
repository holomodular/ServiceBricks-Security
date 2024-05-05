namespace ServiceBricks.Security
{
    /// <summary>
    /// This is a process that sends an email.
    /// </summary>
    public class SendResetPasswordEmailProcess : DomainProcess
    {
        public SendResetPasswordEmailProcess()
        {
        }

        public SendResetPasswordEmailProcess(ApplicationUserDto applicationUser, string callbackUrl)
        {
            ApplicationUser = applicationUser;
            CallbackUrl = callbackUrl;
        }

        public ApplicationUserDto ApplicationUser { get; set; }
        public string CallbackUrl { get; set; }
    }
}