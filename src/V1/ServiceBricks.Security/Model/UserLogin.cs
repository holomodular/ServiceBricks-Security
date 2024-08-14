namespace ServiceBricks.Security
{
    /// <summary>
    /// Class representing a user login.
    /// </summary>
    public partial class UserLogin
    {
        /// <summary>
        /// The email addresses.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// The password.
        /// </summary>
        public string Password { get; set; }
    }
}