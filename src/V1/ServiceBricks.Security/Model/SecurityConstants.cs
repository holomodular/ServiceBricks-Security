namespace ServiceBricks.Security
{
    /// <summary>
    /// These are constants for the ServiceBricks.Security namespace.
    /// </summary>
    public static partial class SecurityConstants
    {
        /// <summary>
        /// AppSetting key for the security token.
        /// </summary>
        public const string APPSETTING_SECURITY_TOKEN = "ServiceBricks:Security:Token";

        /// <summary>
        /// AppSetting key for the callback url.
        /// </summary>
        public const string APPSETTING_SECURITY_CALLBACKURL = "ServiceBricks:Security:CallbackUrl";

        /// <summary>
        /// AppSetting key for the client api configuration.
        /// </summary>
        public const string APPSETTING_CLIENT_APICONFIG = @"ServiceBricks:Security:Client:Api";

        /// <summary>
        /// The authentication scheme for the ServiceBricks.Security module.
        /// </summary>
        public const string SERVICEBRICKS_AUTHENTICATION_SCHEME = "SERVICEBRICKS_AUTH";
    }
}