namespace ServiceBricks.Security
{
    /// <summary>
    /// This is an event when a user requests another confirmation email.
    /// </summary>
    public class UserResendConfirmationProcess : DomainProcess
    {
        public UserResendConfirmationProcess(string userStorageKey)
        {
            UserStorageKey = userStorageKey;
        }

        public string UserStorageKey { get; set; }
    }
}