using Microsoft.AspNetCore.Hosting;
using ServiceBricks;
using ServiceBricks.Security;
using ServiceBricks.Security.InMemory;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;

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
            services.AddScoped<ITestManager<ApplicationUserDto>, ApplicationUserTestManager>();
            services.AddScoped<ITestManager<ApplicationUserRoleDto>, ApplicationUserRoleTestManager>();
            services.AddScoped<ITestManager<ApplicationUserClaimDto>, ApplicationUserClaimTestManager>();
            services.AddScoped<ITestManager<ApplicationUserTokenDto>, ApplicationUserTokenTestManager>();
            services.AddScoped<ITestManager<ApplicationUserLoginDto>, ApplicationUserLoginTestManager>();
            services.AddScoped<ITestManager<ApplicationRoleDto>, ApplicationRoleTestManager>();
            services.AddScoped<ITestManager<ApplicationRoleClaimDto>, ApplicationRoleClaimTestManager>();
            services.AddScoped<ITestManager<AuditUserDto>, AuditUserTestManager>();

            services.AddServiceBricksComplete();
        }

        public virtual void Configure(IApplicationBuilder app)
        {
            app.StartServiceBricks();
            app.StartServiceBricksSecurityInMemory();
            base.CustomConfigure(app);
        }
    }
}