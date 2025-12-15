using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceBricks.Security.EntityFrameworkCore;
using ServiceBricks.Storage.Postgres;

namespace ServiceBricks.Security.Postgres
{
    /// <summary>
    /// Extensions to add the ServiceBricks Security Postgres module to the service collection.
    /// </summary>
    public static partial class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add the ServiceBricks Security Postgres module to the service collection.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddServiceBricksSecurityPostgres(this IServiceCollection services, IConfiguration configuration)
        {
            // AI: Add parent module
            services.AddServiceBricksSecurityEntityFrameworkCore(configuration);

            // AI: Add the module to the ModuleRegistry
            ModuleRegistry.Instance.Register(SecurityPostgresModule.Instance);

            // AI: Add module business rules
            SecurityPostgresModuleAddRule.Register(BusinessRuleRegistry.Instance);
            //ModuleSetStartedRule<SecurityPostgresModule>.Register(BusinessRuleRegistry.Instance);
            //PostgresDatabaseMigrationRule<SecurityModule, SecurityPostgresContext>.Register(BusinessRuleRegistry.Instance);

            return services;
        }
    }
}