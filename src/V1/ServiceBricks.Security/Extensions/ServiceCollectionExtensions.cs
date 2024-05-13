using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;

using System;

namespace ServiceBricks.Security
{
    /// <summary>
    /// IServiceCollection extensions for the Security Brick.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServiceBricksSecurity(this IServiceCollection services, IConfiguration configuration)
        {
            // Add to module registry
            ModuleRegistry.Instance.RegisterItem(typeof(SecurityModule), new SecurityModule());

            services.AddRouting(); // For LinkGenerator

            // Add Authentication
            services.Configure<SecurityTokenOptions>(configuration.GetSection(SecurityConstants.APPSETTING_SECURITY_TOKEN));
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
                // runs on each request
                options.ForwardDefaultSelector = context =>
                {
                    // filter by auth type
                    string authorization = context.Request.Headers[HeaderNames.Authorization];
                    if (!string.IsNullOrEmpty(authorization) && authorization.StartsWith("Bearer ", StringComparison.InvariantCultureIgnoreCase))
                        return JwtBearerDefaults.AuthenticationScheme;

                    // otherwise always check for cookie auth (with identity)
                    return IdentityConstants.ApplicationScheme;
                };
            });

            // Add Authorization
            services.AddAuthorization(options =>
            {
                //Add Built-in Security Policies
                options.AddPolicy(ServiceBricksConstants.SECURITY_POLICY_ADMIN, policy =>
                    policy
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .AddAuthenticationSchemes(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddAuthenticationSchemes(IdentityConstants.ApplicationScheme)
                    .RequireRole(SecurityConstants.ROLE_ADMIN_NAME));

                options.AddPolicy(ServiceBricksConstants.SECURITY_POLICY_USER, policy =>
                    policy
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .AddAuthenticationSchemes(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddAuthenticationSchemes(IdentityConstants.ApplicationScheme)
                    .RequireRole(SecurityConstants.ROLE_USER_NAME));
            });

            // Services
            services.AddScoped<IAuthenticationApiService, AuthenticationApiService>();

            // API Controllers
            services.AddScoped<IAuditUserApiController, AuditUserApiController>();
            services.AddScoped<IApplicationUserApiController, ApplicationUserApiController>();
            services.AddScoped<IApplicationUserClaimApiController, ApplicationUserClaimApiController>();
            services.AddScoped<IApplicationUserRoleApiController, ApplicationUserRoleApiController>();
            services.AddScoped<IApplicationRoleApiController, ApplicationRoleApiController>();
            services.AddScoped<IApplicationUserLoginApiController, ApplicationUserLoginApiController>();
            services.AddScoped<IApplicationRoleClaimApiController, ApplicationRoleClaimApiController>();
            services.AddScoped<IApplicationUserTokenApiController, ApplicationUserTokenApiController>();
            services.AddScoped<IAuthenticationApiController, AuthenticationApiController>();

            // Business Rules
            UserConfirmEmailRule.RegisterRule(BusinessRuleRegistry.Instance);
            UserForgotPasswordRule.RegisterRule(BusinessRuleRegistry.Instance);
            SendResetPasswordEmailRule.RegisterRule(BusinessRuleRegistry.Instance);
            UserInvalidPasswordRule.RegisterRule(BusinessRuleRegistry.Instance);
            UserLoginRule.RegisterRule(BusinessRuleRegistry.Instance);
            UserLogoutRule.RegisterRule(BusinessRuleRegistry.Instance);
            UserMFARule.RegisterRule(BusinessRuleRegistry.Instance);
            UserMfaVerifyRule.RegisterRule(BusinessRuleRegistry.Instance);
            UserPasswordChangeRule.RegisterRule(BusinessRuleRegistry.Instance);
            UserPasswordResetRule.RegisterRule(BusinessRuleRegistry.Instance);
            UserProfileChangeRule.RegisterRule(BusinessRuleRegistry.Instance);
            UserRegisterAdminRule.RegisterRule(BusinessRuleRegistry.Instance);
            UserRegisterRule.RegisterRule(BusinessRuleRegistry.Instance);
            SendConfirmEmailRule.RegisterRule(BusinessRuleRegistry.Instance);
            UserResendConfirmationProcessRule.RegisterRule(BusinessRuleRegistry.Instance);

            return services;
        }

        public static IServiceCollection AddServiceBricksSecurityClient(this IServiceCollection services, IConfiguration configuration)
        {
            // Clients
            services.AddScoped<IApplicationUserApiClient, ApplicationUserApiClient>();
            services.AddScoped<IApplicationUserClaimApiClient, ApplicationUserClaimApiClient>();
            services.AddScoped<IApplicationUserRoleApiClient, ApplicationUserRoleApiClient>();
            services.AddScoped<IApplicationUserTokenApiClient, ApplicationUserTokenApiClient>();
            services.AddScoped<IApplicationUserLoginApiClient, ApplicationUserLoginApiClient>();
            services.AddScoped<IApplicationRoleApiClient, ApplicationRoleApiClient>();
            services.AddScoped<IApplicationRoleClaimApiClient, ApplicationRoleClaimApiClient>();
            services.AddScoped<IAuditUserApiClient, AuditUserApiClient>();
            services.AddScoped<IAuthenticationApiClient, AuthenticationApiClient>();
            return services;
        }
    }
}