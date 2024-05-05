using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace ServiceBricks.Security
{
    public interface IAuthenticationApiService
    {
        IResponseItem<AccessTokenResponse> AuthenticateUser(AccessTokenRequest request);

        Task<IResponseItem<AccessTokenResponse>> AuthenticateUserAsync(AccessTokenRequest request);
    }
}