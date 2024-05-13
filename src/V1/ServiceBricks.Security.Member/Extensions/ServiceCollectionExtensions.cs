using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using System;

namespace ServiceBricks.Security.Member
{
    /// <summary>
    /// IServiceCollection extensions for the Security Brick.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServiceBricksSecurityMember(this IServiceCollection services, IConfiguration configuration)
        {
            return AddServiceBricksSecurityMember(services, configuration, new Action<IdentityOptions>(options => new IdentityOptions()));
        }

        public static IServiceCollection AddServiceBricksSecurityMember(this IServiceCollection services, IConfiguration configuration, Action<IdentityOptions> identityOptions)
        {
            // Add to module registry
            ModuleRegistry.Instance.RegisterItem(typeof(SecurityMemberModule), new SecurityMemberModule());

            // Add Authentication
            services.Configure<SecurityTokenOptions>(configuration.GetSection(SecurityMemberConstants.APPSETTING_SECURITY_TOKEN));
            var securityOptions = services.BuildServiceProvider().GetService<IOptions<SecurityTokenOptions>>().Value;
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = SecurityMemberConstants.SERVICEBRICKS_AUTHENTICATION_SCHEME;
                options.DefaultScheme = SecurityMemberConstants.SERVICEBRICKS_AUTHENTICATION_SCHEME;
                options.DefaultChallengeScheme = SecurityMemberConstants.SERVICEBRICKS_AUTHENTICATION_SCHEME;
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
            .AddPolicyScheme(SecurityMemberConstants.SERVICEBRICKS_AUTHENTICATION_SCHEME, SecurityMemberConstants.SERVICEBRICKS_AUTHENTICATION_SCHEME, options =>
            {
                // runs on each request
                options.ForwardDefaultSelector = context =>
                {
                    // filter by auth type
                    string authorization = context.Request.Headers[HeaderNames.Authorization];
                    if (!string.IsNullOrEmpty(authorization) && authorization.StartsWith("Bearer ", StringComparison.InvariantCultureIgnoreCase))
                        return JwtBearerDefaults.AuthenticationScheme;

                    // otherwise always check for cookie auth
                    return CookieAuthenticationDefaults.AuthenticationScheme;
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
                    .RequireRole(SecurityMemberConstants.ROLE_ADMIN_NAME));

                options.AddPolicy(ServiceBricksConstants.SECURITY_POLICY_USER, policy =>
                    policy
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .AddAuthenticationSchemes(CookieAuthenticationDefaults.AuthenticationScheme)
                    .RequireRole(SecurityMemberConstants.ROLE_USER_NAME));
            });

            return services;
        }
    }
}