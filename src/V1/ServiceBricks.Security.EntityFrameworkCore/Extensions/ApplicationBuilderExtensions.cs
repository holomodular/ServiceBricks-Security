using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ServiceBricks.Security.EntityFrameworkCore
{
    /// <summary>
    /// IApplicationBuilder extensions for the Security Brick.
    /// </summary>
    public static partial class ApplicationBuilderExtensions
    {
        public static bool ModuleStarted = false;

        public static IApplicationBuilder StartServiceBricksSecurityEntityFrameworkCore(this IApplicationBuilder applicationBuilder)
        {
            ModuleStarted = true;

            // Start Core Security
            applicationBuilder.StartServiceBricksSecurity();

            return applicationBuilder;
        }
    }
}