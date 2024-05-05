namespace ServiceBricks.Security
{
    /// <summary>
    /// This is an event when a user changes their profile information.
    /// </summary>
    public class UserProfileChangeProcess : DomainProcess
    {
        public UserProfileChangeProcess(string userStorageKey)
        {
            UserStorageKey = userStorageKey;
        }

        public string UserStorageKey { get; set; }
    }
}