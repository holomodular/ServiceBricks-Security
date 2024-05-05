using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace ServiceBricks.Security
{
    /// <summary>
    /// IApplicationBuilder extensions for the Security Brick.
    /// </summary>
    public static partial class ApplicationBuilderExtensions
    {
        public static bool ModuleStarted = false;

        public static IApplicationBuilder StartServiceBricksSecurity(this IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                //Initialize Data
                var roleService = serviceScope.ServiceProvider.GetService<IApplicationRoleApiService>();

                // Either roleservice is not registered because running unittest without provider (ok) or this module added out of order (bad)
                if (roleService == null)
                    return applicationBuilder;
                var respRoles = roleService.Query(new ServiceQuery.ServiceQueryRequest());

                // Create roles
                if (respRoles.Success && !respRoles.Item.List.Any(x => x.NormalizedName == SecurityConstants.ROLE_ADMIN_NAME.ToUpper()))
                {
                    _ = roleService.Create(new ApplicationRoleDto()
                    {
                        Name = SecurityConstants.ROLE_ADMIN_NAME,
                        NormalizedName = SecurityConstants.ROLE_ADMIN_NAME.ToUpper()
                    });
                }
                if (respRoles.Success && !respRoles.Item.List.Any(x => x.NormalizedName == SecurityConstants.ROLE_USER_NAME.ToUpper()))
                {
                    _ = roleService.Create(new ApplicationRoleDto()
                    {
                        Name = SecurityConstants.ROLE_USER_NAME,
                        NormalizedName = SecurityConstants.ROLE_USER_NAME.ToUpper()
                    });
                }
            }

            ModuleStarted = true;
            return applicationBuilder;
        }
    }
}