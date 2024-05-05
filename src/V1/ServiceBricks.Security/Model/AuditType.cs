namespace ServiceBricks.Security
{
    public class AuditType
    {
        public const string UNKNOWN = "UNKNOWN";
        public const string CONFIRM_EMAIL = "CONFIRM EMAIL";
        public const string REGISTER = "REGISTER";
        public const string FORGOT_PASSWORD = "FORGOT PASSWORD";
        public const string PASSWORD_RESET = "PASSWORD RESET";
        public const string LOGIN = "LOGIN";
        public const string MFA_START = "MFA START";
        public const string MFA_VERIFY = "MFA VERIFY";
        public const string LOGOUT = "LOGOUT";
        public const string INVALID_PASSWORD = "INVALID PASSWORD";
        public const string RESEND_CONFIRMATION = "RESEND CONFIRMATION";
        public const string PASSWORD_CHANGE = "PASSWORD CHANGE";
        public const string PROFILE_CHANGE = "PROFILE CHANGE";
    }
}