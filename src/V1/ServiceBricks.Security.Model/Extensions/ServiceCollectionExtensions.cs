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

        /// <summary>
        /// Add the ServiceBricks Security clients to the service collection for the API Service references
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddServiceBricksSecurityClientForService(this IServiceCollection services, IConfiguration configuration)
        {
            // AI: Add clients for the API Services
            services.AddScoped<IApiService<UserDto>, UserApiClient>();
            services.AddScoped<IUserApiService, UserApiClient>();

            services.AddScoped<IApiService<UserClaimDto>, UserClaimApiClient>();
            services.AddScoped<IUserClaimApiService, UserClaimApiClient>();

            services.AddScoped<IApiService<UserRoleDto>, UserRoleApiClient>();
            services.AddScoped<IUserRoleApiService, UserRoleApiClient>();

            services.AddScoped<IApiService<UserTokenDto>, UserTokenApiClient>();
            services.AddScoped<IUserTokenApiService, UserTokenApiClient>();

            services.AddScoped<IApiService<UserLoginDto>, UserLoginApiClient>();
            services.AddScoped<IUserLoginApiService, UserLoginApiClient>();

            services.AddScoped<IApiService<RoleDto>, RoleApiClient>();
            services.AddScoped<IRoleApiService, RoleApiClient>();

            services.AddScoped<IApiService<RoleClaimDto>, RoleClaimApiClient>();
            services.AddScoped<IRoleClaimApiService, RoleClaimApiClient>();

            services.AddScoped<IApiService<UserAuditDto>, UserAuditApiClient>();
            services.AddScoped<IUserAuditApiService, UserAuditApiClient>();

            return services;
        }
    }
}