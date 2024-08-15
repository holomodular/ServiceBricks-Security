using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ServiceBricks.Security
{
    /// <summary>
    /// This is an exposed REST-based API controller for the UserLoginDto.
    /// </summary>
    [ApiController]
    [Route("api/v{version:apiVersion}/Security/UserLogin")]
    [Produces("application/json")]
    public partial class UserLoginApiController : AdminPolicyApiController<UserLoginDto>, IUserLoginApiController
    {
        protected readonly IUserLoginApiService _applicationUserLoginApiService;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="applicationUserLoginApiService"></param>
        /// <param name="apiOptions"></param>
        public UserLoginApiController(
            IUserLoginApiService applicationUserLoginApiService,
            IOptions<ApiOptions> apiOptions)
            : base(applicationUserLoginApiService, apiOptions)
        {
            _applicationUserLoginApiService = applicationUserLoginApiService;
        }
    }
}