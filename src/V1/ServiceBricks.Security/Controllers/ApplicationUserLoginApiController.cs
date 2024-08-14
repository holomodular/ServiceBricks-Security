using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ServiceBricks.Security
{
    /// <summary>
    /// This is an exposed REST-based API controller for the ApplicationUserLoginDto.
    /// </summary>
    [ApiController]
    [Route("api/v{version:apiVersion}/Security/ApplicationUserLogin")]
    [Produces("application/json")]
    public partial class ApplicationUserLoginApiController : AdminPolicyApiController<ApplicationUserLoginDto>, IApplicationUserLoginApiController
    {
        protected readonly IApplicationUserLoginApiService _applicationUserLoginApiService;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="applicationUserLoginApiService"></param>
        /// <param name="apiOptions"></param>
        public ApplicationUserLoginApiController(
            IApplicationUserLoginApiService applicationUserLoginApiService,
            IOptions<ApiOptions> apiOptions)
            : base(applicationUserLoginApiService, apiOptions)
        {
            _applicationUserLoginApiService = applicationUserLoginApiService;
        }
    }
}