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

        /// <summary>
        /// Add ServiceBricks Security client services to the application.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddServiceBricksSecurityClient(this IServiceCollection services, IConfiguration configuration)
        {
            // AI: Add clients for the module for each DTO
            services.AddScoped<IApiClient<UserDto>, UserApiClient>();
            services.AddScoped<IUserApiClient, UserApiClient>();

            services.AddScoped<IApiClient<UserClaimDto>, UserClaimApiClient>();
            services.AddScoped<IUserClaimApiClient, UserClaimApiClient>();

            services.AddScoped<IApiClient<UserRoleDto>, UserRoleApiClient>();
            services.AddScoped<IUserRoleApiClient, UserRoleApiClient>();

            services.AddScoped<IApiClient<UserTokenDto>, UserTokenApiClient>();
            services.AddScoped<IUserTokenApiClient, UserTokenApiClient>();

            services.AddScoped<IApiClient<UserLoginDto>, UserLoginApiClient>();
            services.AddScoped<IUserLoginApiClient, UserLoginApiClient>();

            services.AddScoped<IApiClient<RoleDto>, RoleApiClient>();
            services.AddScoped<IRoleApiClient, RoleApiClient>();

            services.AddScoped<IApiClient<RoleClaimDto>, RoleClaimApiClient>();
            services.AddScoped<IRoleClaimApiClient, RoleClaimApiClient>();

            services.AddScoped<IApiClient<UserAuditDto>, UserAuditApiClient>();
            services.AddScoped<IUserAuditApiClient, UserAuditApiClient>();

            services.AddScoped<IAuthenticationApiClient, AuthenticationApiClient>();
            return services;
        }
    }
}