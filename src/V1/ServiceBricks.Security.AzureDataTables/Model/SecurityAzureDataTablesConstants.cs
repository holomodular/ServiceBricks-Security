namespace ServiceBricks.Security.AzureDataTables
{
    /// <summary>
    /// This is constants for the Security module.
    /// </summary>
    public static class SecurityAzureDataTablesConstants
    {
        public const string APPSETTING_CONNECTION_STRING = "ServiceBricks:Security:Storage:AzureDataTables:ConnectionString";

        public const string TABLENAME_PREFIX = "Security";

        public static string GetTableName(string tableName)
        {
            return TABLENAME_PREFIX + tableName;
        }
    }
}