using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace ServiceBricks.Security.Cosmos
{
    /// <summary>
    /// Extensions to start the ServiceBricks Security Cosmos module.
    /// </summary>
    public static partial class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Flag to indicate if the module has started.
        /// </summary>
        public static bool ModuleStarted = false;

        /// <summary>
        /// Start the ServiceBricks Security Cosmos module.
        /// </summary>
        /// <param name="applicationBuilder"></param>
        /// <returns></returns>
        public static IApplicationBuilder StartServiceBricksSecurityCosmos(this IApplicationBuilder applicationBuilder)
        {
            // AI: Ensure the database is created
            using (var scope = applicationBuilder.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<SecurityCosmosContext>();
                context.Database.EnsureCreated();
            }

            // AI: Set the module started flag
            ModuleStarted = true;

            // AI: Start the parent module.
            // AI: If the primary keys of the Cosmos models do not match the EFC module, we can't use it rules, so skip EFC and call start on the core module instead.
            applicationBuilder.StartServiceBricksSecurity(); // Skip EFC

            return applicationBuilder;
        }
    }
}