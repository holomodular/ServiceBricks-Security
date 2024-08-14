namespace ServiceBricks.Security.Postgres
{
    /// <summary>
    /// These are constants for the ServiceBricks Security Postgres module.
    /// </summary>
    public static partial class SecurityPostgresConstants
    {
        /// <summary>
        /// Appsetting key for the database name.
        /// </summary>
        public const string APPSETTING_CONNECTION_STRING = "ServiceBricks:Security:Storage:Postgres:ConnectionString";

        /// <summary>
        /// The name of the database schema.
        /// </summary>
        public const string DATABASE_SCHEMA_NAME = "Security";
    }
}