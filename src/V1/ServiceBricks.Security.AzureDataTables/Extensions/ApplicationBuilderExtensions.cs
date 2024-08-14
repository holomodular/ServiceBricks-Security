using Azure.Data.Tables;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceBricks.Storage.AzureDataTables;

namespace ServiceBricks.Security.AzureDataTables
{
    /// <summary>
    /// Extensions for to start the ServiceBricks.Security.AzureDataTables module.
    /// </summary>
    public static partial class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Flag to indicate if the module has started.
        /// </summary>
        public static bool ModuleStarted = false;

        /// <summary>
        /// Start the ServiceBricks.Security.AzureDataTables module.
        /// </summary>
        /// <param name="applicationBuilder"></param>
        /// <returns></returns>
        public static IApplicationBuilder StartServiceBricksSecurityAzureDataTables(this IApplicationBuilder applicationBuilder)
        {
            // AI: Get the connection string
            var configuration = applicationBuilder.ApplicationServices.GetRequiredService<IConfiguration>();
            var connectionString = configuration.GetAzureDataTablesConnectionString(
                SecurityAzureDataTablesConstants.APPSETTING_CONNECTION_STRING);

            // AI: Create each table in the module
            TableClient tableClient = new TableClient(
                connectionString,
                SecurityAzureDataTablesConstants.GetTableName(nameof(ApplicationUser)));
            tableClient.CreateIfNotExists();

            tableClient = new TableClient(
                connectionString,
                SecurityAzureDataTablesConstants.GetTableName(nameof(ApplicationUserClaim)));
            tableClient.CreateIfNotExists();

            tableClient = new TableClient(
                connectionString,
                SecurityAzureDataTablesConstants.GetTableName(nameof(ApplicationUserLogin)));
            tableClient.CreateIfNotExists();

            tableClient = new TableClient(
                connectionString,
                SecurityAzureDataTablesConstants.GetTableName(nameof(ApplicationUserRole)));
            tableClient.CreateIfNotExists();

            tableClient = new TableClient(
                connectionString,
                SecurityAzureDataTablesConstants.GetTableName(nameof(ApplicationUserToken)));
            tableClient.CreateIfNotExists();

            tableClient = new TableClient(
                connectionString,
                SecurityAzureDataTablesConstants.GetTableName(nameof(ApplicationRole)));
            tableClient.CreateIfNotExists();

            tableClient = new TableClient(
                connectionString,
                SecurityAzureDataTablesConstants.GetTableName(nameof(ApplicationRoleClaim)));
            tableClient.CreateIfNotExists();

            tableClient = new TableClient(
                connectionString,
                SecurityAzureDataTablesConstants.GetTableName(nameof(AuditUser)));
            tableClient.CreateIfNotExists();

            // AI: Set the module started flag
            ModuleStarted = true;

            // AI: Start parent module
            applicationBuilder.StartServiceBricksSecurity();

            return applicationBuilder;
        }
    }
}