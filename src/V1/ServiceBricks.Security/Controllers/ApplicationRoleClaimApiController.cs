using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ServiceBricks.Security
{
    /// <summary>
    /// This is an exposed REST-based API controller for the ApplicationRoleClaimDto.
    /// </summary>
    [ApiController]
    [Route("api/v{version:apiVersion}/Security/ApplicationRoleClaim")]
    [Produces("application/json")]
    public partial class ApplicationRoleClaimApiController : AdminPolicyApiController<ApplicationRoleClaimDto>, IApplicationRoleClaimApiController
    {
        protected readonly IApplicationRoleClaimApiService _applicationRoleClaimApiService;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="applicationRoleClaimApiService"></param>
        /// <param name="apiOptions"></param>
        public ApplicationRoleClaimApiController(
            IApplicationRoleClaimApiService applicationRoleClaimApiService,
            IOptions<ApiOptions> apiOptions)
            : base(applicationRoleClaimApiService, apiOptions)
        {
            _applicationRoleClaimApiService = applicationRoleClaimApiService;
        }
    }
}