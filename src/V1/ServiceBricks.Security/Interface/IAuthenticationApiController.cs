using Microsoft.AspNetCore.Mvc;

namespace ServiceBricks.Security
{
    /// <summary>
    /// This is a REST API controller for authentication.
    /// </summary>
    public interface IAuthenticationApiController
    {
        ActionResult AuthenticateUser([FromBody] AccessTokenRequest request);

        Task<ActionResult> AuthenticateUserAsync([FromBody] AccessTokenRequest request);
    }
}