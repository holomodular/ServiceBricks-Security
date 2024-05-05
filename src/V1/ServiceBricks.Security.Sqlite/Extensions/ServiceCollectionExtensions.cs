using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using ServiceBricks.Storage.EntityFrameworkCore;
using ServiceBricks.Security.EntityFrameworkCore;

namespace ServiceBricks.Security.Sqlite
{
    /// <summary>
    /// IServiceCollection extensions for the Security Brick.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServiceBricksSecuritySqlite(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddServiceBricksSecuritySqlite(configuration, new Action<IdentityOptions>(options => new IdentityOptions()));
        }

        public static IServiceCollection AddServiceBricksSecuritySqlite(this IServiceCollection services, IConfiguration configuration, Action<IdentityOptions> identityOptions)
        {
            // Add to module registry
            ModuleRegistry.Instance.RegisterItem(typeof(SecuritySqliteModule), new SecuritySqliteModule());

            //Register Database
            var builder = new DbContextOptionsBuilder<SecuritySqliteContext>();
            string connectionString = configuration.GetSqliteConnectionString(
                SecurityEntityFrameworkCoreConstants.APPSETTING_DATABASE_CONNECTION);
            builder.UseSqlite(connectionString, x =>
            {
                x.MigrationsAssembly(typeof(SecuritySqliteContext).Assembly.GetName().Name);
            });
            services.Configure<DbContextOptions<SecuritySqliteContext>>(o => { o = builder.Options; });
            services.AddSingleton<DbContextOptions<SecuritySqliteContext>>(builder.Options);
            services.AddDbContext<SecuritySqliteContext>(c => { c = builder; }, ServiceLifetime.Scoped);

            // Register Identity
            services
                .AddIdentity<ApplicationUser, ApplicationRole>(identityOptions)
                .AddEntityFrameworkStores<SecuritySqliteContext>()
                .AddUserStore<ApplicationUserStore>()
                .AddRoleStore<ApplicationRoleStore>()
                .AddDefaultTokenProviders();

            // Add Core service
            services.AddServiceBricksSecurityEntityFrameworkCore(configuration);

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

            return services;
        }
    }
}