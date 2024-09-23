using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;

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
            ModuleRegistry.Instance.RegisterItem(typeof(SecurityModule), new SecurityModule());

            // AI: Add any custom requirements for the module

            // AI: Add any options for the module
            services.Configure<SecurityTokenOptions>(configuration.GetSection(SecurityConstants.APPSETTING_SECURITY_TOKEN));

            // AI: Add Authentication
            var securityOptions = services.BuildServiceProvider().GetService<IOptions<SecurityTokenOptions>>().Value;
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = SecurityConstants.SERVICEBRICKS_AUTHENTICATION_SCHEME;
                options.DefaultScheme = SecurityConstants.SERVICEBRICKS_AUTHENTICATION_SCHEME;
                options.DefaultChallengeScheme = SecurityConstants.SERVICEBRICKS_AUTHENTICATION_SCHEME;
            })
            .AddCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromMinutes(securityOptions.ExpireMinutes);
                options.LoginPath = "/Login";
                options.LogoutPath = "/Logout";
            }).AddJwtBearer(options =>
            {
                var tokenValidationParams = new TokenValidationParameters()
                {
                    ValidIssuer = securityOptions.ValidIssuer,
                    ValidAudience = securityOptions.ValidAudience,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        System.Text.Encoding.UTF8.GetBytes(securityOptions.SecretKey)),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true
                };
                options.TokenValidationParameters = tokenValidationParams;
            })
            .AddPolicyScheme(SecurityConstants.SERVICEBRICKS_AUTHENTICATION_SCHEME, SecurityConstants.SERVICEBRICKS_AUTHENTICATION_SCHEME, options =>
            {
                // AI: Runs on each request
                options.ForwardDefaultSelector = context =>
                {
                    // AI: Filter by auth type
                    string authorization = context.Request.Headers[HeaderNames.Authorization];
                    if (!string.IsNullOrEmpty(authorization) && authorization.StartsWith("Bearer ", StringComparison.InvariantCultureIgnoreCase))
                        return JwtBearerDefaults.AuthenticationScheme;

                    // AI: Fallback check for cookie auth (with identity)
                    return IdentityConstants.ApplicationScheme;
                };
            });

            // AI: Add Authorization
            services.AddAuthorization(options =>
            {
                // AI: Add Built-in Security Policies
                options.AddPolicy(ServiceBricksConstants.SECURITY_POLICY_ADMIN, policy =>
                    policy
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .AddAuthenticationSchemes(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddAuthenticationSchemes(IdentityConstants.ApplicationScheme)
                    .RequireRole(ServiceBricksConstants.SECURITY_ROLE_ADMIN_NAME));

                options.AddPolicy(ServiceBricksConstants.SECURITY_POLICY_USER, policy =>
                    policy
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .AddAuthenticationSchemes(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddAuthenticationSchemes(IdentityConstants.ApplicationScheme)
                    .RequireRole(ServiceBricksConstants.SECURITY_ROLE_USER_NAME));
            });

            // AI: Add any miscellaneous services for the module
            services.AddScoped<IAuthenticationApiService, AuthenticationApiService>();

            // AI: Add API Controllers for each DTO in the module
            services.AddScoped<IUserAuditApiController, UserAuditApiController>();
            services.AddScoped<IUserApiController, UserApiController>();
            services.AddScoped<IUserClaimApiController, ApplicationUserClaimApiController>();
            services.AddScoped<IUserRoleApiController, UserRoleApiController>();
            services.AddScoped<IRoleApiController, RoleApiController>();
            services.AddScoped<IUserLoginApiController, UserLoginApiController>();
            services.AddScoped<IRoleClaimApiController, RoleClaimApiController>();
            services.AddScoped<IUserTokenApiController, UserTokenApiController>();
            services.AddScoped<IAuthenticationApiController, AuthenticationApiController>();

            // AI: Register business rules for the module
            SendConfirmEmailRule.Register(BusinessRuleRegistry.Instance);
            SendResetPasswordEmailRule.Register(BusinessRuleRegistry.Instance);
            UserConfirmEmailRule.Register(BusinessRuleRegistry.Instance);
            UserForgotPasswordRule.Register(BusinessRuleRegistry.Instance);
            UserInvalidPasswordRule.Register(BusinessRuleRegistry.Instance);
            UserLoginRule.Register(BusinessRuleRegistry.Instance);
            UserLogoutRule.Register(BusinessRuleRegistry.Instance);
            UserMFARule.Register(BusinessRuleRegistry.Instance);
            UserMfaVerifyRule.Register(BusinessRuleRegistry.Instance);
            UserPasswordChangeRule.Register(BusinessRuleRegistry.Instance);
            UserPasswordResetRule.Register(BusinessRuleRegistry.Instance);
            UserProfileChangeRule.Register(BusinessRuleRegistry.Instance);
            UserRegisterAdminRule.Register(BusinessRuleRegistry.Instance);
            UserRegisterRule.Register(BusinessRuleRegistry.Instance);
            UserResendConfirmationProcessRule.Register(BusinessRuleRegistry.Instance);

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
            services.AddScoped<IUserApiClient, UserApiClient>();
            services.AddScoped<IUserClaimApiClient, UserClaimApiClient>();
            services.AddScoped<IUserRoleApiClient, UserRoleApiClient>();
            services.AddScoped<IUserTokenApiClient, UserTokenApiClient>();
            services.AddScoped<IUserLoginApiClient, UserLoginApiClient>();
            services.AddScoped<IRoleApiClient, RoleApiClient>();
            services.AddScoped<IRoleClaimApiClient, RoleClaimApiClient>();
            services.AddScoped<IUserAuditApiClient, UserAuditApiClient>();
            services.AddScoped<IAuthenticationApiClient, AuthenticationApiClient>();
            return services;
        }
    }
}