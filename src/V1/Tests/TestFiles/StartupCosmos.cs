using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceBricks.Security;
using ServiceBricks.Security.Cosmos;

namespace ServiceBricks.Xunit
{
    public class StartupCosmos : ServiceBricks.Startup
    {
        public StartupCosmos(IConfiguration configuration) : base(configuration)
        {
        }

        public virtual void ConfigureDevelopmentServices(IServiceCollection services)
        {
            base.CustomConfigureServices(services);
            services.AddSingleton(Configuration);
            services.AddServiceBricks(Configuration);
            services.AddServiceBricksSecurityCosmos(Configuration);

            // Remove all background tasks/timers for unit testing
            //var logtimer = services.Where(x => x.ImplementationType == typeof(LoggingWriteMessageTimer)).FirstOrDefault();
            //if (logtimer != null)
            //    services.Remove(logtimer);

            // Register TestManagers
            services.AddScoped<ITestManager<UserDto>, UserTestManager>();
            services.AddScoped<ITestManager<UserRoleDto>, UserRoleTestManager>();
            services.AddScoped<ITestManager<UserClaimDto>, ApplicationUserClaimTestManager>();
            services.AddScoped<ITestManager<UserTokenDto>, UserTokenTestManager>();
            services.AddScoped<ITestManager<UserLoginDto>, UserLoginTestManager>();
            services.AddScoped<ITestManager<RoleDto>, RoleTestManager>();
            services.AddScoped<ITestManager<RoleClaimDto>, RoleClaimTestManager>();
            services.AddScoped<ITestManager<UserAuditDto>, UserAuditTestManager>();

            services.AddServiceBricksComplete();
        }

        public virtual void Configure(IApplicationBuilder app)
        {
            base.CustomConfigure(app);
            app.StartServiceBricks();
            app.StartServiceBricksSecurityCosmos();
        }
    }
}