namespace ServiceBricks.Security
{
    /// <summary>
    /// This is a process when a user changes their profile information.
    /// </summary>
    public partial class UserProfileChangeProcess : DomainProcess
    {
        public UserProfileChangeProcess(string userStorageKey)
        {
            UserStorageKey = userStorageKey;
        }

        public virtual string UserStorageKey { get; set; }
    }
}