namespace ServiceBricks.Security
{
    /// <summary>
    /// This class is used to store the security token options.
    /// </summary>
    public partial class SecurityTokenOptions
    {
        /// <summary>
        /// Issuer of the token
        /// </summary>
        public string ValidIssuer { get; set; }

        /// <summary>
        /// Audience of the token
        /// </summary>
        public string ValidAudience { get; set; }

        /// <summary>
        /// Secret key for the token
        /// </summary>
        public string SecretKey { get; set; }

        /// <summary>
        /// Expiration time for the token
        /// </summary>
        public int ExpireMinutes { get; set; }
    }
}