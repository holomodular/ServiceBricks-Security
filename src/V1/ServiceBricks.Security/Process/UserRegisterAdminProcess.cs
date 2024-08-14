namespace ServiceBricks.Security
{
    /// <summary>
    /// This is a process when a user registers as an admin.
    /// </summary>
    public partial class UserRegisterAdminProcess : UserRegisterProcess
    {
        public UserRegisterAdminProcess(
            string email,
            string password) : base(email, password)
        {
        }
    }
}