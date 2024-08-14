using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ServiceBricks.Security
{
    /// <summary>
    /// This is an exposed REST-based API controller for the ApplicationRoleDto.
    /// </summary>
    [ApiController]
    [Route("api/v{version:apiVersion}/Security/ApplicationRole")]
    [Produces("application/json")]
    public partial class ApplicationRoleApiController : AdminPolicyApiController<ApplicationRoleDto>, IApplicationRoleApiController
    {
        protected readonly IApplicationRoleApiService _applicationRoleApiService;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="applicationRoleApiService"></param>
        /// <param name="apiOptions"></param>
        public ApplicationRoleApiController(
            IApplicationRoleApiService applicationRoleApiService,
            IOptions<ApiOptions> apiOptions)
            : base(applicationRoleApiService, apiOptions)
        {
            _applicationRoleApiService = applicationRoleApiService;
        }
    }
}