using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ServiceBricks.Security
{
    /// <summary>
    /// This is an exposed REST-based API controller to authenticate users.
    /// </summary>
    [ApiController]
    [Route("api/v{version:apiVersion}/Security/Authentication")]
    [Produces("application/json")]
    public partial class AuthenticationApiController : ControllerBase, IAuthenticationApiController
    {
        protected readonly IAuthenticationApiService _authenticationApiService;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="authenticationApiService"></param>
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