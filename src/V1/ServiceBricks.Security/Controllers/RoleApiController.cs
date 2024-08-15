using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ServiceBricks.Security
{
    /// <summary>
    /// This is an exposed REST-based API controller for the RoleDto.
    /// </summary>
    [ApiController]
    [Route("api/v{version:apiVersion}/Security/Role")]
    [Produces("application/json")]
    public partial class RoleApiController : AdminPolicyApiController<RoleDto>, IRoleApiController
    {
        protected readonly IRoleApiService _applicationRoleApiService;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="applicationRoleApiService"></param>
        /// <param name="apiOptions"></param>
        public RoleApiController(
            IRoleApiService applicationRoleApiService,
            IOptions<ApiOptions> apiOptions)
            : base(applicationRoleApiService, apiOptions)
        {
            _applicationRoleApiService = applicationRoleApiService;
        }
    }
}