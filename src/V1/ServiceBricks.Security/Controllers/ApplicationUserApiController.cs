using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using ServiceQuery;

namespace ServiceBricks.Security
{
    /// <summary>
    /// This is an exposed REST API controller for the ApplicationUser domain object
    /// requiring the admin security policy to invoke all methods.
    /// </summary>
    [ApiController]
    [Route("api/v{version:apiVersion}/Security/ApplicationUser")]
    [Produces("application/json")]
    public class ApplicationUserApiController : AdminPolicyApiController<ApplicationUserDto>, IApplicationUserApiController
    {
        private readonly IApplicationUserApiService _applicationUserApiService;

        public ApplicationUserApiController(
            IApplicationUserApiService applicationUserApiService,
            IOptions<ApiOptions> apiOptions)
            : base(applicationUserApiService, apiOptions)
        {
            _applicationUserApiService = applicationUserApiService;
        }
    }
}