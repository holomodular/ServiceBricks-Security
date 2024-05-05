using Azure.Data.Tables;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ServiceBricks.Storage.AzureDataTables;

namespace ServiceBricks.Security.AzureDataTables
{
    /// <summary>
    /// This is a storage repository for the Security module.
    /// </summary>
    /// <typeparam name="TDomain"></typeparam>
    public class SecurityStorageRepository<TDomain> : AzureDataTablesStorageRepository<TDomain>
        where TDomain : class, IAzureDataTablesDomainObject<TDomain>, new()
    {
        public SecurityStorageRepository(
            ILoggerFactory logFactory,
            IConfiguration configuration)
            : base(logFactory)
        {
            ConnectionString = configuration.GetAzureDataTablesConnectionString(
                            SecurityAzureDataTablesConstants.APPSETTINGS_CONNECTION_STRING);
            TableName = SecurityAzureDataTablesConstants.GetTableName(typeof(TDomain).Name);
        }
    }
}