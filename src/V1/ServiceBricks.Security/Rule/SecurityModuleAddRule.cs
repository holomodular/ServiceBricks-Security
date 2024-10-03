using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;

namespace ServiceBricks.Security
{
    /// <summary>
    /// This rule is executed when the ServiceBricks module is added.
    /// </summary>
    public sealed class SecurityModuleAddRule : BusinessRule
    {
        /// <summary>
        /// Register the rule
        /// </summary>
        public static void Register(IBusinessRuleRegistry registry)
        {
            registry.Register(
                typeof(ModuleAddEvent<SecurityModule>),
                typeof(SecurityModuleAddRule));
        }

        /// <summary>
        /// UnRegister the rule.
        /// </summary>
        public static void UnRegister(IBusinessRuleRegistry registry)
        {
            registry.UnRegister(
                typeof(ModuleAddEvent<SecurityModule>),
                typeof(SecurityModuleAddRule));
        }

        /// <summary>
        /// Execute the business rule.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override IResponse ExecuteRule(IBusinessRuleContext context)
        {
            var response = new Response();

            // AI: Make sure the context object is the correct type
            if (context == null || context.Object == null)
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, "context"));
                return response;
            }
            var e = context.Object as ModuleAddEvent<SecurityModule>;
            if (e == null || e.DomainObject == null || e.ServiceCollection == null)
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, "context"));
                return response;
            }

            // AI: Perform logic
            var services = e.ServiceCollection;
            var configuration = e.Configuration;

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
            services.AddScoped<IApiController<UserAuditDto>, UserAuditApiController>();
            services.AddScoped<IUserAuditApiController, UserAuditApiController>();

            services.AddScoped<IApiController<UserDto>, UserApiController>();
            services.AddScoped<IUserApiController, UserApiController>();

            services.AddScoped<IApiController<UserClaimDto>, UserClaimApiController>();
            services.AddScoped<IUserClaimApiController, UserClaimApiController>();

            services.AddScoped<IApiController<UserRoleDto>, UserRoleApiController>();
            services.AddScoped<IUserRoleApiController, UserRoleApiController>();

            services.AddScoped<IApiController<RoleDto>, RoleApiController>();
            services.AddScoped<IRoleApiController, RoleApiController>();

            services.AddScoped<IApiController<UserLoginDto>, UserLoginApiController>();
            services.AddScoped<IUserLoginApiController, UserLoginApiController>();

            services.AddScoped<IApiController<RoleClaimDto>, RoleClaimApiController>();
            services.AddScoped<IRoleClaimApiController, RoleClaimApiController>();

            services.AddScoped<IApiController<UserTokenDto>, UserTokenApiController>();
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

            return response;
        }
    }
}