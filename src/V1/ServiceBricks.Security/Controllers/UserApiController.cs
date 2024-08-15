using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ServiceBricks.Security
{
    /// <summary>
    /// This is an exposed REST API controller for the UserDto domain object
    /// </summary>
    [ApiController]
    [Route("api/v{version:apiVersion}/Security/User")]
    [Produces("application/json")]
    public partial class UserApiController : AdminPolicyApiController<UserDto>, IUserApiController
    {
        protected readonly IUserApiService _applicationUserApiService;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="applicationUserApiService"></param>
        /// <param name="apiOptions"></param>
        public UserApiController(
            IUserApiService applicationUserApiService,
            IOptions<ApiOptions> apiOptions)
            : base(applicationUserApiService, apiOptions)
        {
            _applicationUserApiService = applicationUserApiService;
        }
    }
}