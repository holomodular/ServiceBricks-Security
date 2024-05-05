namespace ServiceBricks.Security
{
    /// <summary>
    /// This is an event when a user registers as an admin.
    /// </summary>
    public class UserRegisterAdminProcess : UserRegisterProcess
    {
        public UserRegisterAdminProcess(
            string email,
            string password) : base(email, password)
        {
        }
    }
}