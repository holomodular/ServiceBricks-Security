using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ServiceBricks.Security.EntityFrameworkCore
{
    /// <summary>
    /// Add ServiceBricks Security EntityFrameworkCore services to the application.
    /// </summary>
    public static partial class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add ServiceBricks Security EntityFrameworkCore services to the application.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddServiceBricksSecurityEntityFrameworkCore(this IServiceCollection services, IConfiguration configuration)
        {
            // AI: Add the parent module
            services.AddServiceBricksSecurity(configuration);

            // AI: Add the module to the ModuleRegistry
            ModuleRegistry.Instance.Register(SecurityEntityFrameworkCoreModule.Instance);

            // AI: Add module business rules
            SecurityEntityFrameworkCoreModuleAddRule.Register(BusinessRuleRegistry.Instance);
            ModuleSetStartedRule<SecurityEntityFrameworkCoreModule>.Register(BusinessRuleRegistry.Instance);

            return services;
        }
    }
}