using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ServiceBricks.Security
{
    /// <summary>
    /// This is an exposed REST-based API controller for the ApplicationUserRole domain object
    /// requiring the admin security policy to invoke all methods.
    /// </summary>
    [ApiController]
    [Route("api/v{version:apiVersion}/Security/ApplicationUserRole")]
    [Produces("application/json")]
    public class ApplicationUserRoleApiController : AdminPolicyApiController<ApplicationUserRoleDto>, IApplicationUserRoleApiController
    {
        private readonly IApplicationUserRoleApiService _applicationUserRoleApiService;

        public ApplicationUserRoleApiController(
            IApplicationUserRoleApiService applicationUserRoleApiService,
            IOptions<ApiOptions> apiOptions)
            : base(applicationUserRoleApiService, apiOptions)
        {
            _applicationUserRoleApiService = applicationUserRoleApiService;
        }
    }
}