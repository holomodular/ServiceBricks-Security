using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ServiceBricks.Security
{
    /// <summary>
    /// This is an exposed REST-based API controller for the UserRoleDto.
    /// </summary>
    [ApiController]
    [Route("api/v{version:apiVersion}/Security/UserRole")]
    [Produces("application/json")]
    public partial class UserRoleApiController : AdminPolicyApiController<UserRoleDto>, IUserRoleApiController
    {
        protected readonly IUserRoleApiService _applicationUserRoleApiService;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="applicationUserRoleApiService"></param>
        /// <param name="apiOptions"></param>
        public UserRoleApiController(
            IUserRoleApiService applicationUserRoleApiService,
            IOptions<ApiOptions> apiOptions)
            : base(applicationUserRoleApiService, apiOptions)
        {
            _applicationUserRoleApiService = applicationUserRoleApiService;
        }
    }
}