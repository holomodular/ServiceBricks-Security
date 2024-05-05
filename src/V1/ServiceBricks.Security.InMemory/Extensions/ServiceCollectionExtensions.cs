using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using ServiceBricks.Security.EntityFrameworkCore;

namespace ServiceBricks.Security.InMemory
{
    /// <summary>
    /// IServiceCollection extensions for the Security Brick.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServiceBricksSecurityInMemory(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddBrickSecurityEntityFrameworkCoreInMemory(configuration, new Action<IdentityOptions>(options => new IdentityOptions()));
        }

        public static IServiceCollection AddBrickSecurityEntityFrameworkCoreInMemory(this IServiceCollection services, IConfiguration configuration, Action<IdentityOptions> identityOptions)
        {
            // Add to module registry
            ModuleRegistry.Instance.RegisterItem(typeof(SecurityInMemoryModule), new SecurityInMemoryModule());

            // Register Database
            var builder = new DbContextOptionsBuilder<SecurityInMemoryContext>();
            builder.UseInMemoryDatabase(Guid.NewGuid().ToString(), b => b.EnableNullChecks(false));
            services.Configure<DbContextOptions<SecurityInMemoryContext>>(o => { o = builder.Options; });
            services.AddSingleton<DbContextOptions<SecurityInMemoryContext>>(builder.Options);
            services.AddDbContext<SecurityInMemoryContext>(c => { c = builder; }, ServiceLifetime.Scoped);

            // Register Identity
            services
                .AddIdentity<ApplicationUser, ApplicationRole>(identityOptions)
                .AddEntityFrameworkStores<SecurityInMemoryContext>()
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