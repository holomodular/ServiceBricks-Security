namespace ServiceBricks.Security
{
    /// <summary>
    /// This is a process when the user forgets their password.
    /// </summary>
    public partial class UserForgotPasswordProcess : DomainProcess<string>
    {
        public UserForgotPasswordProcess(string userStorageKey)
        {
            DomainObject = userStorageKey;
        }
    }
}