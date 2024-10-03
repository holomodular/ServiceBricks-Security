using Microsoft.Extensions.DependencyInjection;
using ServiceQuery;

namespace ServiceBricks.Security
{
    /// <summary>
    /// This rule is executed when the ServiceBricks module is added.
    /// </summary>
    public sealed class SecurityModuleStartRule : BusinessRule
    {
        /// <summary>
        /// Register the rule
        /// </summary>
        public static void Register(IBusinessRuleRegistry registry)
        {
            registry.Register(
                typeof(ModuleStartEvent<SecurityModule>),
                typeof(SecurityModuleStartRule));
        }

        /// <summary>
        /// UnRegister the rule.
        /// </summary>
        public static void UnRegister(IBusinessRuleRegistry registry)
        {
            registry.UnRegister(
                typeof(ModuleStartEvent<SecurityModule>),
                typeof(SecurityModuleStartRule));
        }

        /// <summary>
        /// Execute the business rule.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override IResponse ExecuteRule(IBusinessRuleContext context)
        {
            var response = new Response();

            // AI: Make sure the context object is the correct type
            if (context == null || context.Object == null)
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, "context"));
                return response;
            }
            var e = context.Object as ModuleStartEvent<SecurityModule>;
            if (e == null || e.DomainObject == null || e.ApplicationBuilder == null)
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, "context"));
                return response;
            }

            // AI: Perform logic
            // AI: Initialize Data. Ensure admin and user roles are created
            using (var serviceScope = e.ApplicationBuilder.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                // AI: Get the role service
                var roleService = serviceScope.ServiceProvider.GetService<IRoleApiService>();

                // AI: Either roleservice is not registered because running unittest without provider (ok) or this module added out of order (bad)
                if (roleService == null)
                    return response;

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
            return response;
        }
    }
}