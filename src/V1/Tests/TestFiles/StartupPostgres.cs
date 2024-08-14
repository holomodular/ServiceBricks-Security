using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceBricks.Security;
using ServiceBricks.Security.Postgres;

namespace ServiceBricks.Xunit
{
    public class StartupPostgres : ServiceBricks.Startup
    {
        public StartupPostgres(IConfiguration configuration) : base(configuration)
        {
        }

        public virtual void ConfigureDevelopmentServices(IServiceCollection services)
        {
            base.CustomConfigureServices(services);
            services.AddSingleton(Configuration);
            services.AddServiceBricks(Configuration);
            services.AddServiceBricksSecurityPostgres(Configuration);

            //// Remove all background tasks/timers for unit testing
            //var logtimer = services.Where(x => x.ImplementationType == typeof(LoggingWriteMessageTimer)).FirstOrDefault();
            //if (logtimer != null)
            //    services.Remove(logtimer);

            // Register TestManagers
            services.AddScoped<ITestManager<ApplicationUserDto>, ApplicationUserTestManagerPostgres>();
            services.AddScoped<ITestManager<ApplicationUserRoleDto>, ApplicationUserRoleTestManager>();
            services.AddScoped<ITestManager<ApplicationUserClaimDto>, ApplicationUserClaimTestManager>();
            services.AddScoped<ITestManager<ApplicationUserTokenDto>, ApplicationUserTokenTestManager>();
            services.AddScoped<ITestManager<ApplicationUserLoginDto>, ApplicationUserLoginTestManager>();
            services.AddScoped<ITestManager<ApplicationRoleDto>, ApplicationRoleTestManager>();
            services.AddScoped<ITestManager<ApplicationRoleClaimDto>, ApplicationRoleClaimTestManager>();
            services.AddScoped<ITestManager<AuditUserDto>, AuditUserTestManagerPostgres>();

            services.AddServiceBricksComplete();
        }

        public virtual void Configure(IApplicationBuilder app)
        {
            base.CustomConfigure(app);
            app.StartServiceBricks();
            app.StartServiceBricksSecurityPostgres();
        }
    }
}