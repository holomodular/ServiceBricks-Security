namespace ServiceBricks.Security
{
    /// <summary>
    /// This is an event when the user forgets their password.
    /// </summary>
    public class UserForgotPasswordProcess : DomainProcess<string>
    {
        public UserForgotPasswordProcess(string userStorageKey)
        {
            DomainObject = userStorageKey;
        }
    }
}