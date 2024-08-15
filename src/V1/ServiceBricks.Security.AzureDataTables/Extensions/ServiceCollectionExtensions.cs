using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ServiceBricks.Security.AzureDataTables
{
    /// <summary>
    /// Extensions for the ServiceBricks.Security.AzureDataTables module.
    /// </summary>
    public static partial class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add the ServiceBricks.Security.AzureDataTables module to the service collection.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddServiceBricksSecurityAzureDataTables(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddServiceBricksSecurityAzureDataTables(configuration, new Action<IdentityOptions>(options => new IdentityOptions()));
        }

        /// <summary>
        /// Add the ServiceBricks.Security.AzureDataTables module to the service collection.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <param name="identityOptions"></param>
        /// <returns></returns>
        public static IServiceCollection AddServiceBricksSecurityAzureDataTables(this IServiceCollection services, IConfiguration configuration, Action<IdentityOptions> identityOptions)
        {
            // AI: Add the module to the ModuleRegistry
            ModuleRegistry.Instance.RegisterItem(typeof(SecurityAzureDataTablesModule), new SecurityAzureDataTablesModule());

            // AI: Register Identity
            services
                .AddIdentity<ApplicationUser, ApplicationRole>(identityOptions)
                .AddUserStore<ApplicationUserStore>()
                .AddRoleStore<ApplicationRoleStore>()
                .AddDefaultTokenProviders();

            // AI: Add parent module
            services.AddServiceBricksSecurity(configuration);

            // AI: Add storage services for the module. Each domain object should have its own storage repository
            services.AddScoped<IStorageRepository<ApplicationRole>, SecurityStorageRepository<ApplicationRole>>();
            services.AddScoped<IStorageRepository<ApplicationRoleClaim>, SecurityStorageRepository<ApplicationRoleClaim>>();
            services.AddScoped<IStorageRepository<ApplicationUser>, SecurityStorageRepository<ApplicationUser>>();
            services.AddScoped<IStorageRepository<ApplicationUserClaim>, SecurityStorageRepository<ApplicationUserClaim>>();
            services.AddScoped<IStorageRepository<ApplicationUserLogin>, SecurityStorageRepository<ApplicationUserLogin>>();
            services.AddScoped<IStorageRepository<ApplicationUserRole>, SecurityStorageRepository<ApplicationUserRole>>();
            services.AddScoped<IStorageRepository<ApplicationUserToken>, SecurityStorageRepository<ApplicationUserToken>>();
            services.AddScoped<IUserAuditStorageRepository, UserAuditStorageRepository>();
            services.AddScoped<IStorageRepository<UserAudit>, UserAuditStorageRepository>();

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

            // AI: Register business rules for the module
            DomainCreateUpdateDateRule<ApplicationUser>.RegisterRule(BusinessRuleRegistry.Instance);
            DomainQueryPropertyRenameRule<ApplicationUser>.RegisterRule(BusinessRuleRegistry.Instance, "UserStorageKey", "PartitionKey");
            DomainQueryPropertyRenameRule<ApplicationUser>.RegisterRule(BusinessRuleRegistry.Instance, "RoleStorageKey", "PartitionKey");
            ApplicationUserCreateRule.RegisterRule(BusinessRuleRegistry.Instance);
            DomainQueryPropertyRenameRule<ApplicationUser>.RegisterRule(BusinessRuleRegistry.Instance, "StorageKey", "PartitionKey");

            DomainCreateDateRule<UserAudit>.RegisterRule(BusinessRuleRegistry.Instance);
            UserAuditQueryRule.RegisterRule(BusinessRuleRegistry.Instance);
            DomainQueryPropertyRenameRule<UserAudit>.RegisterRule(BusinessRuleRegistry.Instance, "UserStorageKey", "PartitionKey");
            DomainQueryPropertyRenameRule<UserAudit>.RegisterRule(BusinessRuleRegistry.Instance, "RoleStorageKey", "PartitionKey");
            UserAuditCreateRule.RegisterRule(BusinessRuleRegistry.Instance);

            ApplicationUserClaimCreateRule.RegisterRule(BusinessRuleRegistry.Instance);
            DomainQueryPropertyRenameRule<ApplicationUserClaim>.RegisterRule(BusinessRuleRegistry.Instance, "UserStorageKey", "PartitionKey");
            DomainQueryPropertyRenameRule<ApplicationUserClaim>.RegisterRule(BusinessRuleRegistry.Instance, "RoleStorageKey", "PartitionKey");

            ApplicationUserLoginCreateRule.RegisterRule(BusinessRuleRegistry.Instance);
            ApplicationUserRoleCreateRule.RegisterRule(BusinessRuleRegistry.Instance);
            ApplicationUserTokenCreateRule.RegisterRule(BusinessRuleRegistry.Instance);

            ApplicationRoleClaimCreateRule.RegisterRule(BusinessRuleRegistry.Instance);

            ApplicationRoleCreateRule.RegisterRule(BusinessRuleRegistry.Instance);
            DomainQueryPropertyRenameRule<ApplicationRole>.RegisterRule(BusinessRuleRegistry.Instance, "UserStorageKey", "PartitionKey");
            DomainQueryPropertyRenameRule<ApplicationRole>.RegisterRule(BusinessRuleRegistry.Instance, "RoleStorageKey", "PartitionKey");
            DomainQueryPropertyRenameRule<ApplicationRole>.RegisterRule(BusinessRuleRegistry.Instance, "StorageKey", "PartitionKey");

            ApplicationRoleClaimQueryRule.RegisterRule(BusinessRuleRegistry.Instance);
            DomainQueryPropertyRenameRule<ApplicationRoleClaim>.RegisterRule(BusinessRuleRegistry.Instance, "UserStorageKey", "PartitionKey");
            DomainQueryPropertyRenameRule<ApplicationRoleClaim>.RegisterRule(BusinessRuleRegistry.Instance, "RoleStorageKey", "PartitionKey");

            ApplicationUserClaimQueryRule.RegisterRule(BusinessRuleRegistry.Instance);
            ApplicationUserLoginQueryRule.RegisterRule(BusinessRuleRegistry.Instance);

            ApplicationUserTokenQueryRule.RegisterRule(BusinessRuleRegistry.Instance);
            DomainQueryPropertyRenameRule<ApplicationUserToken>.RegisterRule(BusinessRuleRegistry.Instance, "UserStorageKey", "PartitionKey");
            DomainQueryPropertyRenameRule<ApplicationUserToken>.RegisterRule(BusinessRuleRegistry.Instance, "RoleStorageKey", "PartitionKey");

            ApplicationUserRoleQueryRule.RegisterRule(BusinessRuleRegistry.Instance);

            return services;
        }
    }
}