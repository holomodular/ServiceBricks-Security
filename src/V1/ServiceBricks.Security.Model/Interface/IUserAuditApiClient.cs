namespace ServiceBricks.Security
{
    /// <summary>
    /// This is a REST API client for the UserSecurity domain object.
    /// </summary>
    public partial interface IUserAuditApiClient : IApiClient<UserAuditDto>, IUserAuditApiService
    {
    }
}