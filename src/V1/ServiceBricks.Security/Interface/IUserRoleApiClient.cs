namespace ServiceBricks.Security
{
    /// <summary>
    /// This is a REST API client for the ApplicationUserRole domain object.
    /// </summary>
    public partial interface IUserRoleApiClient : IApiClient<UserRoleDto>, IUserRoleApiService
    {
    }
}