using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using ServiceBricks.Storage.EntityFrameworkCore;
using ServiceBricks.Security.EntityFrameworkCore;

namespace ServiceBricks.Security.Cosmos
{
    /// <summary>
    /// IServiceCollection extensions for the Security Brick.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServiceBricksSecurityCosmos(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddServiceBricksSecurityCosmos(configuration, new Action<IdentityOptions>(options => new IdentityOptions()));
        }

        public static IServiceCollection AddServiceBricksSecurityCosmos(this IServiceCollection services, IConfiguration configuration, Action<IdentityOptions> identityOptions)
        {
            // Add to module registry
            ModuleRegistry.Instance.RegisterItem(typeof(SecurityCosmosModule), new SecurityCosmosModule());

            //Register Database
            var builder = new DbContextOptionsBuilder<SecurityCosmosContext>();
            string connectionString = configuration.GetCosmosConnectionString(
                SecurityCosmosConstants.APPSETTING_CONNECTION);
            string database = configuration.GetCosmosDatabase(
                SecurityCosmosConstants.APPSETTING_DATABASE);
            builder.UseCosmos(connectionString, database);
            services.Configure<DbContextOptions<SecurityCosmosContext>>(o => { o = builder.Options; });
            services.AddSingleton<DbContextOptions<SecurityCosmosContext>>(builder.Options);
            services.AddDbContext<SecurityCosmosContext>(c => { c = builder; }, ServiceLifetime.Scoped);

            // Register Identity
            services
                .AddIdentity<ApplicationUser, ApplicationRole>(identityOptions)
                .AddEntityFrameworkStores<SecurityCosmosContext>()
                .AddUserStore<ApplicationUserStore>()
                .AddRoleStore<ApplicationRoleStore>()
                .AddDefaultTokenProviders();

            // Add Core service
            services.AddServiceBricksSecurity(configuration);

            // Storage Services
            services.AddScoped<IStorageRepository<Cosmos.ApplicationRole>, SecurityStorageRepository<Cosmos.ApplicationRole>>();
            services.AddScoped<IStorageRepository<Cosmos.ApplicationRoleClaim>, SecurityStorageRepository<Cosmos.ApplicationRoleClaim>>();
            services.AddScoped<IStorageRepository<Cosmos.ApplicationUser>, SecurityStorageRepository<Cosmos.ApplicationUser>>();
            services.AddScoped<IStorageRepository<Cosmos.ApplicationUserClaim>, SecurityStorageRepository<Cosmos.ApplicationUserClaim>>();
            services.AddScoped<IStorageRepository<Cosmos.ApplicationUserLogin>, SecurityStorageRepository<Cosmos.ApplicationUserLogin>>();
            services.AddScoped<IStorageRepository<Cosmos.ApplicationUserRole>, SecurityStorageRepository<Cosmos.ApplicationUserRole>>();
            services.AddScoped<IStorageRepository<Cosmos.ApplicationUserToken>, SecurityStorageRepository<Cosmos.ApplicationUserToken>>();
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

            // Register Business rules
            DomainCreateUpdateDateRule<ApplicationUser>.RegisterRule(BusinessRuleRegistry.Instance);
            DomainQueryPropertyRenameRule<ApplicationUser>.RegisterRule(BusinessRuleRegistry.Instance, "StorageKey", "Id");

            DomainCreateDateRule<AuditUser>.RegisterRule(BusinessRuleRegistry.Instance);
            DomainQueryPropertyRenameRule<AuditUser>.RegisterRule(BusinessRuleRegistry.Instance, "StorageKey", "Key");

            DomainQueryPropertyRenameRule<ApplicationUserClaim>.RegisterRule(BusinessRuleRegistry.Instance, "UserStorageKey", "UserId");
            DomainQueryPropertyRenameRule<ApplicationUserClaim>.RegisterRule(BusinessRuleRegistry.Instance, "RoleStorageKey", "RoleId");
            DomainQueryPropertyRenameRule<ApplicationUserClaim>.RegisterRule(BusinessRuleRegistry.Instance, "StorageKey", "Key");

            DomainQueryPropertyRenameRule<ApplicationUserLogin>.RegisterRule(BusinessRuleRegistry.Instance, "UserStorageKey", "UserId");
            DomainQueryPropertyRenameRule<ApplicationUserLogin>.RegisterRule(BusinessRuleRegistry.Instance, "RoleStorageKey", "RoleId");
            DomainQueryPropertyRenameRule<ApplicationUserLogin>.RegisterRule(BusinessRuleRegistry.Instance, "StorageKey", "Key");

            DomainQueryPropertyRenameRule<ApplicationUserRole>.RegisterRule(BusinessRuleRegistry.Instance, "UserStorageKey", "UserId");
            DomainQueryPropertyRenameRule<ApplicationUserRole>.RegisterRule(BusinessRuleRegistry.Instance, "RoleStorageKey", "RoleId");
            ApplicationUserRoleQueryRule.RegisterRule(BusinessRuleRegistry.Instance);

            DomainQueryPropertyRenameRule<ApplicationUserToken>.RegisterRule(BusinessRuleRegistry.Instance, "UserStorageKey", "UserId");
            DomainQueryPropertyRenameRule<ApplicationUserToken>.RegisterRule(BusinessRuleRegistry.Instance, "RoleStorageKey", "RoleId");
            DomainQueryPropertyRenameRule<ApplicationUserToken>.RegisterRule(BusinessRuleRegistry.Instance, "StorageKey", "Key");

            DomainQueryPropertyRenameRule<ApplicationRoleClaim>.RegisterRule(BusinessRuleRegistry.Instance, "UserStorageKey", "UserId");
            DomainQueryPropertyRenameRule<ApplicationRoleClaim>.RegisterRule(BusinessRuleRegistry.Instance, "RoleStorageKey", "RoleId");
            DomainQueryPropertyRenameRule<ApplicationRoleClaim>.RegisterRule(BusinessRuleRegistry.Instance, "StorageKey", "Key");

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