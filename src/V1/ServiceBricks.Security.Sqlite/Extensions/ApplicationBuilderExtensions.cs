using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ServiceBricks.Security.EntityFrameworkCore;

namespace ServiceBricks.Security.Sqlite
{
    /// <summary>
    /// IApplicationBuilder extensions for the Security Brick.
    /// </summary>
    public static partial class ApplicationBuilderExtensions
    {
        public static bool ModuleStarted = false;

        public static IApplicationBuilder StartServiceBricksSecuritySqlite(this IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                // Migrate
                var context = serviceScope.ServiceProvider.GetService<SecuritySqliteContext>();
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