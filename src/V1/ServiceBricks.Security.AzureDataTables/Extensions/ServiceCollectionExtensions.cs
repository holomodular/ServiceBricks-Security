using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace ServiceBricks.Security.AzureDataTables
{
    /// <summary>
    /// IServiceCollection extensions for the Security Brick.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServiceBricksSecurityAzureDataTables(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddServiceBricksSecurityAzureDataTables(configuration, new Action<IdentityOptions>(options => new IdentityOptions()));
        }

        public static IServiceCollection AddServiceBricksSecurityAzureDataTables(this IServiceCollection services, IConfiguration configuration, Action<IdentityOptions> identityOptions)
        {
            // Add to module registry
            ModuleRegistry.Instance.RegisterItem(typeof(SecurityAzureDataTablesModule), new SecurityAzureDataTablesModule());

            // Register Identity
            services
                .AddIdentity<ApplicationUser, ApplicationRole>(identityOptions)
                .AddUserStore<ApplicationUserStore>()
                .AddRoleStore<ApplicationRoleStore>()
                .AddDefaultTokenProviders();

            // Add Core
            services.AddServiceBricksSecurity(configuration);

            // Storage Services
            services.AddScoped<IStorageRepository<ApplicationRole>, SecurityStorageRepository<ApplicationRole>>();
            services.AddScoped<IStorageRepository<ApplicationRoleClaim>, SecurityStorageRepository<ApplicationRoleClaim>>();
            services.AddScoped<IStorageRepository<ApplicationUser>, SecurityStorageRepository<ApplicationUser>>();
            services.AddScoped<IStorageRepository<ApplicationUserClaim>, SecurityStorageRepository<ApplicationUserClaim>>();
            services.AddScoped<IStorageRepository<ApplicationUserLogin>, SecurityStorageRepository<ApplicationUserLogin>>();
            services.AddScoped<IStorageRepository<ApplicationUserRole>, SecurityStorageRepository<ApplicationUserRole>>();
            services.AddScoped<IStorageRepository<ApplicationUserToken>, SecurityStorageRepository<ApplicationUserToken>>();
            services.AddScoped<IAuditUserStorageRepository, AuditUserStorageRepository>();
            services.AddScoped<IStorageRepository<AuditUser>, AuditUserStorageRepository>();

            // API Services
            services.AddScoped<IApiService<AuditUserDto>, AuditUserApiService>();
            services.AddScoped<IAuditUserApiService, AuditUserApiService>();

            services.AddScoped<IApiService<ApplicationUserDto>, ApplicationUserApiService>();
            services.AddScoped<IApplicationUserApiService, ApplicationUserApiService>();

            services.AddScoped<IApiService<ApplicationUserClaimDto>, ApplicationUserClaimApiService>();
            services.AddScoped<IApplicationUserClaimApiService, ApplicationUserClaimApiService>();

            services.AddScoped<IApiService<ApplicationUserRoleDto>, ApplicationUserRoleApiService>();
            services.AddScoped<IApplicationUserRoleApiService, ApplicationUserRoleApiService>();

            services.AddScoped<IApiService<ApplicationRoleDto>, ApplicationRoleApiService>();
            services.AddScoped<IApplicationRoleApiService, ApplicationRoleApiService>();

            services.AddScoped<IApiService<ApplicationUserLoginDto>, ApplicationUserLoginApiService>();
            services.AddScoped<IApplicationUserLoginApiService, ApplicationUserLoginApiService>();

            services.AddScoped<IApiService<ApplicationRoleClaimDto>, ApplicationRoleClaimApiService>();
            services.AddScoped<IApplicationRoleClaimApiService, ApplicationRoleClaimApiService>();

            services.AddScoped<IApiService<ApplicationUserTokenDto>, ApplicationUserTokenApiService>();
            services.AddScoped<IApplicationUserTokenApiService, ApplicationUserTokenApiService>();

            services.AddScoped<IUserManagerService, UserManagerService>();

            // Add Business Rules
            DomainCreateUpdateDateRule<ApplicationUser>.RegisterRule(BusinessRuleRegistry.Instance);
            DomainQueryPropertyRenameRule<ApplicationUser>.RegisterRule(BusinessRuleRegistry.Instance, "UserStorageKey", "PartitionKey");
            DomainQueryPropertyRenameRule<ApplicationUser>.RegisterRule(BusinessRuleRegistry.Instance, "RoleStorageKey", "PartitionKey");
            ApplicationUserCreateRule.RegisterRule(BusinessRuleRegistry.Instance);
            DomainQueryPropertyRenameRule<ApplicationUser>.RegisterRule(BusinessRuleRegistry.Instance, "StorageKey", "PartitionKey");

            DomainCreateDateRule<AuditUser>.RegisterRule(BusinessRuleRegistry.Instance);
            AuditUserQueryRule.RegisterRule(BusinessRuleRegistry.Instance);
            DomainQueryPropertyRenameRule<AuditUser>.RegisterRule(BusinessRuleRegistry.Instance, "UserStorageKey", "PartitionKey");
            DomainQueryPropertyRenameRule<AuditUser>.RegisterRule(BusinessRuleRegistry.Instance, "RoleStorageKey", "PartitionKey");
            AuditUserCreateRule.RegisterRule(BusinessRuleRegistry.Instance);

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