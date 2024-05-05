using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ServiceBricks.Security.EntityFrameworkCore;

namespace ServiceBricks.Security.Cosmos
{
    /// <summary>
    /// IApplicationBuilder extensions for the Security Brick.
    /// </summary>
    public static partial class ApplicationBuilderExtensions
    {
        public static bool ModuleStarted = false;

        public static IApplicationBuilder StartServiceBricksSecurityCosmos(this IApplicationBuilder applicationBuilder)
        {
            using (var scope = applicationBuilder.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<SecurityCosmosContext>();
                context.Database.EnsureCreated();
            }

            ModuleStarted = true;

            // Start Core Security
            applicationBuilder.StartServiceBricksSecurity();

            return applicationBuilder;
        }
    }
}