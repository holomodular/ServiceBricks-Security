using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ServiceBricks.Security
{
    /// <summary>
    /// This is an exposed REST-based API controller for the ApplicationUserClaimDto.
    /// </summary>
    [ApiController]
    [Route("api/v{version:apiVersion}/Security/ApplicationUserClaim")]
    [Produces("application/json")]
    public partial class ApplicationUserClaimApiController : AdminPolicyApiController<ApplicationUserClaimDto>, IApplicationUserClaimApiController
    {
        protected readonly IApplicationUserClaimApiService _applicationUserClaimApiService;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="applicationUserClaimApiService"></param>
        /// <param name="apiOptions"></param>
        public ApplicationUserClaimApiController(
            IApplicationUserClaimApiService applicationUserClaimApiService,
            IOptions<ApiOptions> apiOptions)
            : base(applicationUserClaimApiService, apiOptions)
        {
            _applicationUserClaimApiService = applicationUserClaimApiService;
        }
    }
}