using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ServiceBricks.Security
{
    /// <summary>
    /// Extension methods for IServiceCollection to add ServiceBricks Security services.
    /// </summary>
    public static partial class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add ServiceBricks Security services to the application.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddServiceBricksSecurity(this IServiceCollection services, IConfiguration configuration)
        {
            // AI: Add the module to the ModuleRegistry
            ModuleRegistry.Instance.Register(SecurityModule.Instance);

            // AI: Add module business rules
            SecurityModuleAddRule.Register(BusinessRuleRegistry.Instance);
            SecurityModuleStartRule.Register(BusinessRuleRegistry.Instance);
            ModuleSetStartedRule<SecurityModule>.Register(BusinessRuleRegistry.Instance);

            return services;
        }      
    }
}