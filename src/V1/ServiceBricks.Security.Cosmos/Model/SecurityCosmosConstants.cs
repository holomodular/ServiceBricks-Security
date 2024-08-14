namespace ServiceBricks.Security.Cosmos
{
    /// <summary>
    /// These are constants for the Security module.
    /// </summary>
    public static partial class SecurityCosmosConstants
    {
        /// <summary>
        /// AppSetting key for the Cosmos connection string.
        /// </summary>
        public const string APPSETTING_CONNECTION_STRING = "ServiceBricks:Security:Storage:Cosmos:ConnectionString";

        /// <summary>
        /// AppSetting key for the Cosmos database.
        /// </summary>
        public const string APPSETTING_DATABASE = "ServiceBricks:Security:Storage:Cosmos:Database";

        /// <summary>
        /// The database schema name.
        /// </summary>
        public const string DATABASE_SCHEMA_NAME = "Security";
    }
}