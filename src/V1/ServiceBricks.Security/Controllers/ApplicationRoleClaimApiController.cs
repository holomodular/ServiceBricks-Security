using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ServiceBricks.Security
{
    /// <summary>
    /// This is an exposed REST-based API controller for the ApplicationRoleClaim domain object
    /// requiring the admin security policy to invoke all methods.
    /// </summary>
    [ApiController]
    [Route("api/v{version:apiVersion}/Security/ApplicationRoleClaim")]
    [Produces("application/json")]
    public class ApplicationRoleClaimApiController : AdminPolicyApiController<ApplicationRoleClaimDto>, IApplicationRoleClaimApiController
    {
        public ApplicationRoleClaimApiController(
            IApplicationRoleClaimApiService applicationRoleClaimApiService,
            IOptions<ApiOptions> apiOptions)
            : base(applicationRoleClaimApiService, apiOptions)
        {
        }
    }
}