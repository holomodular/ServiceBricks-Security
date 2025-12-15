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
            services.AddScoped<ITestManager<UserDto>, MongoDbUserTestManager>();
            services.AddScoped<ITestManager<UserRoleDto>, MongoDbUserRoleTestManager>();
            services.AddScoped<ITestManager<UserClaimDto>, MongoDbUserClaimTestManager>();
            services.AddScoped<ITestManager<UserTokenDto>, MongoDbUserTokenTestManager>();
            services.AddScoped<ITestManager<UserLoginDto>, MongoDbUserLoginTestManager>();
            services.AddScoped<ITestManager<RoleDto>, MongoDbRoleTestManager>();
            services.AddScoped<ITestManager<RoleClaimDto>, MongoDbRoleClaimTestManager>();
            services.AddScoped<ITestManager<UserAuditDto>, MongoDbUserAuditTestManager>();

            services.AddServiceBricksComplete(Configuration);
        }

        public virtual void Configure(IApplicationBuilder app)
        {
            base.CustomConfigure(app);
            app.StartServiceBricks();
        }
    }
}