namespace ServiceBricks.Security
{
    /// <summary>
    /// This is a process when a user requests another confirmation email.
    /// </summary>
    public partial class UserResendConfirmationProcess : DomainProcess
    {
        public UserResendConfirmationProcess(string userStorageKey)
        {
            UserStorageKey = userStorageKey;
        }

        public virtual string UserStorageKey { get; set; }
    }
}