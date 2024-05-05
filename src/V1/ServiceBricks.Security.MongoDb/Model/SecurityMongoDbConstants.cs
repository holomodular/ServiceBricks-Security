namespace ServiceBricks.Security.MongoDb
{
    /// <summary>
    /// This is constants for the Security module.
    /// </summary>
    public static class SecurityMongoDbConstants
    {
        public const string APPSETTINGS_CONNECTION_STRING = "ServiceBricks:Security:MongoDb:ConnectionString";
        public const string APPSETTINGS_DATABASE_NAME = "ServiceBricks:Security:MongoDb:DatabaseName";

        public const string COLLECTIONNAME_PREFIX = "Security";

        public static string GetCollectionName(string tableName)
        {
            return COLLECTIONNAME_PREFIX + tableName;
        }
    }
}