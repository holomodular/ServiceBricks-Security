namespace ServiceBricks.Security
{
    /// <summary>
    /// This is a REST API service for the Authentication domain object.
    /// </summary>
    public partial interface IAuthenticationApiService
    {
        /// <summary>
        /// Authenticates a user.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        IResponseItem<AccessTokenResponse> AuthenticateUser(AccessTokenRequest request);

        /// <summary>
        /// Authenticates a user.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<IResponseItem<AccessTokenResponse>> AuthenticateUserAsync(AccessTokenRequest request);
    }
}