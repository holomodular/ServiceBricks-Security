using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ServiceBricks.Security
{
    /// <summary>
    /// This is an exposed REST-based API controller for the ApplicationUserLogin domain object
    /// requiring the admin security policy to invoke all methods.
    /// </summary>
    [ApiController]
    [Route("api/v{version:apiVersion}/Security/ApplicationUserLogin")]
    [Produces("application/json")]
    public class ApplicationUserLoginApiController : AdminPolicyApiController<ApplicationUserLoginDto>, IApplicationUserLoginApiController
    {
        public ApplicationUserLoginApiController(
            IApplicationUserLoginApiService applicationUserLoginApiService,
            IOptions<ApiOptions> apiOptions)
            : base(applicationUserLoginApiService, apiOptions)
        {
        }
    }
}