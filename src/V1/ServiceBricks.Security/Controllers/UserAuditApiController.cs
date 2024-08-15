using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ServiceBricks.Security
{
    /// <summary>
    /// This is an exposed REST API controller for the UserAuditDto domain object.
    /// </summary>
    [ApiController]
    [Route("api/v{version:apiVersion}/Security/UserAudit")]
    [Produces("application/json")]
    public partial class UserAuditApiController : AdminPolicyApiController<UserAuditDto>, IUserAuditApiController
    {
        protected readonly IUserAuditApiService _auditUserApiService;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="auditUserApiService"></param>
        /// <param name="apiOptions"></param>
        public UserAuditApiController(
            IUserAuditApiService auditUserApiService,
            IOptions<ApiOptions> apiOptions)
            : base(auditUserApiService, apiOptions)
        {
            _auditUserApiService = auditUserApiService;
        }
    }
}