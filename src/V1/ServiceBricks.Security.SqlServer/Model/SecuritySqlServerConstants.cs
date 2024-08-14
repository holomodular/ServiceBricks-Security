namespace ServiceBricks.Security.SqlServer
{
    /// <summary>
    /// These are constants for the ServiceBricks Security SqlServer module.
    /// </summary>
    public static partial class SecuritySqlServerConstants
    {
        /// <summary>
        /// Appsetting key for the database name.
        /// </summary>
        public const string APPSETTING_CONNECTION_STRING = "ServiceBricks:Security:Storage:SqlServer:ConnectionString";

        /// <summary>
        /// The name of the database schema.
        /// </summary>
        public const string DATABASE_SCHEMA_NAME = "Security";
    }
}