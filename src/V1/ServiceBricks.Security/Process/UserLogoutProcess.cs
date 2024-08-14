namespace ServiceBricks.Security
{
    /// <summary>
    /// This is a process when the user logs out.
    /// </summary>
    public partial class UserLogoutProcess : DomainProcess
    {
        public UserLogoutProcess(string userStorageKey)
        {
            UserStorageKey = userStorageKey;
        }

        public string UserStorageKey { get; set; }
    }
}