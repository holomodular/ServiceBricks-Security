using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using ServiceQuery;

namespace ServiceBricks.Security
{
    /// <summary>
    /// Extension methods to start the ServiceBricks.Security module.
    /// </summary>
    public static partial class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Flag to indicate if the module has been started.
        /// </summary>
        public static bool ModuleStarted = false;

        /// <summary>
        /// Start the ServiceBricks.Security module.
        /// </summary>
        /// <param name="applicationBuilder"></param>
        /// <returns></returns>
        public static IApplicationBuilder StartServiceBricksSecurity(this IApplicationBuilder applicationBuilder)
        {
            // AI: Initialize Data. Ensure admin and user roles are created
            using (var serviceScope = applicationBuilder.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                // AI: Get the role service
                var roleService = serviceScope.ServiceProvider.GetService<IRoleApiService>();

                // AI: Either roleservice is not registered because running unittest without provider (ok) or this module added out of order (bad)
                if (roleService == null)
                    return applicationBuilder;

                // AI: Query for roles needed
                var sq = new ServiceQueryRequestBuilder().
                    IsEqual(nameof(RoleDto.Name), ServiceBricksConstants.SECURITY_ROLE_ADMIN_NAME)
                    .Or()
                    .IsEqual(nameof(RoleDto.Name), ServiceBricksConstants.SECURITY_ROLE_USER_NAME)
                    .Build();
                var respRoles = roleService.Query(sq);

                // AI: Create required role for admin
                if (respRoles.Success && !respRoles.Item.List.Any(x => string.Compare(x.NormalizedName, ServiceBricksConstants.SECURITY_ROLE_ADMIN_NAME, true) == 0))
                {
                    _ = roleService.Create(new RoleDto()
                    {
                        Name = ServiceBricksConstants.SECURITY_ROLE_ADMIN_NAME,
                        NormalizedName = ServiceBricksConstants.SECURITY_ROLE_ADMIN_NAME.ToUpper()
                    });
                }

                // AI: Create required roles for user
                if (respRoles.Success && !respRoles.Item.List.Any(x => string.Compare(x.NormalizedName, ServiceBricksConstants.SECURITY_ROLE_USER_NAME, true) == 0))
                {
                    _ = roleService.Create(new RoleDto()
                    {
                        Name = ServiceBricksConstants.SECURITY_ROLE_USER_NAME,
                        NormalizedName = ServiceBricksConstants.SECURITY_ROLE_USER_NAME.ToUpper()
                    });
                }
            }

            // AI: Set the module started flag
            ModuleStarted = true;

            return applicationBuilder;
        }
    }
}