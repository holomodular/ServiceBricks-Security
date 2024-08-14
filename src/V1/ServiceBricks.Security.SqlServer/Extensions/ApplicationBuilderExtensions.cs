using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ServiceBricks.Security.EntityFrameworkCore;

namespace ServiceBricks.Security.SqlServer
{
    /// <summary>
    /// Extensions to start the ServiceBricks Security SqlServer module.
    /// </summary>
    public static partial class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Flag to indicate if the module has started.
        /// </summary>
        public static bool ModuleStarted = false;

        /// <summary>
        /// Start the ServiceBricks Security SqlServer module.
        /// </summary>
        /// <param name="applicationBuilder"></param>
        /// <returns></returns>
        public static IApplicationBuilder StartServiceBricksSecuritySqlServer(this IApplicationBuilder applicationBuilder)
        {
            // AI: Migrate the database
            using (var serviceScope = applicationBuilder.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<SecuritySqlServerContext>();
                context.Database.Migrate();
                context.SaveChanges();
            }

            // AI: Set the module started flag
            ModuleStarted = true;

            // AI: Start the parent module
            applicationBuilder.StartServiceBricksSecurityEntityFrameworkCore();

            return applicationBuilder;
        }
    }
}