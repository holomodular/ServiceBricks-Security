namespace ServiceBricks.Security
{
    /// <summary>
    /// This is a process that sends an email to confirm a user's email.
    /// </summary>
    public partial class SendConfirmEmailProcess : DomainProcess
    {
        public SendConfirmEmailProcess(UserDto applicationUser, string callbackUrl)
        {
            ApplicationUser = applicationUser;
            CallbackUrl = callbackUrl;
        }

        public virtual UserDto ApplicationUser { get; set; }
        public virtual string CallbackUrl { get; set; }
    }
}