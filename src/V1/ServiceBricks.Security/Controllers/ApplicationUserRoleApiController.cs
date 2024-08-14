using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ServiceBricks.Security
{
    /// <summary>
    /// This is an exposed REST-based API controller for the ApplicationUserRoleDto.
    /// </summary>
    [ApiController]
    [Route("api/v{version:apiVersion}/Security/ApplicationUserRole")]
    [Produces("application/json")]
    public partial class ApplicationUserRoleApiController : AdminPolicyApiController<ApplicationUserRoleDto>, IApplicationUserRoleApiController
    {
        protected readonly IApplicationUserRoleApiService _applicationUserRoleApiService;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="applicationUserRoleApiService"></param>
        /// <param name="apiOptions"></param>
        public ApplicationUserRoleApiController(
            IApplicationUserRoleApiService applicationUserRoleApiService,
            IOptions<ApiOptions> apiOptions)
            : base(applicationUserRoleApiService, apiOptions)
        {
            _applicationUserRoleApiService = applicationUserRoleApiService;
        }
    }
}