using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ServiceBricks.Security
{
    /// <summary>
    /// This is an exposed REST-based API controller for the UserClaimDto.
    /// </summary>
    [ApiController]
    [Route("api/v{version:apiVersion}/Security/UserClaim")]
    [Produces("application/json")]
    public partial class UserClaimApiController : AdminPolicyApiController<UserClaimDto>, IUserClaimApiController
    {
        protected readonly IUserClaimApiService _applicationUserClaimApiService;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="applicationUserClaimApiService"></param>
        /// <param name="apiOptions"></param>
        public UserClaimApiController(
            IUserClaimApiService applicationUserClaimApiService,
            IOptions<ApiOptions> apiOptions)
            : base(applicationUserClaimApiService, apiOptions)
        {
            _applicationUserClaimApiService = applicationUserClaimApiService;
        }
    }
}