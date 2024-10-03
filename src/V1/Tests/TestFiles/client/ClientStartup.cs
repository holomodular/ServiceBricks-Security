using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceBricks.Xunit;

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
            services.AddScoped<ITestManager<UserDto>, UserTestManager>();
            services.AddScoped<ITestManager<UserClaimDto>, ApplicationUserClaimTestManager>();
            services.AddScoped<ITestManager<UserRoleDto>, UserRoleTestManager>();
            services.AddScoped<ITestManager<UserTokenDto>, UserTokenTestManager>();
            services.AddScoped<ITestManager<UserLoginDto>, UserLoginTestManager>();
            services.AddScoped<ITestManager<RoleDto>, RoleTestManager>();
            services.AddScoped<ITestManager<RoleClaimDto>, RoleClaimTestManager>();
            services.AddScoped<ITestManager<UserAuditDto>, UserAuditTestManager>();

            services.AddServiceBricksComplete(Configuration);
        }

        public virtual void Configure(IApplicationBuilder app)
        {
            base.CustomConfigure(app);
            app.StartServiceBricks();
        }
    }
}