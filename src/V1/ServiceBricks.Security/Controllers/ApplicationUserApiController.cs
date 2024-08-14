using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ServiceBricks.Security
{
    /// <summary>
    /// This is an exposed REST API controller for the ApplicationUserDto domain object
    /// </summary>
    [ApiController]
    [Route("api/v{version:apiVersion}/Security/ApplicationUser")]
    [Produces("application/json")]
    public partial class ApplicationUserApiController : AdminPolicyApiController<ApplicationUserDto>, IApplicationUserApiController
    {
        protected readonly IApplicationUserApiService _applicationUserApiService;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="applicationUserApiService"></param>
        /// <param name="apiOptions"></param>
        public ApplicationUserApiController(
            IApplicationUserApiService applicationUserApiService,
            IOptions<ApiOptions> apiOptions)
            : base(applicationUserApiService, apiOptions)
        {
            _applicationUserApiService = applicationUserApiService;
        }
    }
}