namespace ServiceBricks.Security.AzureDataTables
{
    /// <summary>
    /// These are constants for the ServiceBricks.Security.AzureDataTables namespace.
    /// </summary>
    public static partial class SecurityAzureDataTablesConstants
    {
        /// <summary>
        /// Application setting key for the Azure Data Tables connection string.
        /// </summary>
        public const string APPSETTING_CONNECTION_STRING = "ServiceBricks:Security:Storage:AzureDataTables:ConnectionString";

        /// <summary>
        /// The default table name prefix.
        /// </summary>
        public const string TABLENAME_PREFIX = "Security";

        /// <summary>
        /// Get the table name using the default prefix.
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static string GetTableName(string tableName)
        {
            string replaceName = tableName.Replace("Application", string.Empty);
            return TABLENAME_PREFIX + replaceName;
        }
    }
}