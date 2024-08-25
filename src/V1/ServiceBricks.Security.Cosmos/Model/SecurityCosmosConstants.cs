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
        /// The container prefix name.
        /// </summary>
        public const string CONTAINER_PREFIX = "Security";

        /// <summary>
        /// Get the container name for the given table name.
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static string GetContainerName(string tableName)
        {
            string replaceName = tableName.Replace("Application", string.Empty);
            return CONTAINER_PREFIX + replaceName;
        }
    }
}