namespace ServiceBricks.Security
{
    /// <summary>
    /// This is a REST API client for the ApplicationUser domain object.
    /// </summary>
    public partial interface IApplicationUserApiClient : IApiClient<ApplicationUserDto>, IApplicationUserApiService
    {
    }
}