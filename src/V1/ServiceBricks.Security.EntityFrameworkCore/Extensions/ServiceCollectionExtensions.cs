﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ServiceBricks.Security.EntityFrameworkCore
{
    /// <summary>
    /// Add ServiceBricks Security EntityFrameworkCore services to the application.
    /// </summary>
    public static partial class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add ServiceBricks Security EntityFrameworkCore services to the application.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddServiceBricksSecurityEntityFrameworkCore(this IServiceCollection services, IConfiguration configuration)
        {
            // AI: Add the module to the ModuleRegistry
            ModuleRegistry.Instance.RegisterItem(typeof(SecurityEntityFrameworkCoreModule), new SecurityEntityFrameworkCoreModule());

            // AI: Add the parent module
            services.AddServiceBricksSecurity(configuration);

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

            // AI: Register business rules for the module
            DomainCreateUpdateDateRule<ApplicationUser>.RegisterRule(BusinessRuleRegistry.Instance);
            DomainQueryPropertyRenameRule<ApplicationUser>.RegisterRule(BusinessRuleRegistry.Instance, "StorageKey", "Id");

            DomainCreateDateRule<UserAudit>.RegisterRule(BusinessRuleRegistry.Instance);
            DomainQueryPropertyRenameRule<UserAudit>.RegisterRule(BusinessRuleRegistry.Instance, "StorageKey", "Key");

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

            DomainQueryPropertyRenameRule<UserAudit>.RegisterRule(BusinessRuleRegistry.Instance, "UserStorageKey", "UserId");
            DomainQueryPropertyRenameRule<UserAudit>.RegisterRule(BusinessRuleRegistry.Instance, "RoleStorageKey", "RoleId");

            DomainQueryPropertyRenameRule<ApplicationRole>.RegisterRule(BusinessRuleRegistry.Instance, "StorageKey", "Id");

            ApplicationUserLoginQueryRule.RegisterRule(BusinessRuleRegistry.Instance);
            ApplicationUserTokenQueryRule.RegisterRule(BusinessRuleRegistry.Instance);
            ApplicationUserRoleQueryRule.RegisterRule(BusinessRuleRegistry.Instance);

            return services;
        }
    }
}