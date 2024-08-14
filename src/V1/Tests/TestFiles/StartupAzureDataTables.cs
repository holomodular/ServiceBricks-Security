using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceBricks.Security;
using ServiceBricks.Security.AzureDataTables;

namespace ServiceBricks.Xunit
{
    public class StartupAzureDataTables : ServiceBricks.Startup
    {
        public StartupAzureDataTables(IConfiguration configuration) : base(configuration)
        {
        }

        public virtual void ConfigureDevelopmentServices(IServiceCollection services)
        {
            base.CustomConfigureServices(services);
            services.AddSingleton(Configuration);
            services.AddServiceBricks(Configuration);
            services.AddServiceBricksSecurityAzureDataTables(Configuration);

            services.AddRouting(); // For LinkGenerator

            // Remove all background tasks/timers for unit testing
            //var logtimer = services.Where(x => x.ImplementationType == typeof(LoggingWriteMessageTimer)).FirstOrDefault();
            //if (logtimer != null)
            //    services.Remove(logtimer);

            // Register TestManager
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
            base.CustomConfigure(app);
            app.StartServiceBricks();
            app.StartServiceBricksSecurityAzureDataTables();
        }
    }
}