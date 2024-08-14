using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceBricks.Security;
using ServiceBricks.Security.MongoDb;

namespace ServiceBricks.Xunit
{
    public class StartupMongoDb : ServiceBricks.Startup
    {
        public StartupMongoDb(IConfiguration configuration) : base(configuration)
        {
        }

        public virtual void ConfigureDevelopmentServices(IServiceCollection services)
        {
            base.CustomConfigureServices(services);
            services.AddSingleton(Configuration);
            services.AddServiceBricks(Configuration);
            services.AddServiceBricksSecurityMongoDb(Configuration);

            // Remove all background tasks/timers for unit testing
            //var logtimer = services.Where(x => x.ImplementationType == typeof(LoggingWriteMessageTimer)).FirstOrDefault();
            //if (logtimer != null)
            //    services.Remove(logtimer);

            // Register TestManagers
            services.AddScoped<ITestManager<ApplicationUserDto>, MongoDbApplicationUserTestManager>();
            services.AddScoped<ITestManager<ApplicationUserRoleDto>, MongoDbApplicationUserRoleTestManager>();
            services.AddScoped<ITestManager<ApplicationUserClaimDto>, MongoDbApplicationUserClaimTestManager>();
            services.AddScoped<ITestManager<ApplicationUserTokenDto>, MongoDbApplicationUserTokenTestManager>();
            services.AddScoped<ITestManager<ApplicationUserLoginDto>, MongoDbApplicationUserLoginTestManager>();
            services.AddScoped<ITestManager<ApplicationRoleDto>, MongoDbApplicationRoleTestManager>();
            services.AddScoped<ITestManager<ApplicationRoleClaimDto>, MongoDbApplicationRoleClaimTestManager>();
            services.AddScoped<ITestManager<AuditUserDto>, MongoDbAuditUserTestManager>();

            services.AddServiceBricksComplete();
        }

        public virtual void Configure(IApplicationBuilder app)
        {
            base.CustomConfigure(app);
            app.StartServiceBricks();
            app.StartServiceBricksSecurityMongoDb();
        }
    }
}