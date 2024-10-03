using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ServiceBricks.Security.MongoDb
{
    /// <summary>
    /// Extensions to add the security module to the service collection.
    /// </summary>
    public static partial class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add the ServiceBricks Security MongoDb module to the service collection.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddServiceBricksSecurityMongoDb(this IServiceCollection services, IConfiguration configuration)
        {
            // AI: Add the parent module
            services.AddServiceBricksSecurity(configuration);

            // AI: Add the module to the ModuleRegistry
            ModuleRegistry.Instance.Register(SecurityMongoDbModule.Instance);

            // AI: Add module business rules
            SecurityMongoDbModuleAddRule.Register(BusinessRuleRegistry.Instance);
            ModuleSetStartedRule<SecurityMongoDbModule>.Register(BusinessRuleRegistry.Instance);

            return services;
        }
    }
}