using Microsoft.AspNetCore.Builder;

namespace ServiceBricks.Security.EntityFrameworkCore
{
    /// <summary>
    /// Extension methods for setting up ServiceBricks Security EntityFrameworkCore services.
    /// </summary>
    public static partial class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Flag to check if the module has been started
        /// </summary>
        public static bool ModuleStarted = false;

        /// <summary>
        /// Start ServiceBricks Security EntityFrameworkCore
        /// </summary>
        /// <param name="applicationBuilder"></param>
        /// <returns></returns>
        public static IApplicationBuilder StartServiceBricksSecurityEntityFrameworkCore(this IApplicationBuilder applicationBuilder)
        {
            // AI: Set the module started flag when complete
            ModuleStarted = true;

            // AI: Start the parent module
            applicationBuilder.StartServiceBricksSecurity();

            return applicationBuilder;
        }
    }
}