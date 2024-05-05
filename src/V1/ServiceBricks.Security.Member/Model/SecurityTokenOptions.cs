namespace ServiceBricks.Security.Member
{
    public class SecurityTokenOptions
    {
        public bool ValidateIssuer { get; set; }
        public string ValidIssuer { get; set; }
        public bool ValidateAudience { get; set; }
        public string ValidAudience { get; set; }
        public bool ValidateIssuerSigningKey { get; set; }
        public string SecretKey { get; set; }
        public int ExpireMinutes { get; set; }
    }
}