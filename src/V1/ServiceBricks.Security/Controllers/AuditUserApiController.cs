using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ServiceBricks.Security
{
    /// <summary>
    /// This is an exposed REST API controller for the AuditUserDto domain object.
    /// </summary>
    [ApiController]
    [Route("api/v{version:apiVersion}/Security/AuditUser")]
    [Produces("application/json")]
    public partial class AuditUserApiController : AdminPolicyApiController<AuditUserDto>, IAuditUserApiController
    {
        protected readonly IAuditUserApiService _auditUserApiService;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="auditUserApiService"></param>
        /// <param name="apiOptions"></param>
        public AuditUserApiController(
            IAuditUserApiService auditUserApiService,
            IOptions<ApiOptions> apiOptions)
            : base(auditUserApiService, apiOptions)
        {
            _auditUserApiService = auditUserApiService;
        }
    }
}