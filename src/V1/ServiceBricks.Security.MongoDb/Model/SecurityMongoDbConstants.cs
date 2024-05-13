namespace ServiceBricks.Security.MongoDb
{
    /// <summary>
    /// This is constants for the Security module.
    /// </summary>
    public static class SecurityMongoDbConstants
    {
        public const string APPSETTING_CONNECTION_STRING = "ServiceBricks:Security:Storage:MongoDb:ConnectionString";
        public const string APPSETTING_DATABASE = "ServiceBricks:Security:Storage:MongoDb:Database";

        public const string COLLECTIONNAME_PREFIX = "Security";

        public static string GetCollectionName(string tableName)
        {
            return COLLECTIONNAME_PREFIX + tableName;
        }
    }
}