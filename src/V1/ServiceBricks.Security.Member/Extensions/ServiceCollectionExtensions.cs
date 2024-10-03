using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ServiceBricks.Security.Member
{
    /// <summary>
    /// Extensions to add the ServiceBricks Security Member module to the IServiceCollection.
    /// </summary>
    public static partial class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add the ServiceBricks Security Member module to the IServiceCollection.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddServiceBricksSecurityMember(this IServiceCollection services, IConfiguration configuration)
        {
            // AI: Add the module to the ModuleRegistry
            ModuleRegistry.Instance.Register(SecurityMemberModule.Instance);

            // AI: Add module business rules
            SecurityMemberModuleAddRule.Register(BusinessRuleRegistry.Instance);
            ModuleSetStartedRule<SecurityMemberModule>.Register(BusinessRuleRegistry.Instance);

            return services;
        }
    }
}