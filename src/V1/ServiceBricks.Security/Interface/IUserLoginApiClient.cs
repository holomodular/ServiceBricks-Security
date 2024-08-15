namespace ServiceBricks.Security
{
    /// <summary>
    /// This is a REST API client for the ApplicationUserLogin domain object.
    /// </summary>
    public partial interface IUserLoginApiClient : IApiClient<UserLoginDto>, IUserLoginApiService
    {
    }
}