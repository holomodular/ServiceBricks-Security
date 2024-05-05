using Microsoft.AspNetCore.Hosting;
using ServiceBricks;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using ServiceBricks.Xunit;
using ServiceBricks.Security;

namespace ServiceBricks.Security.Client.Xunit
{
    public class ClientStartup : ServiceBricks.Startup
    {
        public ClientStartup(IConfiguration configuration) : base(configuration)
        {
        }

        public virtual void ConfigureDevelopmentServices(IServiceCollection services)
        {
            base.CustomConfigureServices(services);
            services.AddSingleton(Configuration);
            services.AddServiceBricks(Configuration);
            services.AddServiceBricksSecurityClient(Configuration);

            // Remove all background tasks/timers for unit testing

            // Register TestManagers
            services.AddScoped<ITestManager<ApplicationUserDto>, ApplicationUserTestManager>();
            services.AddScoped<ITestManager<ApplicationUserClaimDto>, ApplicationUserClaimTestManager>();
            services.AddScoped<ITestManager<ApplicationUserRoleDto>, ApplicationUserRoleTestManager>();
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
        }
    }
}