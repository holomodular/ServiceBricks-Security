using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using ServiceBricks.Storage.EntityFrameworkCore;

namespace ServiceBricks.Security.EntityFrameworkCore
{
    /// <summary>
    /// IServiceCollection extensions for the Security Brick.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServiceBricksSecurityEntityFrameworkCore(this IServiceCollection services, IConfiguration configuration)
        {
            // Add to module registry
            ModuleRegistry.Instance.RegisterItem(typeof(SecurityEntityFrameworkCoreModule), new SecurityEntityFrameworkCoreModule());

            // Add Core
            services.AddServiceBricksSecurity(configuration);

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

            // Register Business rules
            DomainCreateUpdateDateRule<ApplicationUser>.RegisterRule(BusinessRuleRegistry.Instance);
            DomainQueryPropertyRenameRule<ApplicationUser>.RegisterRule(BusinessRuleRegistry.Instance, "StorageKey", "Id");

            DomainCreateDateRule<AuditUser>.RegisterRule(BusinessRuleRegistry.Instance);
            DomainQueryPropertyRenameRule<AuditUser>.RegisterRule(BusinessRuleRegistry.Instance, "StorageKey", "Key");

            DomainQueryPropertyRenameRule<ApplicationUserClaim>.RegisterRule(BusinessRuleRegistry.Instance, "UserStorageKey", "UserId");
            DomainQueryPropertyRenameRule<ApplicationUserClaim>.RegisterRule(BusinessRuleRegistry.Instance, "RoleStorageKey", "RoleId");
            DomainQueryPropertyRenameRule<ApplicationUserClaim>.RegisterRule(BusinessRuleRegistry.Instance, "StorageKey", "Id");

            DomainQueryPropertyRenameRule<ApplicationUserLogin>.RegisterRule(BusinessRuleRegistry.Instance, "UserStorageKey", "UserId");
            DomainQueryPropertyRenameRule<ApplicationUserLogin>.RegisterRule(BusinessRuleRegistry.Instance, "RoleStorageKey", "RoleId");

            DomainQueryPropertyRenameRule<ApplicationUserRole>.RegisterRule(BusinessRuleRegistry.Instance, "UserStorageKey", "UserId");
            DomainQueryPropertyRenameRule<ApplicationUserRole>.RegisterRule(BusinessRuleRegistry.Instance, "RoleStorageKey", "RoleId");

            DomainQueryPropertyRenameRule<ApplicationUserToken>.RegisterRule(BusinessRuleRegistry.Instance, "UserStorageKey", "UserId");
            DomainQueryPropertyRenameRule<ApplicationUserToken>.RegisterRule(BusinessRuleRegistry.Instance, "RoleStorageKey", "RoleId");

            DomainQueryPropertyRenameRule<ApplicationRoleClaim>.RegisterRule(BusinessRuleRegistry.Instance, "UserStorageKey", "UserId");
            DomainQueryPropertyRenameRule<ApplicationRoleClaim>.RegisterRule(BusinessRuleRegistry.Instance, "RoleStorageKey", "RoleId");
            DomainQueryPropertyRenameRule<ApplicationRoleClaim>.RegisterRule(BusinessRuleRegistry.Instance, "StorageKey", "Id");

            DomainQueryPropertyRenameRule<AuditUser>.RegisterRule(BusinessRuleRegistry.Instance, "UserStorageKey", "UserId");
            DomainQueryPropertyRenameRule<AuditUser>.RegisterRule(BusinessRuleRegistry.Instance, "RoleStorageKey", "RoleId");

            DomainQueryPropertyRenameRule<ApplicationRole>.RegisterRule(BusinessRuleRegistry.Instance, "StorageKey", "Id");

            ApplicationUserLoginQueryRule.RegisterRule(BusinessRuleRegistry.Instance);
            ApplicationUserTokenQueryRule.RegisterRule(BusinessRuleRegistry.Instance);
            ApplicationUserRoleQueryRule.RegisterRule(BusinessRuleRegistry.Instance);

            return services;
        }
    }
}