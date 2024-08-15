namespace ServiceBricks.Security
{
    /// <summary>
    /// This is a a process when an invalid password is entered.
    /// </summary>
    public partial class UserInvalidPasswordProcess : DomainProcess
    {
        public UserInvalidPasswordProcess(string userStorageKey, string email)
        {
            UserStorageKey = userStorageKey;
            Email = email;
        }

        public virtual string Email { get; set; }
        public virtual string UserStorageKey { get; set; }
    }
}