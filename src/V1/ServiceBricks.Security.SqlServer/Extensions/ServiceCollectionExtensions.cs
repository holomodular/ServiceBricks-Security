using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceBricks.Security.EntityFrameworkCore;
using ServiceBricks.Storage.SqlServer;

namespace ServiceBricks.Security.SqlServer
{
    /// <summary>
    /// Extensions methods to add the ServiceBricks Security module to the IServiceCollection.
    /// </summary>
    public static partial class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add the ServiceBricks Security module to the IServiceCollection.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddServiceBricksSecuritySqlServer(this IServiceCollection services, IConfiguration configuration)
        {
            // AI: Add parent module
            services.AddServiceBricksSecurityEntityFrameworkCore(configuration);

            // AI: Add the module to the ModuleRegistry
            ModuleRegistry.Instance.Register(SecuritySqlServerModule.Instance);

            // AI: Add module business rules
            SecuritySqlServerModuleAddRule.Register(BusinessRuleRegistry.Instance);
            ModuleSetStartedRule<SecuritySqlServerModule>.Register(BusinessRuleRegistry.Instance);
            SqlServerDatabaseMigrationRule<SecuritySqlServerModule, SecuritySqlServerContext>.Register(BusinessRuleRegistry.Instance);

            return services;
        }
    }
}