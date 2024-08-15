namespace ServiceBricks.Security
{
    /// <summary>
    /// This is a REST API client for the ApplicationUserToken domain object.
    /// </summary>
    public partial interface IUserTokenApiClient : IApiClient<UserTokenDto>, IUserTokenApiService
    {
    }
}