using Azure.Data.Tables;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceBricks.Storage.AzureDataTables;

namespace ServiceBricks.Security.AzureDataTables
{
    /// <summary>
    /// This rule is executed when the ServiceBricks module is added.
    /// </summary>
    public sealed class SecurityAzureDataTablesModuleStartRule : BusinessRule
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public SecurityAzureDataTablesModuleStartRule()
        {
            Priority = PRIORITY_HIGH;
        }

        /// <summary>
        /// Register the rule
        /// </summary>
        public static void Register(IBusinessRuleRegistry registry)
        {
            registry.Register(
                typeof(ModuleStartEvent<SecurityModule>),
                typeof(SecurityAzureDataTablesModuleStartRule));
        }

        /// <summary>
        /// UnRegister the rule.
        /// </summary>
        public static void UnRegister(IBusinessRuleRegistry registry)
        {
            registry.UnRegister(
                typeof(ModuleStartEvent<SecurityModule>),
                typeof(SecurityAzureDataTablesModuleStartRule));
        }

        /// <summary>
        /// Execute the business rule.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override IResponse ExecuteRule(IBusinessRuleContext context)
        {
            var response = new Response();

            // AI: Make sure the context object is the correct type
            if (context == null || context.Object == null)
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, "context"));
                return response;
            }
            var e = context.Object as ModuleStartEvent<SecurityModule>;
            if (e == null || e.DomainObject == null || e.ApplicationBuilder == null)
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, "context"));
                return response;
            }

            // AI: Perform logic

            // AI: Get the connection string
            var configuration = e.ApplicationBuilder.ApplicationServices.GetRequiredService<IConfiguration>();
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
                SecurityAzureDataTablesConstants.GetTableName(nameof(UserAudit)));
            tableClient.CreateIfNotExists();

            return response;
        }
    }
}