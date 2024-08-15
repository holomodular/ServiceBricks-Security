namespace ServiceBricks.Security
{
    /// <summary>
    /// This is a REST API client for the ApplicationUser domain object.
    /// </summary>
    public partial interface IUserApiClient : IApiClient<UserDto>, IUserApiService
    {
    }
}