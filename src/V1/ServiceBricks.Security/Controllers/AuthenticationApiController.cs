using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

using ServiceQuery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ServiceBricks.Security
{
    [ApiController]
    [Route("api/v{version:apiVersion}/Security/Authentication")]
    [Produces("application/json")]
    public class AuthenticationApiController : ControllerBase, IAuthenticationApiController
    {
        private readonly IAuthenticationApiService _authenticationApiService;

        public AuthenticationApiController(
            IAuthenticationApiService authenticationApiService)
        {
            _authenticationApiService = authenticationApiService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("AuthenticateUser")]
        public ActionResult AuthenticateUser([FromBody] AccessTokenRequest request)
        {
            var resp = _authenticationApiService.AuthenticateUser(request);
            if (resp.Error)
                return GetErrorResponse(resp);
            return Ok(resp.Item);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("AuthenticateUserAsync")]
        public async Task<ActionResult> AuthenticateUserAsync([FromBody] AccessTokenRequest request)
        {
            var resp = await _authenticationApiService.AuthenticateUserAsync(request);
            if (resp.Error)
                return GetErrorResponse(resp);
            return Ok(resp.Item);
        }

        [NonAction]
        public virtual ActionResult GetErrorResponse(IResponse response)
        {
            var details = new ProblemDetails()
            {
                Title = LocalizationResource.ERROR_SYSTEM,
                Status = (int)HttpStatusCode.BadRequest,
                Detail = response.GetMessage(Environment.NewLine)
            };
            return BadRequest(details);
        }
    }
}