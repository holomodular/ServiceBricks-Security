namespace ServiceBricks.Security.Member
{
    /// <summary>
    /// Options for security token
    /// </summary>
    public class SecurityTokenOptions
    {
        /// <summary>
        /// Valid issuer
        /// </summary>
        public string ValidIssuer { get; set; }

        /// <summary>
        /// Valid audience
        /// </summary>
        public string ValidAudience { get; set; }

        /// <summary>
        /// The secret key
        /// </summary>
        public string SecretKey { get; set; }

        /// <summary>
        /// Expire minutes
        /// </summary>
        public int ExpireMinutes { get; set; }
    }
}