using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace ServiceBricks.Security
{
    /// <summary>
    /// This is an exposed REST API controller for the AuditUser domain object.
    /// </summary>
    [ApiController]
    [Route("api/v{version:apiVersion}/Security/AuditUser")]
    [Produces("application/json")]
    public class AuditUserApiController : AdminPolicyApiController<AuditUserDto>, IAuditUserApiController
    {
        public AuditUserApiController(
            IAuditUserApiService auditUserApiService,
            IOptions<ApiOptions> apiOptions)
            : base(auditUserApiService, apiOptions)
        {
        }
    }
}