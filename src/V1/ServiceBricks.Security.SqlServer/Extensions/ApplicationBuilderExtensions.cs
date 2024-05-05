using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ServiceBricks.Security.EntityFrameworkCore;

namespace ServiceBricks.Security.SqlServer
{
    /// <summary>
    /// IApplicationBuilder extensions for the Security Brick.
    /// </summary>
    public static partial class ApplicationBuilderExtensions
    {
        public static bool ModuleStarted = false;

        public static IApplicationBuilder StartServiceBricksSecuritySqlServer(this IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                // Migrate
                var context = serviceScope.ServiceProvider.GetService<SecuritySqlServerContext>();
                context.Database.Migrate();
                context.SaveChanges();
            }
            ModuleStarted = true;

            // Start Core Security
            applicationBuilder.StartServiceBricksSecurityEntityFrameworkCore();

            return applicationBuilder;
        }
    }
}