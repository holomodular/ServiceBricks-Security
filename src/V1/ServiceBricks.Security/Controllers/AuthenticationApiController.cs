using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ServiceBricks.Security
{
    /// <summary>
    /// This is an exposed REST-based API controller to authenticate users.
    /// </summary>
    [ApiController]
    [Route("api/v{version:apiVersion}/Security/Authentication")]
    [Produces("application/json")]
    public partial class AuthenticationApiController : ApiControllerBase, IAuthenticationApiController
    {
        protected readonly IAuthenticationApiService _authenticationApiService;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="authenticationApiService"></param>
        public AuthenticationApiController(
            IAuthenticationApiService authenticationApiService,
            IOptions<ApiOptions> apiOptions) : base(apiOptions)
        {
            _authenticationApiService = authenticationApiService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("AuthenticateUser")]
        public ActionResult AuthenticateUser([FromBody] AccessTokenRequest request)
        {
            if (_apiOptions.DisableSyncMethods)
            {
                Response response = new Response();
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_API_SYNC_DISABLED));
                return GetErrorResponse(response);
            }

            var resp = _authenticationApiService.AuthenticateUser(request);
            if (resp.Error)
                return GetErrorResponse(resp);

            if (UseModernResponse())
                return Ok(resp);
            else
                return Ok(resp.Item);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("AuthenticateUserAsync")]
        public async Task<ActionResult> AuthenticateUserAsync([FromBody] AccessTokenRequest request, CancellationToken cancellationToken = default)
        {
            var resp = await _authenticationApiService.AuthenticateUserAsync(request);
            if (resp.Error)
                return GetErrorResponse(resp);

            if (UseModernResponse())
                return Ok(resp);
            else
                return Ok(resp.Item);
        }
    }
}