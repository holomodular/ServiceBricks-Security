using Azure.Data.Tables;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceBricks.Storage.AzureDataTables;

namespace ServiceBricks.Security.AzureDataTables
{
    /// <summary>
    /// IApplicationBuilder extensions for the Security Brick.
    /// </summary>
    public static partial class ApplicationBuilderExtensions
    {
        public static bool ModuleStarted = false;

        public static IApplicationBuilder StartServiceBricksSecurityAzureDataTables(this IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var configuration = serviceScope.ServiceProvider.GetRequiredService<IConfiguration>();

                var connectionString = configuration.GetAzureDataTablesConnectionString(
                    SecurityAzureDataTablesConstants.APPSETTING_CONNECTION_STRING);

                // Create each table if not exists
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
            }
            ModuleStarted = true;

            // Start core
            applicationBuilder.StartServiceBricksSecurity();

            return applicationBuilder;
        }
    }
}