namespace ServiceBricks.Security
{
    /// <summary>
    /// This is a process that sends an email to confirm a user's email.
    /// </summary>
    public partial class SendConfirmEmailProcess : DomainProcess
    {
        public SendConfirmEmailProcess(ApplicationUserDto applicationUser, string callbackUrl)
        {
            ApplicationUser = applicationUser;
            CallbackUrl = callbackUrl;
        }

        public ApplicationUserDto ApplicationUser { get; set; }
        public string CallbackUrl { get; set; }
    }
}