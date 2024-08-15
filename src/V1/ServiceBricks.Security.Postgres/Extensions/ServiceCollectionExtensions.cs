using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceBricks.Security.EntityFrameworkCore;
using ServiceBricks.Storage.EntityFrameworkCore;

namespace ServiceBricks.Security.Postgres
{
    /// <summary>
    /// Extensions to add the ServiceBricks Security Postgres module to the service collection.
    /// </summary>
    public static partial class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add the ServiceBricks Security Postgres module to the service collection.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddServiceBricksSecurityPostgres(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddServiceBricksSecurityPostgres(configuration, new Action<IdentityOptions>(options => new IdentityOptions()));
        }

        /// <summary>
        /// Add the ServiceBricks Security Postgres module to the service collection.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <param name="identityOptions"></param>
        /// <returns></returns>
        public static IServiceCollection AddServiceBricksSecurityPostgres(this IServiceCollection services, IConfiguration configuration, Action<IdentityOptions> identityOptions)
        {
            // AI: Add the module to the ModuleRegistry
            ModuleRegistry.Instance.RegisterItem(typeof(SecurityPostgresModule), new SecurityPostgresModule());

            // AI: Register the database for the module
            var builder = new DbContextOptionsBuilder<SecurityPostgresContext>();
            string connectionString = configuration.GetPostgresConnectionString(
                SecurityPostgresConstants.APPSETTING_CONNECTION_STRING);
            builder.UseNpgsql(connectionString, x =>
            {
                x.MigrationsAssembly(typeof(ServiceCollectionExtensions).Assembly.GetName().Name);
                x.EnableRetryOnFailure();
            });
            services.Configure<DbContextOptions<SecurityPostgresContext>>(o => { o = builder.Options; });
            services.AddSingleton<DbContextOptions<SecurityPostgresContext>>(builder.Options);
            services.AddDbContext<SecurityPostgresContext>(c => { c = builder; }, ServiceLifetime.Scoped);

            // AI: Register requirements for the module
            services
                .AddIdentity<ApplicationUser, ApplicationRole>(identityOptions)
                .AddEntityFrameworkStores<SecurityPostgresContext>()
                .AddUserStore<ApplicationUserStore>()
                .AddRoleStore<ApplicationRoleStore>()
                .AddDefaultTokenProviders();

            // AI: Add parent module
            services.AddServiceBricksSecurityEntityFrameworkCore(configuration);

            // AI: Add storage services for the module. Each domain object should have its own storage repository.
            services.AddScoped<IStorageRepository<ApplicationRole>, SecurityStorageRepository<ApplicationRole>>();
            services.AddScoped<IStorageRepository<ApplicationRoleClaim>, SecurityStorageRepository<ApplicationRoleClaim>>();
            services.AddScoped<IStorageRepository<ApplicationUser>, SecurityStorageRepository<ApplicationUser>>();
            services.AddScoped<IStorageRepository<ApplicationUserClaim>, SecurityStorageRepository<ApplicationUserClaim>>();
            services.AddScoped<IStorageRepository<ApplicationUserLogin>, SecurityStorageRepository<ApplicationUserLogin>>();
            services.AddScoped<IStorageRepository<ApplicationUserRole>, SecurityStorageRepository<ApplicationUserRole>>();
            services.AddScoped<IStorageRepository<ApplicationUserToken>, SecurityStorageRepository<ApplicationUserToken>>();
            services.AddScoped<IUserAuditStorageRepository, UserAuditStorageRepository>();
            services.AddScoped<IStorageRepository<UserAudit>, UserAuditStorageRepository>();

            return services;
        }
    }
}