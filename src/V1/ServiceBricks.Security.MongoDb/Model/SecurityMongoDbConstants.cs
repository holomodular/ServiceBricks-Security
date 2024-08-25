namespace ServiceBricks.Security.MongoDb
{
    /// <summary>
    /// These are constants for the ServiceBricks.Security.MongoDb namespace.
    /// </summary>
    public static partial class SecurityMongoDbConstants
    {
        /// <summary>
        /// AppSetting key for the connection string.
        /// </summary>
        public const string APPSETTING_CONNECTION_STRING = "ServiceBricks:Security:Storage:MongoDb:ConnectionString";

        /// <summary>
        /// AppSetting key for the database name.
        /// </summary>
        public const string APPSETTING_DATABASE = "ServiceBricks:Security:Storage:MongoDb:Database";

        /// <summary>
        /// The prefix for the collection name.
        /// </summary>
        public const string COLLECTIONNAME_PREFIX = "Security";

        /// <summary>
        /// Get the collection name for the table.
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static string GetCollectionName(string tableName)
        {
            string replacename = tableName.Replace("Application", string.Empty);
            return COLLECTIONNAME_PREFIX + replacename;
        }
    }
}