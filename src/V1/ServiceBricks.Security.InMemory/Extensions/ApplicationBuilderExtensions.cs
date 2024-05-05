using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ServiceBricks.Security.EntityFrameworkCore;

namespace ServiceBricks.Security.InMemory
{
    /// <summary>
    /// IApplicationBuilder extensions for the Security Brick.
    /// </summary>
    public static partial class ApplicationBuilderExtensions
    {
        public static bool BrickStarted = false;

        public static IApplicationBuilder StartServiceBricksSecurityInMemory(this IApplicationBuilder applicationBuilder)
        {
            BrickStarted = true;

            // Start Core Security
            applicationBuilder.StartServiceBricksSecurityEntityFrameworkCore();

            return applicationBuilder;
        }
    }
}