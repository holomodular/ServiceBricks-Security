using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ServiceBricks.Security.AzureDataTables
{
    /// <summary>
    /// Extensions for the ServiceBricks.Security.AzureDataTables module.
    /// </summary>
    public static partial class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add the ServiceBricks.Security.AzureDataTables module to the service collection.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddServiceBricksSecurityAzureDataTables(this IServiceCollection services, IConfiguration configuration)
        {
            // AI: Add parent module
            services.AddServiceBricksSecurity(configuration);

            // AI: Add the module to the ModuleRegistry
            ModuleRegistry.Instance.Register(SecurityAzureDataTablesModule.Instance);

            // AI: Add module business rules
            SecurityAzureDataTablesModuleAddRule.Register(BusinessRuleRegistry.Instance);
            SecurityAzureDataTablesModuleStartRule.Register(BusinessRuleRegistry.Instance);
            ModuleSetStartedRule<SecurityAzureDataTablesModule>.Register(BusinessRuleRegistry.Instance);

            return services;
        }
    }
}