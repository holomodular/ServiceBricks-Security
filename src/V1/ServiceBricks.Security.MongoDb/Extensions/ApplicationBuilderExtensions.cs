using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceBricks.Storage.MongoDb;

namespace ServiceBricks.Security.MongoDb
{
    /// <summary>
    /// IApplicationBuilder extensions for the Security Brick.
    /// </summary>
    public static partial class ApplicationBuilderExtensions
    {
        public static bool ModuleStarted = false;

        public static IApplicationBuilder StartServiceBricksSecurityMongoDb(this IApplicationBuilder applicationBuilder)
        {
            ModuleStarted = true;

            applicationBuilder.StartServiceBricksSecurity();

            return applicationBuilder;
        }
    }
}