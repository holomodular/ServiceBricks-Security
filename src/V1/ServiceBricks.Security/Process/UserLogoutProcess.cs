namespace ServiceBricks.Security
{
    /// <summary>
    /// This is an event when the user logs out.
    /// </summary>
    public class UserLogoutProcess : DomainProcess
    {
        public UserLogoutProcess(string userStorageKey)
        {
            UserStorageKey = userStorageKey;
        }

        public string UserStorageKey { get; set; }
    }
}