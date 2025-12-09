using Microsoft.Extensions.DependencyInjection;

namespace ServiceBricks.Security.EntityFrameworkCore
{
    /// <summary>
    /// This rule is executed when the ServiceBricks module is added.
    /// </summary>
    public sealed class SecurityEntityFrameworkCoreModuleAddRule : BusinessRule
    {
        /// <summary>
        /// Register the rule
        /// </summary>
        public static void Register(IBusinessRuleRegistry registry)
        {
            registry.Register(
                typeof(ModuleAddEvent<SecurityEntityFrameworkCoreModule>),
                typeof(SecurityEntityFrameworkCoreModuleAddRule));
        }

        /// <summary>
        /// UnRegister the rule.
        /// </summary>
        public static void UnRegister(IBusinessRuleRegistry registry)
        {
            registry.UnRegister(
                typeof(ModuleAddEvent<SecurityEntityFrameworkCoreModule>),
                typeof(SecurityEntityFrameworkCoreModuleAddRule));
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
            var e = context.Object as ModuleAddEvent<SecurityEntityFrameworkCoreModule>;
            if (e == null || e.DomainObject == null || e.ServiceCollection == null)
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, "context"));
                return response;
            }

            // AI: Perform logic
            var services = e.ServiceCollection;
            //var configuration = e.Configuration;

            // AI: Add any miscellaneous services for the module

            // AI: Add API services for the module. Each DTO should have two registrations, one for the generic IApiService<> and one for the named interface
            services.AddScoped<IApiService<UserAuditDto>, UserAuditApiService>();
            services.AddScoped<IUserAuditApiService, UserAuditApiService>();

            services.AddScoped<IApiService<UserDto>, ApplicationUserApiService>();
            services.AddScoped<IUserApiService, ApplicationUserApiService>();

            services.AddScoped<IApiService<UserClaimDto>, ApplicationUserClaimApiService>();
            services.AddScoped<IUserClaimApiService, ApplicationUserClaimApiService>();

            services.AddScoped<IApiService<UserRoleDto>, ApplicationUserRoleApiService>();
            services.AddScoped<IUserRoleApiService, ApplicationUserRoleApiService>();

            services.AddScoped<IApiService<RoleDto>, ApplicationRoleApiService>();
            services.AddScoped<IRoleApiService, ApplicationRoleApiService>();

            services.AddScoped<IApiService<UserLoginDto>, ApplicationUserLoginApiService>();
            services.AddScoped<IUserLoginApiService, ApplicationUserLoginApiService>();

            services.AddScoped<IApiService<RoleClaimDto>, ApplicationRoleClaimApiService>();
            services.AddScoped<IRoleClaimApiService, ApplicationRoleClaimApiService>();

            services.AddScoped<IApiService<UserTokenDto>, ApplicationUserTokenApiService>();
            services.AddScoped<IUserTokenApiService, ApplicationUserTokenApiService>();

            services.AddScoped<IUserManagerService, UserManagerService>();

            // AI: Register mappings
            ApplicationRoleClaimMappingProfile.Register(MapperRegistry.Instance);
            ApplicationRoleMappingProfile.Register(MapperRegistry.Instance);
            ApplicationUserClaimMappingProfile.Register(MapperRegistry.Instance);
            ApplicationUserLoginMappingProfile.Register(MapperRegistry.Instance);
            ApplicationUserMappingProfile.Register(MapperRegistry.Instance);
            ApplicationUserRoleMappingProfile.Register(MapperRegistry.Instance);
            ApplicationUserTokenMappingProfile.Register(MapperRegistry.Instance);
            UserAuditMappingProfile.Register(MapperRegistry.Instance);

            // AI: Register business rules for the module
            DomainCreateUpdateDateRule<ApplicationUser>.Register(BusinessRuleRegistry.Instance);
            DomainQueryPropertyRenameRule<ApplicationUser>.Register(BusinessRuleRegistry.Instance, "StorageKey", "Id");

            DomainCreateDateRule<UserAudit>.Register(BusinessRuleRegistry.Instance);
            DomainQueryPropertyRenameRule<UserAudit>.Register(BusinessRuleRegistry.Instance, "StorageKey", "Key");
            DomainQueryPropertyRenameRule<UserAudit>.Register(BusinessRuleRegistry.Instance, "UserStorageKey", "UserId");

            DomainQueryPropertyRenameRule<ApplicationUserClaim>.Register(BusinessRuleRegistry.Instance, "UserStorageKey", "UserId");
            DomainQueryPropertyRenameRule<ApplicationUserClaim>.Register(BusinessRuleRegistry.Instance, "RoleStorageKey", "RoleId");
            DomainQueryPropertyRenameRule<ApplicationUserClaim>.Register(BusinessRuleRegistry.Instance, "StorageKey", "Id");

            ApplicationUserLoginQueryRule.Register(BusinessRuleRegistry.Instance);
            DomainQueryPropertyRenameRule<ApplicationUserLogin>.Register(BusinessRuleRegistry.Instance, "UserStorageKey", "UserId");
            DomainQueryPropertyRenameRule<ApplicationUserLogin>.Register(BusinessRuleRegistry.Instance, "RoleStorageKey", "RoleId");

            ApplicationUserRoleQueryRule.Register(BusinessRuleRegistry.Instance);
            DomainQueryPropertyRenameRule<ApplicationUserRole>.Register(BusinessRuleRegistry.Instance, "UserStorageKey", "UserId");
            DomainQueryPropertyRenameRule<ApplicationUserRole>.Register(BusinessRuleRegistry.Instance, "RoleStorageKey", "RoleId");

            ApplicationUserTokenQueryRule.Register(BusinessRuleRegistry.Instance);
            DomainQueryPropertyRenameRule<ApplicationUserToken>.Register(BusinessRuleRegistry.Instance, "UserStorageKey", "UserId");
            DomainQueryPropertyRenameRule<ApplicationUserToken>.Register(BusinessRuleRegistry.Instance, "RoleStorageKey", "RoleId");

            DomainQueryPropertyRenameRule<ApplicationRoleClaim>.Register(BusinessRuleRegistry.Instance, "UserStorageKey", "UserId");
            DomainQueryPropertyRenameRule<ApplicationRoleClaim>.Register(BusinessRuleRegistry.Instance, "RoleStorageKey", "RoleId");
            DomainQueryPropertyRenameRule<ApplicationRoleClaim>.Register(BusinessRuleRegistry.Instance, "StorageKey", "Id");

            DomainQueryPropertyRenameRule<ApplicationRole>.Register(BusinessRuleRegistry.Instance, "StorageKey", "Id");

            return response;
        }
    }
}