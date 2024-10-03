using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceBricks.Security.EntityFrameworkCore;
using ServiceBricks.Storage.EntityFrameworkCore;

namespace ServiceBricks.Security.Cosmos
{
    /// <summary>
    /// Extensions for adding the ServiceBricks Security Cosmos module to the service collection.
    /// </summary>
    public static partial class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add the ServiceBricks Security Cosmos module to the service collection.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddServiceBricksSecurityCosmos(this IServiceCollection services, IConfiguration configuration)
        {
            // AI: Add the parent module
            services.AddServiceBricksSecurityEntityFrameworkCore(configuration);

            // AI: Add the module to the ModuleRegistry
            ModuleRegistry.Instance.Register(SecurityCosmosModule.Instance);

            // AI: Add module business rules
            SecurityCosmosModuleAddRule.Register(BusinessRuleRegistry.Instance);
            EntityFrameworkCoreDatabaseEnsureCreatedRule<SecurityCosmosModule, SecurityCosmosContext>.Register(BusinessRuleRegistry.Instance);
            ModuleSetStartedRule<SecurityCosmosModule>.Register(BusinessRuleRegistry.Instance);

            return services;
        }
    }
}