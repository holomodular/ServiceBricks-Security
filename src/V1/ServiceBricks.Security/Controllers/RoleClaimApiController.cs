using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ServiceBricks.Security
{
    /// <summary>
    /// This is an exposed REST-based API controller for the RoleClaimDto.
    /// </summary>
    [ApiController]
    [Route("api/v{version:apiVersion}/Security/RoleClaim")]
    [Produces("application/json")]
    public partial class RoleClaimApiController : AdminPolicyApiController<RoleClaimDto>, IRoleClaimApiController
    {
        protected readonly IRoleClaimApiService _applicationRoleClaimApiService;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="applicationRoleClaimApiService"></param>
        /// <param name="apiOptions"></param>
        public RoleClaimApiController(
            IRoleClaimApiService applicationRoleClaimApiService,
            IOptions<ApiOptions> apiOptions)
            : base(applicationRoleClaimApiService, apiOptions)
        {
            _applicationRoleClaimApiService = applicationRoleClaimApiService;
        }
    }
}