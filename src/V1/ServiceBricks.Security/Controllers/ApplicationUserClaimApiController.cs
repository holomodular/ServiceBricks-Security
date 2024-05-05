using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ServiceBricks.Security
{
    /// <summary>
    /// This is an exposed REST-based API controller for the ApplicationUserClaim domain object
    /// requiring the admin security policy to invoke all methods.
    /// </summary>
    [ApiController]
    [Route("api/v{version:apiVersion}/Security/ApplicationUserClaim")]
    [Produces("application/json")]
    public class ApplicationUserClaimApiController : AdminPolicyApiController<ApplicationUserClaimDto>, IApplicationUserClaimApiController
    {
        public ApplicationUserClaimApiController(
            IApplicationUserClaimApiService applicationUserClaimApiService,
            IOptions<ApiOptions> apiOptions)
            : base(applicationUserClaimApiService, apiOptions)
        {
        }
    }
}