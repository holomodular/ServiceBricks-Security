using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceBricks.Security.EntityFrameworkCore;
using ServiceBricks.Storage.Sqlite;

namespace ServiceBricks.Security.Sqlite
{
    /// <summary>
    /// Extensions to add the ServiceBricks Security Sqlite module to the service collection.
    /// </summary>
    public static partial class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add the ServiceBricks Security Sqlite module to the service collection.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddServiceBricksSecuritySqlite(this IServiceCollection services, IConfiguration configuration)
        {
            // AI: Add parent module
            services.AddServiceBricksSecurityEntityFrameworkCore(configuration);

            // AI: Add the module to the ModuleRegistry
            ModuleRegistry.Instance.Register(SecuritySqliteModule.Instance);

            // AI: Add module business rules
            SecuritySqliteModuleAddRule.Register(BusinessRuleRegistry.Instance);
            ModuleSetStartedRule<SecuritySqliteModule>.Register(BusinessRuleRegistry.Instance);
            SqliteDatabaseMigrationRule<SecuritySqliteModule, SecuritySqliteContext>.Register(BusinessRuleRegistry.Instance);

            return services;
        }
    }
}