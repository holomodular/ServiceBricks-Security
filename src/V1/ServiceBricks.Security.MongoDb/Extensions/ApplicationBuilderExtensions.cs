using Microsoft.AspNetCore.Builder;

namespace ServiceBricks.Security.MongoDb
{
    /// <summary>
    /// Extensions for to start the ServiceBricks Security MongoDb module.
    /// </summary>
    public static partial class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Flag to indicate if the module has been started.
        /// </summary>
        public static bool ModuleStarted = false;

        /// <summary>
        /// Start the ServiceBricks Security MongoDb module.
        /// </summary>
        /// <param name="applicationBuilder"></param>
        /// <returns></returns>
        public static IApplicationBuilder StartServiceBricksSecurityMongoDb(this IApplicationBuilder applicationBuilder)
        {
            // AI: Flag the module as started
            ModuleStarted = true;

            // AI: Start the parent module
            applicationBuilder.StartServiceBricksSecurity();

            return applicationBuilder;
        }
    }
}