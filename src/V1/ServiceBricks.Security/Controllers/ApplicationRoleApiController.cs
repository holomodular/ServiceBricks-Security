using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ServiceBricks.Security
{
    /// <summary>
    /// This is an exposed REST-based API controller for the ApplicationRole domain object
    /// requiring the admin security policy to invoke all methods.
    /// </summary>
    [ApiController]
    [Route("api/v{version:apiVersion}/Security/ApplicationRole")]
    [Produces("application/json")]
    public class ApplicationRoleApiController : AdminPolicyApiController<ApplicationRoleDto>, IApplicationRoleApiController
    {
        public ApplicationRoleApiController(
            IApplicationRoleApiService applicationRoleApiService,
            IOptions<ApiOptions> apiOptions)
            : base(applicationRoleApiService, apiOptions)
        {
        }
    }
}