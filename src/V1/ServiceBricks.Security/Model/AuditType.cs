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
        public const string UNKNOWN_TEXT = "UNKNOWN";

        /// <summary>
        /// REGISTER
        /// </summary>
        public const string REGISTER_TEXT = "REGISTER";

        /// <summary>
        /// CONFIRM_EMAIL
        /// </summary>
        public const string CONFIRM_EMAIL_TEXT = "CONFIRM EMAIL";

        /// <summary>
        /// LOGIN
        /// </summary>
        public const string LOGIN_TEXT = "LOGIN";

        /// <summary>
        /// FORGOT_PASSWORD
        /// </summary>
        public const string FORGOT_PASSWORD_TEXT = "FORGOT PASSWORD";

        /// <summary>
        /// PASSWORD_RESET
        /// </summary>
        public const string PASSWORD_RESET_TEXT = "PASSWORD RESET";

        /// <summary>
        /// MFA_START
        /// </summary>
        public const string MFA_START_TEXT = "MFA START";

        /// <summary>
        /// MFA_VERIFY
        /// </summary>
        public const string MFA_VERIFY_TEXT = "MFA VERIFY";

        /// <summary>
        /// LOGOUT
        /// </summary>
        public const string LOGOUT_TEXT = "LOGOUT";

        /// <summary>
        /// INVALID_PASSWORD
        /// </summary>
        public const string INVALID_PASSWORD_TEXT = "INVALID PASSWORD";

        /// <summary>
        /// RESEND_CONFIRMATION
        /// </summary>
        public const string RESEND_CONFIRMATION_TEXT = "RESEND CONFIRMATION";

        /// <summary>
        /// PASSWORD_CHANGE
        /// </summary>
        public const string PASSWORD_CHANGE_TEXT = "PASSWORD CHANGE";

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
                new AuditType(){ Key = UNKNOWN_TEXT, Name = UNKNOWN_TEXT },
                new AuditType(){ Key = REGISTER_TEXT, Name = REGISTER_TEXT },
                new AuditType(){ Key = CONFIRM_EMAIL_TEXT, Name = CONFIRM_EMAIL_TEXT },
                new AuditType(){ Key = LOGIN_TEXT, Name = LOGIN_TEXT },
                new AuditType(){ Key = FORGOT_PASSWORD_TEXT, Name = FORGOT_PASSWORD_TEXT },
                new AuditType(){ Key = PASSWORD_RESET_TEXT, Name = PASSWORD_RESET_TEXT },
                new AuditType(){ Key = MFA_START_TEXT, Name = MFA_START_TEXT },
                new AuditType(){ Key = MFA_VERIFY_TEXT, Name = MFA_VERIFY_TEXT },
                new AuditType(){ Key = LOGOUT_TEXT, Name = LOGOUT_TEXT },
                new AuditType(){ Key = INVALID_PASSWORD_TEXT, Name = INVALID_PASSWORD_TEXT },
                new AuditType(){ Key = RESEND_CONFIRMATION_TEXT, Name = RESEND_CONFIRMATION_TEXT },
                new AuditType(){ Key = PASSWORD_CHANGE_TEXT, Name = PASSWORD_CHANGE_TEXT },
                new AuditType(){ Key = PROFILE_CHANGE_TEXT, Name = PROFILE_CHANGE_TEXT },
            };
        }
    }
}