using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace ServiceBricks.Security.Member
{
    /// <summary>
    /// IApplicationBuilder extensions for the Security Brick.
    /// </summary>
    public static partial class ApplicationBuilderExtensions
    {
        public static bool ModuleStarted = false;

        public static IApplicationBuilder StartServiceBricksSecurityMember(this IApplicationBuilder applicationBuilder)
        {
            ModuleStarted = true;
            return applicationBuilder;
        }
    }
}