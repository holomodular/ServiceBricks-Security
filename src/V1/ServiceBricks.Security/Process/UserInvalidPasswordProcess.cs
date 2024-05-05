namespace ServiceBricks.Security
{
    /// <summary>
    /// This is an event when an invalid password is entered.
    /// </summary>
    public class UserInvalidPasswordProcess : DomainProcess
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