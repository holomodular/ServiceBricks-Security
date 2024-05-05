using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;

namespace ServiceBricks.Security.MongoDb
{
    /// <summary>
    /// IServiceCollection extensions for the Security Brick.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServiceBricksSecurityMongoDb(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddServiceBricksSecurityMongoDb(configuration, new Action<IdentityOptions>(options => new IdentityOptions()));
        }

        public static IServiceCollection AddServiceBricksSecurityMongoDb(this IServiceCollection services, IConfiguration configuration, Action<IdentityOptions> identityOptions)
        {
            // Add to module registry
            ModuleRegistry.Instance.RegisterItem(typeof(SecurityMongoDbModule), new SecurityMongoDbModule());

            // Register Identity
            services
                .AddIdentity<ApplicationIdentityUser, ApplicationIdentityRole>(identityOptions)
                .AddUserStore<ApplicationUserStore>()
                .AddRoleStore<ApplicationRoleStore>()
                .AddDefaultTokenProviders();

            // Add core
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

            // Add business rules
            DomainCreateUpdateDateRule<ApplicationUser>.RegisterRule(BusinessRuleRegistry.Instance);
            DomainQueryPropertyRenameRule<ApplicationUser>.RegisterRule(BusinessRuleRegistry.Instance, "StorageKey", "Id");

            DomainQueryPropertyRenameRule<ApplicationRoleClaim>.RegisterRule(BusinessRuleRegistry.Instance, "StorageKey", "Key");
            DomainQueryPropertyRenameRule<ApplicationRoleClaim>.RegisterRule(BusinessRuleRegistry.Instance, "RoleStorageKey", "RoleId");

            DomainQueryPropertyRenameRule<ApplicationRole>.RegisterRule(BusinessRuleRegistry.Instance, "StorageKey", "Id");

            DomainQueryPropertyRenameRule<ApplicationUserClaim>.RegisterRule(BusinessRuleRegistry.Instance, "StorageKey", "Key");
            DomainQueryPropertyRenameRule<ApplicationUserClaim>.RegisterRule(BusinessRuleRegistry.Instance, "UserStorageKey", "UserId");

            DomainQueryPropertyRenameRule<ApplicationUserLogin>.RegisterRule(BusinessRuleRegistry.Instance, "StorageKey", "Key");
            DomainQueryPropertyRenameRule<ApplicationUserLogin>.RegisterRule(BusinessRuleRegistry.Instance, "UserStorageKey", "UserId");

            DomainQueryPropertyRenameRule<ApplicationUserRole>.RegisterRule(BusinessRuleRegistry.Instance, "StorageKey", "Key");
            DomainQueryPropertyRenameRule<ApplicationUserRole>.RegisterRule(BusinessRuleRegistry.Instance, "RoleStorageKey", "RoleId");
            DomainQueryPropertyRenameRule<ApplicationUserRole>.RegisterRule(BusinessRuleRegistry.Instance, "UserStorageKey", "UserId");

            DomainQueryPropertyRenameRule<ApplicationUserToken>.RegisterRule(BusinessRuleRegistry.Instance, "StorageKey", "Key");
            DomainQueryPropertyRenameRule<ApplicationUserToken>.RegisterRule(BusinessRuleRegistry.Instance, "UserStorageKey", "UserId");

            DomainQueryPropertyRenameRule<AuditUser>.RegisterRule(BusinessRuleRegistry.Instance, "StorageKey", "Key");
            DomainQueryPropertyRenameRule<AuditUser>.RegisterRule(BusinessRuleRegistry.Instance, "UserStorageKey", "UserId");
            DomainCreateDateRule<AuditUser>.RegisterRule(BusinessRuleRegistry.Instance);

            return services;
        }
    }
}