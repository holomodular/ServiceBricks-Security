using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceBricks.Security.EntityFrameworkCore;
using ServiceBricks.Storage.EntityFrameworkCore;

namespace ServiceBricks.Security.Cosmos
{
    /// <summary>
    /// Extensions for adding the ServiceBricks Security Cosmos module to the service collection.
    /// </summary>
    public static partial class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add the ServiceBricks Security Cosmos module to the service collection.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddServiceBricksSecurityCosmos(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddServiceBricksSecurityCosmos(configuration, new Action<IdentityOptions>(options => new IdentityOptions()));
        }

        /// <summary>
        /// Add the ServiceBricks Security Cosmos module to the service collection.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <param name="identityOptions"></param>
        /// <returns></returns>
        public static IServiceCollection AddServiceBricksSecurityCosmos(this IServiceCollection services, IConfiguration configuration, Action<IdentityOptions> identityOptions)
        {
            // AI: Add the module to the ModuleRegistry
            ModuleRegistry.Instance.RegisterItem(typeof(SecurityCosmosModule), new SecurityCosmosModule());

            // AI: Register the database for the module
            var builder = new DbContextOptionsBuilder<SecurityCosmosContext>();
            string connectionString = configuration.GetCosmosConnectionString(
                SecurityCosmosConstants.APPSETTING_CONNECTION_STRING);
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

            // AI: Add the parent module
            // AI: If the primary keys of the Cosmos models do not match the EFC module, we can't use EFC rules, so skip EFC and call start on the core module instead.
            services.AddServiceBricksSecurity(configuration); // Skip EFC

            // AI: Storage Services for the module for each domain object
            services.AddScoped<IStorageRepository<Cosmos.ApplicationRole>, SecurityStorageRepository<Cosmos.ApplicationRole>>();
            services.AddScoped<IStorageRepository<Cosmos.ApplicationRoleClaim>, SecurityStorageRepository<Cosmos.ApplicationRoleClaim>>();
            services.AddScoped<IStorageRepository<Cosmos.ApplicationUser>, SecurityStorageRepository<Cosmos.ApplicationUser>>();
            services.AddScoped<IStorageRepository<Cosmos.ApplicationUserClaim>, SecurityStorageRepository<Cosmos.ApplicationUserClaim>>();
            services.AddScoped<IStorageRepository<Cosmos.ApplicationUserLogin>, SecurityStorageRepository<Cosmos.ApplicationUserLogin>>();
            services.AddScoped<IStorageRepository<Cosmos.ApplicationUserRole>, SecurityStorageRepository<Cosmos.ApplicationUserRole>>();
            services.AddScoped<IStorageRepository<Cosmos.ApplicationUserToken>, SecurityStorageRepository<Cosmos.ApplicationUserToken>>();
            services.AddScoped<IUserAuditStorageRepository, UserAuditStorageRepository>();
            services.AddScoped<IStorageRepository<UserAudit>, UserAuditStorageRepository>();

            // AI: Add API services for the module. Each DTO should have two registrations, one for the generic IApiService<> and one for the named interface
            // AI: If the primary keys of the Cosmos models match the EFC module, we can use the EFC rules
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
            // AI: If the primary keys of the Cosmos models match the EFC module, we can use the EFC rules
            DomainCreateUpdateDateRule<ApplicationUser>.RegisterRule(BusinessRuleRegistry.Instance);
            DomainQueryPropertyRenameRule<ApplicationUser>.RegisterRule(BusinessRuleRegistry.Instance, "StorageKey", "Id");

            DomainCreateDateRule<UserAudit>.RegisterRule(BusinessRuleRegistry.Instance);
            DomainQueryPropertyRenameRule<UserAudit>.RegisterRule(BusinessRuleRegistry.Instance, "StorageKey", "Key");

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