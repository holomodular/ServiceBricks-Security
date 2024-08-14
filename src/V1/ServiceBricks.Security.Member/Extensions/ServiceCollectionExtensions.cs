using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;

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
            return AddServiceBricksSecurityMember(services, configuration, new Action<IdentityOptions>(options => new IdentityOptions()));
        }

        /// <summary>
        /// Add the ServiceBricks Security Member module to the IServiceCollection.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <param name="identityOptions"></param>
        /// <returns></returns>
        public static IServiceCollection AddServiceBricksSecurityMember(this IServiceCollection services, IConfiguration configuration, Action<IdentityOptions> identityOptions)
        {
            // AI: Add the module to the ModuleRegistry
            ModuleRegistry.Instance.RegisterItem(typeof(SecurityMemberModule), new SecurityMemberModule());

            // AI: Add Authentication
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
                // AI: Runs on each request
                options.ForwardDefaultSelector = context =>
                {
                    // AI: Filter by auth type
                    string authorization = context.Request.Headers[HeaderNames.Authorization];
                    if (!string.IsNullOrEmpty(authorization) && authorization.StartsWith("Bearer ", StringComparison.InvariantCultureIgnoreCase))
                        return JwtBearerDefaults.AuthenticationScheme;

                    // AI: Otherwise always check for cookie auth. Note, since identity is not added, use cookieauth scheme instead of identity.
                    return CookieAuthenticationDefaults.AuthenticationScheme;
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