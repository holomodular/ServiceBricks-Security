using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace ServiceBricks.Security.AzureDataTables
{
    /// <summary>
    /// This rule is executed when the ServiceBricks module is added.
    /// </summary>
    public sealed class SecurityAzureDataTablesModuleAddRule : BusinessRule
    {
        /// <summary>
        /// Register the rule
        /// </summary>
        public static void Register(IBusinessRuleRegistry registry)
        {
            registry.Register(
                typeof(ModuleAddEvent<SecurityAzureDataTablesModule>),
                typeof(SecurityAzureDataTablesModuleAddRule));
        }

        /// <summary>
        /// UnRegister the rule.
        /// </summary>
        public static void UnRegister(IBusinessRuleRegistry registry)
        {
            registry.UnRegister(
                typeof(ModuleAddEvent<SecurityAzureDataTablesModule>),
                typeof(SecurityAzureDataTablesModuleAddRule));
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
            var e = context.Object as ModuleAddEvent<SecurityAzureDataTablesModule>;
            if (e == null || e.DomainObject == null || e.ServiceCollection == null)
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, "context"));
                return response;
            }

            // AI: Perform logic
            var services = e.ServiceCollection;
            //var configuration = e.Configuration;

            // AI: Register Identity
            services
                .AddIdentity<ApplicationUser, ApplicationRole>(SecurityModule.Instance.IdentityOptions)
                .AddUserStore<ApplicationUserStore>()
                .AddRoleStore<ApplicationRoleStore>()
                .AddDefaultTokenProviders();

            // AI: Add storage services for the module. Each domain object should have its own storage repository
            services.AddScoped<IStorageRepository<ApplicationRole>, SecurityStorageRepository<ApplicationRole>>();
            services.AddScoped<IStorageRepository<ApplicationRoleClaim>, SecurityStorageRepository<ApplicationRoleClaim>>();
            services.AddScoped<IStorageRepository<ApplicationUser>, SecurityStorageRepository<ApplicationUser>>();
            services.AddScoped<IStorageRepository<ApplicationUserClaim>, SecurityStorageRepository<ApplicationUserClaim>>();
            services.AddScoped<IStorageRepository<ApplicationUserLogin>, SecurityStorageRepository<ApplicationUserLogin>>();
            services.AddScoped<IStorageRepository<ApplicationUserRole>, SecurityStorageRepository<ApplicationUserRole>>();
            services.AddScoped<IStorageRepository<ApplicationUserToken>, SecurityStorageRepository<ApplicationUserToken>>();
            services.AddScoped<IStorageRepository<UserAudit>, SecurityStorageRepository<UserAudit>>();

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
            DomainQueryPropertyRenameRule<ApplicationUser>.Register(BusinessRuleRegistry.Instance, "UserStorageKey", "PartitionKey");
            ApplicationUserCreateRule.Register(BusinessRuleRegistry.Instance);
            DomainQueryPropertyRenameRule<ApplicationUser>.Register(BusinessRuleRegistry.Instance, "StorageKey", "PartitionKey");

            DomainCreateDateRule<UserAudit>.Register(BusinessRuleRegistry.Instance);
            UserAuditQueryRule.Register(BusinessRuleRegistry.Instance);
            DomainQueryPropertyRenameRule<UserAudit>.Register(BusinessRuleRegistry.Instance, "UserStorageKey", "UserId");
            UserAuditCreateRule.Register(BusinessRuleRegistry.Instance);

            ApplicationUserClaimCreateRule.Register(BusinessRuleRegistry.Instance);
            DomainQueryPropertyRenameRule<ApplicationUserClaim>.Register(BusinessRuleRegistry.Instance, "UserStorageKey", "PartitionKey");
            DomainQueryPropertyRenameRule<ApplicationUserClaim>.Register(BusinessRuleRegistry.Instance, "RoleStorageKey", "PartitionKey");

            ApplicationUserLoginCreateRule.Register(BusinessRuleRegistry.Instance);
            ApplicationUserRoleCreateRule.Register(BusinessRuleRegistry.Instance);
            ApplicationUserTokenCreateRule.Register(BusinessRuleRegistry.Instance);

            ApplicationRoleClaimCreateRule.Register(BusinessRuleRegistry.Instance);

            ApplicationRoleCreateRule.Register(BusinessRuleRegistry.Instance);
            DomainQueryPropertyRenameRule<ApplicationRole>.Register(BusinessRuleRegistry.Instance, "UserStorageKey", "PartitionKey");
            DomainQueryPropertyRenameRule<ApplicationRole>.Register(BusinessRuleRegistry.Instance, "RoleStorageKey", "PartitionKey");
            DomainQueryPropertyRenameRule<ApplicationRole>.Register(BusinessRuleRegistry.Instance, "StorageKey", "PartitionKey");

            ApplicationRoleClaimQueryRule.Register(BusinessRuleRegistry.Instance);
            DomainQueryPropertyRenameRule<ApplicationRoleClaim>.Register(BusinessRuleRegistry.Instance, "UserStorageKey", "PartitionKey");
            DomainQueryPropertyRenameRule<ApplicationRoleClaim>.Register(BusinessRuleRegistry.Instance, "RoleStorageKey", "PartitionKey");

            ApplicationUserClaimQueryRule.Register(BusinessRuleRegistry.Instance);
            ApplicationUserLoginQueryRule.Register(BusinessRuleRegistry.Instance);

            ApplicationUserTokenQueryRule.Register(BusinessRuleRegistry.Instance);
            DomainQueryPropertyRenameRule<ApplicationUserToken>.Register(BusinessRuleRegistry.Instance, "UserStorageKey", "PartitionKey");
            DomainQueryPropertyRenameRule<ApplicationUserToken>.Register(BusinessRuleRegistry.Instance, "RoleStorageKey", "PartitionKey");

            ApplicationUserRoleQueryRule.Register(BusinessRuleRegistry.Instance);

            return response;
        }
    }
}