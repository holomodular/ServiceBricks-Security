namespace ServiceBricks.Security
{
    public class SecurityTokenOptions
    {
        public string ValidIssuer { get; set; }
        public string ValidAudience { get; set; }
        public string SecretKey { get; set; }
        public int ExpireMinutes { get; set; }
    }
}