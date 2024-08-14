namespace ServiceBricks.Security
{
    /// <summary>
    /// This is a domaintype for the AuditType domain object.
    /// </summary>
    public partial class AuditType : DomainType
    {
        /// <summary>
        /// Unknown
        /// </summary>
        public const int UNKNOWN = 1;

        /// <summary>
        /// Unknown
        /// </summary>
        public const string UNKNOWN_TEXT = "UNKNOWN";

        /// <summary>
        /// REGISTER
        /// </summary>
        public const int REGISTER = 2;

        /// <summary>
        /// REGISTER
        /// </summary>
        public const string REGISTER_TEXT = "REGISTER";

        /// <summary>
        /// CONFIRM_EMAIL
        /// </summary>
        public const int CONFIRM_EMAIL = 3;

        /// <summary>
        /// CONFIRM_EMAIL
        /// </summary>
        public const string CONFIRM_EMAIL_TEXT = "CONFIRM EMAIL";

        /// <summary>
        /// LOGIN
        /// </summary>
        public const int LOGIN = 4;

        /// <summary>
        /// LOGIN
        /// </summary>
        public const string LOGIN_TEXT = "LOGIN";

        /// <summary>
        /// FORGOT_PASSWORD
        /// </summary>
        public const int FORGOT_PASSWORD = 5;

        /// <summary>
        /// FORGOT_PASSWORD
        /// </summary>
        public const string FORGOT_PASSWORD_TEXT = "FORGOT PASSWORD";

        /// <summary>
        /// PASSWORD_RESET
        /// </summary>
        public const int PASSWORD_RESET = 6;

        /// <summary>
        /// PASSWORD_RESET
        /// </summary>
        public const string PASSWORD_RESET_TEXT = "PASSWORD RESET";

        /// <summary>
        /// MFA_START
        /// </summary>
        public const int MFA_START = 7;

        /// <summary>
        /// MFA_START
        /// </summary>
        public const string MFA_START_TEXT = "MFA START";

        /// <summary>
        /// MFA_VERIFY
        /// </summary>
        public const int MFA_VERIFY = 8;

        /// <summary>
        /// MFA_VERIFY
        /// </summary>
        public const string MFA_VERIFY_TEXT = "MFA VERIFY";

        /// <summary>
        /// LOGOUT
        /// </summary>
        public const int LOGOUT = 9;

        /// <summary>
        /// LOGOUT
        /// </summary>
        public const string LOGOUT_TEXT = "LOGOUT";

        /// <summary>
        /// INVALID_PASSWORD
        /// </summary>
        public const int INVALID_PASSWORD = 10;

        /// <summary>
        /// INVALID_PASSWORD
        /// </summary>
        public const string INVALID_PASSWORD_TEXT = "INVALID PASSWORD";

        /// <summary>
        /// RESEND_CONFIRMATION
        /// </summary>
        public const int RESEND_CONFIRMATION = 11;

        /// <summary>
        /// RESEND_CONFIRMATION
        /// </summary>
        public const string RESEND_CONFIRMATION_TEXT = "RESEND CONFIRMATION";

        /// <summary>
        /// PASSWORD_CHANGE
        /// </summary>
        public const int PASSWORD_CHANGE = 12;

        /// <summary>
        /// PASSWORD_CHANGE
        /// </summary>
        public const string PASSWORD_CHANGE_TEXT = "PASSWORD CHANGE";

        /// <summary>
        /// PROFILE_CHANGE
        /// </summary>
        public const int PROFILE_CHANGE = 13;

        /// <summary>
        /// PROFILE_CHANGE
        /// </summary>
        public const string PROFILE_CHANGE_TEXT = "PROFILE CHANGE";

        /// <summary>
        /// Get all AuditTypes.
        /// </summary>
        /// <returns></returns>
        public static List<AuditType> GetAll()
        {
            return new List<AuditType>()
            {
                new AuditType(){ Key = UNKNOWN, Name = UNKNOWN_TEXT },
                new AuditType(){ Key = REGISTER, Name = REGISTER_TEXT },
                new AuditType(){ Key = CONFIRM_EMAIL, Name = CONFIRM_EMAIL_TEXT },
                new AuditType(){ Key = LOGIN, Name = LOGIN_TEXT },
                new AuditType(){ Key = FORGOT_PASSWORD, Name = FORGOT_PASSWORD_TEXT },
                new AuditType(){ Key = PASSWORD_RESET, Name = PASSWORD_RESET_TEXT },
                new AuditType(){ Key = MFA_START, Name = MFA_START_TEXT },
                new AuditType(){ Key = MFA_VERIFY, Name = MFA_VERIFY_TEXT },
                new AuditType(){ Key = LOGOUT, Name = LOGOUT_TEXT },
                new AuditType(){ Key = INVALID_PASSWORD, Name = INVALID_PASSWORD_TEXT },
                new AuditType(){ Key = RESEND_CONFIRMATION, Name = RESEND_CONFIRMATION_TEXT },
                new AuditType(){ Key = PASSWORD_CHANGE, Name = PASSWORD_CHANGE_TEXT },
                new AuditType(){ Key = PROFILE_CHANGE, Name = PROFILE_CHANGE_TEXT },
            };
        }
    }
}