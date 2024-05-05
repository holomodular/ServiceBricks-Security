using Microsoft.AspNetCore.Mvc;

using Microsoft.Extensions.Options;

namespace ServiceBricks.Security
{
    /// <summary>
    /// This is an exposed REST-based API controller for the ApplicationUserToken domain object
    /// requiring the admin security policy to invoke all methods.
    /// </summary>
    [ApiController]
    [Route("api/v{version:apiVersion}/Security/ApplicationUserToken")]
    [Produces("application/json")]
    public class ApplicationUserTokenApiController : AdminPolicyApiController<ApplicationUserTokenDto>, IApplicationUserTokenApiController
    {
        private readonly IApplicationUserTokenApiService _applicationUserTokenApiService;

        public ApplicationUserTokenApiController(
            IApplicationUserTokenApiService applicationUserTokenApiService,
            IOptions<ApiOptions> apiOptions)
            : base(applicationUserTokenApiService, apiOptions)
        {
            _applicationUserTokenApiService = applicationUserTokenApiService;
        }
    }
}