using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ServiceBricks.Storage.MongoDb;

namespace ServiceBricks.Security.MongoDb
{
    /// <summary>
    /// This is a storage repository for the Security module.
    /// </summary>
    /// <typeparam name="TDomain"></typeparam>
    public class SecurityStorageRepository<TDomain> : MongoDbStorageRepository<TDomain>
        where TDomain : class, IMongoDbDomainObject<TDomain>, new()
    {
        public SecurityStorageRepository(
            ILoggerFactory logFactory,
            IConfiguration configuration)
            : base(logFactory)
        {
            ConnectionString = configuration.GetMongoDbConnectionString(
                SecurityMongoDbConstants.APPSETTINGS_CONNECTION_STRING);
            DatabaseName = configuration.GetMongoDbDatabaseName(
                SecurityMongoDbConstants.APPSETTINGS_DATABASE_NAME);
            CollectionName = SecurityMongoDbConstants.GetCollectionName(typeof(TDomain).Name);
        }
    }
}