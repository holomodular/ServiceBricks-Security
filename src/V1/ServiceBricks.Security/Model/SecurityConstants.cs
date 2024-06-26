﻿namespace ServiceBricks.Security
{
    /// <summary>
    /// This is constants for the Security module.
    /// </summary>
    public static partial class SecurityConstants
    {
        public const string APPSETTING_SECURITY_TOKEN = "ServiceBricks:Security:Token";
        public const string APPSETTING_SECURITY_CALLBACKURL = "ServiceBricks:Security:CallbackUrl";
        public const string APPSETTING_CLIENT_APICONFIG = @"ServiceBricks:Security:Client:Api";

        public const string ROLE_ADMIN_NAME = "ADMIN";
        public const string ROLE_USER_NAME = "USER";

        public const string SERVICEBRICKS_AUTHENTICATION_SCHEME = "SERVICEBRICKS_AUTH";
    }
}