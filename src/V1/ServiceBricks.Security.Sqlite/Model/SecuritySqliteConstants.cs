namespace ServiceBricks.Security.Sqlite
{
    /// <summary>
    /// These are constants for the Security module.
    /// </summary>
    public static partial class SecuritySqliteConstants
    {
        /// <summary>
        /// Appsetting key for the database name.
        /// </summary>
        public const string APPSETTING_CONNECTION_STRING = "ServiceBricks:Security:Storage:Sqlite:ConnectionString";

        /// <summary>
        /// The name of the database schema.
        /// </summary>
        public const string DATABASE_SCHEMA_NAME = "Security";
    }
}