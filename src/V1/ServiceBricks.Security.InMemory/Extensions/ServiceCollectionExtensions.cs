using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using ServiceBricks.Security.EntityFrameworkCore;

namespace ServiceBricks.Security.InMemory
{
    /// <summary>
    /// Extensions to add the ServiceBricks Security InMemory module to the IServiceCollection.
    /// </summary>
    public static partial class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add the ServiceBricks Security InMemory module to the IServiceCollection.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddServiceBricksSecurityInMemory(this IServiceCollection services, IConfiguration configuration)
        {
            // AI: Add the parent module
            services.AddServiceBricksSecurityEntityFrameworkCore(configuration);

            // AI: Add the module to the ModuleRegistry
            ModuleRegistry.Instance.Register(SecurityInMemoryModule.Instance);

            // AI: Add module business rules
            SecurityInMemoryModuleAddRule.Register(BusinessRuleRegistry.Instance);
            ModuleSetStartedRule<SecurityInMemoryModule>.Register(BusinessRuleRegistry.Instance);

            return services;
        }
    }
}