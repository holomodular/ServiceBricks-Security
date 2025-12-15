using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceBricks.Security;
using ServiceBricks.Security.InMemory;

namespace ServiceBricks.Xunit
{
    public class StartupInMemory : ServiceBricks.Startup
    {
        public StartupInMemory(IConfiguration configuration) : base(configuration)
        {
        }

        public virtual void ConfigureDevelopmentServices(IServiceCollection services)
        {
            base.CustomConfigureServices(services);
            services.AddSingleton(Configuration);
            services.AddServiceBricks(Configuration);
            services.AddServiceBricksSecurityInMemory(Configuration);

            // Register Database AS TRANSIENT
            var builder = new DbContextOptionsBuilder<SecurityInMemoryContext>();
            builder.UseInMemoryDatabase(Guid.NewGuid().ToString(), b => b.EnableNullChecks(false));
            services.Configure<DbContextOptions<SecurityInMemoryContext>>(o => { o = builder.Options; });
            services.AddSingleton<DbContextOptions<SecurityInMemoryContext>>(builder.Options);
            services.AddDbContext<SecurityInMemoryContext>(c => { c = builder; }, ServiceLifetime.Transient);

            // Remove all background tasks/timers for unit testing
            //var logtimer = services.Where(x => x.ImplementationType == typeof(LoggingWriteMessageTimer)).FirstOrDefault();
            //if (logtimer != null)
            //    services.Remove(logtimer);

            // Register TestManagers
            services.AddScoped<ITestManager<UserDto>, UserTestManager>();
            services.AddScoped<ITestManager<UserRoleDto>, UserRoleTestManager>();
            services.AddScoped<ITestManager<UserClaimDto>, UserClaimTestManager>();
            services.AddScoped<ITestManager<UserTokenDto>, UserTokenTestManager>();
            services.AddScoped<ITestManager<UserLoginDto>, UserLoginTestManager>();
            services.AddScoped<ITestManager<RoleDto>, RoleTestManager>();
            services.AddScoped<ITestManager<RoleClaimDto>, RoleClaimTestManager>();
            services.AddScoped<ITestManager<UserAuditDto>, UserAuditTestManager>();

            services.AddServiceBricksComplete(Configuration);
        }

        public virtual void Configure(IApplicationBuilder app)
        {
            app.StartServiceBricks();
            base.CustomConfigure(app);
        }
    }
}