using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ServiceBricks.Security
{
    /// <summary>
    /// This is a REST API client for the ApplicationUserRole entity.
    /// </summary>
    public partial class UserRoleApiClient : ApiClient<UserRoleDto>, IUserRoleApiClient
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="loggerFactory"></param>
        /// <param name="httpClientFactory"></param>
        /// <param name="configuration"></param>
        public UserRoleApiClient(
            ILoggerFactory loggerFactory,
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration)
            : base(loggerFactory, httpClientFactory, configuration.GetApiConfig(SecurityConstants.APPSETTING_CLIENT_APICONFIG))
        {
            ApiResource = @"Security/UserRole";
        }
    }
}