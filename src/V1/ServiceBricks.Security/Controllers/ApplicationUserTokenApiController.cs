using Microsoft.AspNetCore.Mvc;

using Microsoft.Extensions.Options;

namespace ServiceBricks.Security
{
    /// <summary>
    /// This is an exposed REST-based API controller for the ApplicationUserTokenDto.
    /// </summary>
    [ApiController]
    [Route("api/v{version:apiVersion}/Security/ApplicationUserToken")]
    [Produces("application/json")]
    public partial class ApplicationUserTokenApiController : AdminPolicyApiController<ApplicationUserTokenDto>, IApplicationUserTokenApiController
    {
        protected readonly IApplicationUserTokenApiService _applicationUserTokenApiService;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="applicationUserTokenApiService"></param>
        /// <param name="apiOptions"></param>
        public ApplicationUserTokenApiController(
            IApplicationUserTokenApiService applicationUserTokenApiService,
            IOptions<ApiOptions> apiOptions)
            : base(applicationUserTokenApiService, apiOptions)
        {
            _applicationUserTokenApiService = applicationUserTokenApiService;
        }
    }
}