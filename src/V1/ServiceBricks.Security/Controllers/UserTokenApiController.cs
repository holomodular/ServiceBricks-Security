using Microsoft.AspNetCore.Mvc;

using Microsoft.Extensions.Options;

namespace ServiceBricks.Security
{
    /// <summary>
    /// This is an exposed REST-based API controller for the UserTokenDto.
    /// </summary>
    [ApiController]
    [Route("api/v{version:apiVersion}/Security/UserToken")]
    [Produces("application/json")]
    public partial class UserTokenApiController : AdminPolicyApiController<UserTokenDto>, IUserTokenApiController
    {
        protected readonly IUserTokenApiService _applicationUserTokenApiService;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="applicationUserTokenApiService"></param>
        /// <param name="apiOptions"></param>
        public UserTokenApiController(
            IUserTokenApiService applicationUserTokenApiService,
            IOptions<ApiOptions> apiOptions)
            : base(applicationUserTokenApiService, apiOptions)
        {
            _applicationUserTokenApiService = applicationUserTokenApiService;
        }
    }
}