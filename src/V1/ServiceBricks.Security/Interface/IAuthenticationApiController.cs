using Microsoft.AspNetCore.Mvc;

namespace ServiceBricks.Security
{
    /// <summary>
    /// This is a REST API controller for authentication.
    /// </summary>
    public partial interface IAuthenticationApiController
    {
        /// <summary>
        /// Authenticate a user.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ActionResult AuthenticateUser([FromBody] AccessTokenRequest request);

        /// <summary>
        /// Authenticate a user asynchronously.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ActionResult> AuthenticateUserAsync([FromBody] AccessTokenRequest request);
    }
}