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

        public string Email { get; set; }
        public string UserStorageKey { get; set; }
    }
}