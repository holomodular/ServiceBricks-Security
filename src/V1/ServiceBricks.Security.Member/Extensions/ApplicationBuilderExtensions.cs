using Microsoft.AspNetCore.Builder;

namespace ServiceBricks.Security.Member
{
    /// <summary>
    /// Extensions for the IApplicationBuilder interface.
    /// </summary>
    public static partial class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Flag to indicate if the brick has been started.
        /// </summary>
        public static bool ModuleStarted = false;

        /// <summary>
        /// Start the ServiceBricks Security Member brick.
        /// </summary>
        /// <param name="applicationBuilder"></param>
        /// <returns></returns>
        public static IApplicationBuilder StartServiceBricksSecurityMember(this IApplicationBuilder applicationBuilder)
        {
            // AI: Flag the module as started
            ModuleStarted = true;

            return applicationBuilder;
        }
    }
}