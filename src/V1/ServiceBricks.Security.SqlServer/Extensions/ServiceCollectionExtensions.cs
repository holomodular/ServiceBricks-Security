using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using ServiceBricks.Storage.EntityFrameworkCore;
using ServiceBricks.Security.EntityFrameworkCore;

namespace ServiceBricks.Security.SqlServer
{
    /// <summary>
    /// IServiceCollection extensions for the Security Brick.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServiceBricksSecuritySqlServer(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddServiceBricksSecuritySqlServer(configuration, new Action<IdentityOptions>(options => new IdentityOptions()));
        }

        public static IServiceCollection AddServiceBricksSecuritySqlServer(this IServiceCollection services, IConfiguration configuration, Action<IdentityOptions> identityOptions)
        {
            // Add to module registry
            ModuleRegistry.Instance.RegisterItem(typeof(SecuritySqlServerModule), new SecuritySqlServerModule());

            //Register Database
            var builder = new DbContextOptionsBuilder<SecuritySqlServerContext>();
            string connectionString = configuration.GetSqlServerConnectionString(
                SecuritySqlServerConstants.APPSETTING_DATABASE_CONNECTION);
            builder.UseSqlServer(connectionString, x =>
            {
                x.MigrationsAssembly(typeof(ServiceCollectionExtensions).Assembly.GetName().Name);
                x.EnableRetryOnFailure();
            });
            services.Configure<DbContextOptions<SecuritySqlServerContext>>(o => { o = builder.Options; });
            services.AddSingleton<DbContextOptions<SecuritySqlServerContext>>(builder.Options);
            services.AddDbContext<SecuritySqlServerContext>(c => { c = builder; }, ServiceLifetime.Scoped);

            // Register Identity
            services
                .AddIdentity<ApplicationUser, ApplicationRole>(identityOptions)
                .AddEntityFrameworkStores<SecuritySqlServerContext>()
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