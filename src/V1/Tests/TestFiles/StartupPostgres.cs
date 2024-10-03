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
            services.AddScoped<ITestManager<UserDto>, UserTestManagerPostgres>();
            services.AddScoped<ITestManager<UserRoleDto>, UserRoleTestManager>();
            services.AddScoped<ITestManager<UserClaimDto>, ApplicationUserClaimTestManager>();
            services.AddScoped<ITestManager<UserTokenDto>, UserTokenTestManager>();
            services.AddScoped<ITestManager<UserLoginDto>, UserLoginTestManager>();
            services.AddScoped<ITestManager<RoleDto>, RoleTestManager>();
            services.AddScoped<ITestManager<RoleClaimDto>, RoleClaimTestManager>();
            services.AddScoped<ITestManager<UserAuditDto>, UserAuditTestManagerPostgres>();

            services.AddServiceBricksComplete(Configuration);
        }

        public virtual void Configure(IApplicationBuilder app)
        {
            base.CustomConfigure(app);
            app.StartServiceBricks();
        }
    }
}